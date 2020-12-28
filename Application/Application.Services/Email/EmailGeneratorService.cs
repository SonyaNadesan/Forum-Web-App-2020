using Application.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using static Application.Domain.Enums;

namespace Application.Services.Email
{
    public class EmailGeneratorService : IEmailGeneratorService
    {
        private readonly IConfiguration Configuration;

        private string _body;
        private bool _isBodyHtml;
        private string _subject;
        private string _recipients;
        private List<FileStreamAndName> _files = new List<FileStreamAndName>();

        public EmailGeneratorService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public EmailGeneratorService SetBody(string body, EmailBodyType bodyType)
        {
            switch (bodyType)
            {
                case EmailBodyType.HtmlFile:
                    _body = File.ReadAllText(body);
                    _isBodyHtml = true;
                    break;
                case EmailBodyType.HtmlString:
                    _body = body;
                    _isBodyHtml = true;
                    break;
                case EmailBodyType.RegularString:
                    _body = body;
                    _isBodyHtml = false;
                    break;
                default:
                    _body = body;
                    break;
            }

            return this;
        }

        public EmailGeneratorService SetRecipients(string recipients)
        {
            _recipients = recipients;
            return this;
        }

        public EmailGeneratorService AddFile(MemoryStream attachmentStream, string filename)
        {
            if (attachmentStream != null)
            {
                filename = string.IsNullOrEmpty(filename) ? "attachment" + (_files.Count + 2) : filename;

                _files.Add(new FileStreamAndName() { AttachmentStream = attachmentStream, FileName = filename });
            }

            return this;
        }

        public EmailGeneratorService AddFile(FileStreamAndName file)
        {
            if (file.AttachmentStream != null)
            {
                file.FileName = string.IsNullOrEmpty(file.FileName) ? "attachment" + (_files.Count + 2) : file.FileName;
                _files.Add(file);
            }

            return this;
        }

        public EmailGeneratorService SetSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public MailMessage CreateEmail()
        {
            Attachment attachment = null;

            var msg = new MailMessage()
            {
                From = new MailAddress(Configuration.GetSection("FromEmail").Value),
                Subject = _subject,
                Body = _body,
                IsBodyHtml = _isBodyHtml
            };

            if (_files.Any())
            {
                foreach (var file in _files)
                {
                    file.AttachmentStream.Position = 0;

                    attachment = new Attachment(file.AttachmentStream, file.FileName, System.Net.Mime.MediaTypeNames.Application.Pdf);
                    attachment.ContentDisposition.FileName = file.FileName + ".pdf";
                    attachment.ContentDisposition.CreationDate = DateTime.Now;
                    attachment.ContentDisposition.ModificationDate = DateTime.Now;
                    msg.Attachments.Add(attachment);
                }
            }

            msg.To.Add(_recipients);

            return msg;
        }
    }
}
