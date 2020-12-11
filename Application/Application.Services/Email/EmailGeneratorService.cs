using System.Net.Mail;

namespace Application.Services.Email
{
    public class EmailGeneratorService : IEmailGeneratorService
    {
        public MailMessage CreateEmail(string toEmail, string fromEmail, string subject, string body)
        {
            var to = new MailAddress(toEmail);
            var from = new MailAddress(fromEmail);

            var mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = body;

            return mail;
        }
    }
}
