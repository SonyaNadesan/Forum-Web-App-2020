using Application.Domain;
using Application.Services.Documents;
using Application.Services.Email;
using Application.Services.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IConfiguration Configuration;
        private readonly IEmailSenderService _emailService;
        private readonly IEmailGeneratorService _emailGeneratorService;
        private readonly IRandomStringGeneratorService _randomStringGeneratorService;
        private readonly IPdfGeneratorService<string> _pdfGeneratorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationService(IConfiguration configuration, IEmailSenderService emailService, IEmailGeneratorService emailGeneratorService, 
                                   IRandomStringGeneratorService randomStringGeneratorService, IPdfGeneratorService<string> pdfGeneratorService, 
                                   UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _emailService = emailService;
            _emailGeneratorService = emailGeneratorService;
            _randomStringGeneratorService = randomStringGeneratorService;
            _pdfGeneratorService = pdfGeneratorService;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<ApplicationUser>> RegisterAccount(string email)
        {
            var response = new ServiceResponse<ApplicationUser>();

            var user = InitializeUser(email);

            var password = _randomStringGeneratorService.Generate(10);

            var addUserToDbResponse = await _userManager.CreateAsync(user, PasswordSaltService.GetPasswordWithSalt(password, user.Salt));

            if (addUserToDbResponse.Succeeded)
            {
                response.Result = user;

                var emailSent = SendRegistrationConfirmationEmail(email, password);

                if (!emailSent.IsValid)
                {
                    response.ErrorMessage = "User was created but email failed to send.";
                }

                return response;
            }

            var isEmailInUse = await _userManager.FindByNameAsync(email);

            response.ErrorMessage = isEmailInUse != null ? "There is already an account against that email." : "Failed to create user.";

            return response;
        }

        private ApplicationUser InitializeUser(string email)
        {
            return new ApplicationUser(email)
            {
                Email = email,
                UserName = email,
                EmailConfirmed = false,
                Salt = Guid.NewGuid().ToString(),
                RegistrationConfirmed = false
            };
        }

        private ServiceResponse<bool> SendRegistrationConfirmationEmail(string email, string password)
        {
            var htmlBody = File.ReadAllText(Configuration.GetSection("RegistrationEmail").Value).Replace("#password#", password);

            var attachment1 = new FileStreamAndName() { AttachmentStream = _pdfGeneratorService.Generate(htmlBody), FileName = "A1" };
            var attachment2 = new FileStreamAndName() { AttachmentStream = _pdfGeneratorService.Generate(htmlBody), FileName = "A2" };

            var mail = _emailGeneratorService.SetBody(htmlBody, Enums.EmailBodyType.HtmlString)
                                             .SetSubject("Registration")
                                             .SetRecipients(email)
                                             .AddFile(attachment1)
                                             .AddFile(attachment2)
                                             .CreateEmail();

            return _emailService.Send(mail);
        }
    }
}