using System.Net.Mail;

namespace Application.Services.Email
{
    public interface IEmailGeneratorService
    {
        MailMessage CreateEmail(string toEmail, string fromEmail, string subject, string body);
    }
}
