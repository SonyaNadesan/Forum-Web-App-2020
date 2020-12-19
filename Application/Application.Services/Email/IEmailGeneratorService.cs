using System.IO;
using System.Net.Mail;

namespace Application.Services.Email
{
    public interface IEmailGeneratorService
    {
        MailMessage CreateEmail(string bodyText, string receipants, string subject, MemoryStream attachmentStream = null, string filename = "New File");
    }
}
