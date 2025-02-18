using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourNest.Models;
using TourNest.Models.Rating_and_Reviews;

namespace TourNest.Controllers.Reviews
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly TourNestContext context;

        public ReviewController(TourNestContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewDto reviewdto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User authentication failed. Please log in again." });
                }

                if (!Guid.TryParse(userId, out Guid parsedUserId))
                {
                    return Unauthorized(new { message = "Invalid User ID." });
                }

                var review = new Review
                {
                    ReviewId = Guid.NewGuid(),
                    UserId = parsedUserId,
                    CreatedAt = reviewdto.CreatedAt,
                    Rating = reviewdto.Rating,
                    Review1 = reviewdto.Review1,
                };

                await context.Reviews.AddAsync(review);
                await context.SaveChangesAsync();

                return Ok(new { message = "Review added successfully", review });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> AllReviews()
        {
            var reviews = await (from review in context.Reviews
                                 join user in context.Users on review.UserId equals user.UserId
                                 select new ReviewDto
                                 {
                                     ReviewId = review.ReviewId,
                                     Rating = review.Rating,
                                     Review1 = review.Review1,
                                     CreatedAt = review.CreatedAt,
                                     Username = user.Username, // Assuming the column for username is 'UserName'
                                     ProfilePhoto = user.ProfilePhoto // Assuming the column for profile photo is 'ProfilePhotoUrl'
                                 }).ToListAsync();

            return Ok(reviews);
        }




    }
}
