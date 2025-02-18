using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TourNest.Models;
using TourNest.Models.BloggingPlatform;

namespace TourNest.Controllers.Blogs
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly TourNestContext _context;
        private readonly IConfiguration _configuration;

        public BlogController(TourNestContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("CreateBlog")]
        public async Task<IActionResult> CreateBlog([FromForm] BlogView view)
        {
            // Retrieve the user ID from token claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User ID is missing in token claims" });

            if (!Guid.TryParse(userId, out var parsedUserId))
                return BadRequest(new { message = "Invalid User ID format in token claims" });

            if (view.Image == null)
                return BadRequest(new { message = "Image is required" });

            var ext = Path.GetExtension(view.Image.FileName);
            if (!new[] { ".png", ".jpg", ".jpeg" }.Contains(ext.ToLower()))
                return BadRequest(new { message = "Only PNG, JPG, and JPEG are allowed" });

            if (view.Image.Length > 1_000_000)
                return BadRequest(new { message = "File size must not exceed 1MB" });

            // Upload the image to Azure Blob Storage
            string blobUri;
            try
            {
                blobUri = await UploadToBlobAsync(view.Image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Image upload failed: {ex.Message}");
            }

            // Save blog data to the database
            var blog = new Blogging
            {
                BlogId = Guid.NewGuid(),
                UserId = parsedUserId,
                CreatedAt = DateTime.UtcNow,
                Location = view.Location,
                Caption = view.Caption,
                Image = blobUri // Store the blob URI
            };

            await _context.Bloggings.AddAsync(blog);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Blog created successfully", blog });
        }

        [HttpGet("AllBlogs")]
        public async Task<IActionResult> Blogs()
        {
            var blogs = await _context.Bloggings
                .Join(_context.Users,
                      blog => blog.UserId,
                      user => user.UserId,
                      (blog, user) => new
                      {
                          blog.BlogId,
                          blog.UserId,
                          blog.CreatedAt,
                          blog.Location,
                          blog.Caption,
                          ImageUrl = blog.Image, // Return the Azure Blob URI directly
                          user.Username,
                          user.ProfilePhoto // Assuming this is the path or URL of the profile photo
                      })
                .ToListAsync();

            return Ok(blogs);
      
}


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserById()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Extract user ID from the claims
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var userId = Guid.Parse(userIdClaim.Value); // Parse the user ID to GUID

            var user = await _context.Users
                .Include(u => u.Bloggings)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound(new { message = "Create Your First Blog" });
            }

            var result = new
            {
                user.Username,
                user.ProfilePhoto,
                Blogs = user.Bloggings.Select(b => new
                {
                    b.BlogId,
                    b.Caption,
                    b.Location,
                    b.CreatedAt,
                    b.Image
                })
            };

            return Ok(result);
        }


        private async Task<string> UploadToBlobAsync(IFormFile file)
        {
            string connectionString = _configuration["AzureStorage:ConnectionString"];
            string containerName = "blogging"; // Container name

            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            await containerClient.CreateIfNotExistsAsync();

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
