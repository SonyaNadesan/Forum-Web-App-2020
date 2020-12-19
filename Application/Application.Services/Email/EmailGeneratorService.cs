using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Mail;

namespace Application.Services.Email
{
    public class EmailGeneratorService : IEmailGeneratorService
    {
        private readonly IConfiguration Configuration;

        public EmailGeneratorService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public MailMessage CreateEmail(string bodyAsText, string receipants, string subject, MemoryStream attachmentStream = null, string filename = "New File")
        {
            Attachment attachment = null;

            if (attachmentStream != null)
            {
                attachmentStream.Position = 0;

                attachment = new Attachment(attachmentStream, filename, System.Net.Mime.MediaTypeNames.Application.Pdf);
                attachment.ContentDisposition.FileName = filename + ".pdf";
                attachment.ContentDisposition.CreationDate = DateTime.Now;
                attachment.ContentDisposition.ModificationDate = DateTime.Now;
            }

            var msg = new MailMessage()
            {
                From = new MailAddress(Configuration.GetSection("FromEmail").Value),
                Subject = subject,
                Body = bodyAsText,
                IsBodyHtml = true
            };

            msg.Attachments.Add(attachment);

            msg.To.Add(receipants);

            return msg;
        }
    }
}
