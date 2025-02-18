namespace TourNest.Services.Mail
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
