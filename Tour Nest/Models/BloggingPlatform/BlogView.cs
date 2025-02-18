using TourNest.Models.UserProfile;
using Microsoft.AspNetCore.Http; // For IFormFile


namespace TourNest.Models.BloggingPlatform
{
    public class BlogView
    {
        public Guid BlogId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Location { get; set; }

        public string? Caption { get; set; }

        public IFormFile Image { get; set; } = null!;

    }
}
