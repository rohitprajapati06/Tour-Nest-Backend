namespace TourNest.Models.UserProfile
{
    public class VerifyOtpDTO
    {
        public string Email { get; set; } = null!;
        public int Otp { get; set; }
    }
}
