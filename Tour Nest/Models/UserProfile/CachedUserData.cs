namespace TourNest.Models.UserProfile
{
    public class CachedUserData
    {
        public int Otp { get; set; }
        public RegisterDTO User { get; set; } = null!;
    }
}
