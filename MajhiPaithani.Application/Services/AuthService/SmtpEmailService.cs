using MajhiPaithani.Application.Interfaces.IAuthService;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MajhiPaithani.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public SmtpEmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtp = _config.GetSection("SmtpSettings");
            using var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]!))
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtp["Username"], smtp["Password"]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mail = new MailMessage
            {
                From = new MailAddress(smtp["From"]!, "MajhiPaithani"),
                Subject = subject,
                Body = body
            };
            mail.To.Add(toEmail);

            await client.SendMailAsync(mail);
        }
    }
}
