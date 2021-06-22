using Application.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using static Application.Domain.Enums;

namespace Application.Services.Email
{
    public class EmailConfigurationBuilder
    {
        private string _body;
        private bool _isBodyHtml;
        private string _subject;
        private string _recipients;
        private string _fromAddress;
        private List<FileStreamAndName> _files = new List<FileStreamAndName>();

        internal EmailConfigurationBuilder(string recipients, string fromAddress)
        {
            _recipients = recipients;
            _fromAddress = fromAddress;
        }

        public EmailConfigurationBuilder SetBody(string body, EmailBodyType bodyType)
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

        public EmailConfigurationBuilder AddFile(MemoryStream attachmentStream, string filename)
        {
            if (attachmentStream != null)
            {
                filename = string.IsNullOrEmpty(filename) ? "attachment" + (_files.Count + 2) : filename;

                _files.Add(new FileStreamAndName() { AttachmentStream = attachmentStream, FileName = filename });
            }

            return this;
        }

        public EmailConfigurationBuilder AddFile(FileStreamAndName file)
        {
            if (file.AttachmentStream != null)
            {
                file.FileName = string.IsNullOrEmpty(file.FileName) ? "attachment" + (_files.Count + 2) : file.FileName;
                _files.Add(file);
            }

            return this;
        }

        public EmailConfigurationBuilder SetSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public MailMessage Build()
        {
            Attachment attachment = null;

            var msg = new MailMessage()
            {
                From = new MailAddress(_fromAddress),
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
