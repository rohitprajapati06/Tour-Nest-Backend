namespace TourNest.Services.Otp
{
    public interface IOtpServices
   {
        Task<int> GenerateOtpAsync();
    }
}
