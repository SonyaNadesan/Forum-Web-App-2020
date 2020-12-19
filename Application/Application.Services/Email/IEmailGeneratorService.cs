using Application.Domain;
using System.IO;
using System.Net.Mail;
using static Application.Domain.Enums;

namespace Application.Services.Email
{
    public interface IEmailGeneratorService
    {
        EmailGeneratorService SetBody(string body, EmailBodyType bodyType);
        EmailGeneratorService SetRecipients(string recipients);

        EmailGeneratorService AddFile(MemoryStream attachmentStream, string filename);

        EmailGeneratorService AddFile(FileStreamAndName file);

        EmailGeneratorService SetSubject(string subject);

        MailMessage CreateEmail();
    }
}
