using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TourNest.Services.User.ProfilePicture;

namespace TourNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly string _blobConnectionString;
        private readonly string _containerName;
        private readonly IUserRepository _userRepository;

        public UserProfileController(IConfiguration configuration, IUserRepository userRepository)
        {
            _blobConnectionString = configuration["AzureStorage:ConnectionString"];
            _containerName = configuration["AzureStorage:ContainerName"];
            _userRepository = userRepository;
        }

        [HttpPut("UploadProfilePhoto")]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid or missing token.");

            try
            {
                string fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                // Initialize blob container client
                BlobContainerClient containerClient = new BlobContainerClient(_blobConnectionString, _containerName);
                await containerClient.CreateIfNotExistsAsync();

                // Upload or overwrite the file
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                // Public URI of the uploaded file
                string publicUri = blobClient.Uri.ToString();

                // Update the database with the blob's URI
                await _userRepository.UpdateUserProfilePhotoAsync(Guid.Parse(userId), publicUri);

                return Ok(new { Message = "File uploaded or updated successfully.", FileUri = publicUri });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
