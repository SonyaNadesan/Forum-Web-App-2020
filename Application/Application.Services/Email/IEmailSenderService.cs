using System.Collections.Generic;
using System.Net.Mail;

namespace Application.Services.Email
{
    public interface IEmailSenderService
    {
        ServiceResponse<bool> Send(string from, IList<string> to, string subject, string body);
        ServiceResponse<bool> Send(MailMessage message);
    }
}
