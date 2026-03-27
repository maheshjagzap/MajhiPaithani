namespace MajhiPaithani.Application.Interfaces.IAuthService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
