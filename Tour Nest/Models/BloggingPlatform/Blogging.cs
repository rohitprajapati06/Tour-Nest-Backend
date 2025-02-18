

using TourNest.Models.UserProfile;

namespace TourNest.Models.BloggingPlatform
{
    public partial class Blogging
    {
        public Guid BlogId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Location { get; set; }

        public string? Caption { get; set; }

        public string Image { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
