namespace TourNest.Services.Otp
{
    public class OtpServices : IOtpServices
    {
        private readonly Random random;

        public OtpServices(Random random)
        {
            this.random = random;
        }
        public async Task<int> GenerateOtpAsync()
        {
            await Task.Delay(10);
            return random.Next(100000, 1000000);
        }
    }
}
