using TourNest.Models.UserProfile;

namespace TourNest.Models.Rating_and_Reviews
{
    public partial class Review
    {
        public Guid ReviewId { get; set; }

        public Guid UserId { get; set; }

        public DateOnly CreatedAt { get; set; }

        public int Rating { get; set; }

        public string Review1 { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
