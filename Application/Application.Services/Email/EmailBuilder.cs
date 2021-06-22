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
    public class EmailBuilder : IEmailBuilder
    {
        public EmailConfigurationBuilder SetRecipientsAndFromAddress(string recipients, string fromAddress)
        {
            return new EmailConfigurationBuilder(recipients, fromAddress);
        }
    }
}
