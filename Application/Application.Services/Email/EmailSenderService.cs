using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Application.Services.Email
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration Configuration;
        private readonly SmtpClient _smtpClient;

        public EmailSenderService(IConfiguration configuration)
        {
            Configuration = configuration;

            _smtpClient = new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = Configuration.GetSection("EmailDirectory").Value
            };
        }

        public ServiceResponse<bool> Send(string from, IList<string> to, string subject, string body)
        {
            if (string.IsNullOrEmpty(from) || to == null || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
            {
                var response = new ServiceResponse<bool>
                {
                    Result = false
                };

                response.ErrorMessage = "One or more of the parameters passed in is/are null or empty.";
                return response;
            }

            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body
            };

            foreach (string email in to)
            {
                message.To.Add(new MailAddress(email));
            }

            return Send(message);
        }

        public ServiceResponse<bool> Send(MailMessage message)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                _smtpClient.Send(message);
                response.Result = true;
            }
            catch (Exception ex)
            {
                var msg = string.Format("There was a problem with the Email Service: {0}", ex.Message);
                response.Result = false;
                response.ErrorMessage = msg;
            }

            return response;
        }
    }
}
