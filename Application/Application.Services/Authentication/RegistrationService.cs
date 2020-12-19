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
                                   IUserStore<ApplicationUser> userStore, IRandomStringGeneratorService randomStringGeneratorService,
                                   IPdfGeneratorService<string> pdfGeneratorService, UserManager<ApplicationUser> userManager)
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

            var user = new ApplicationUser(email)
            {
                Email = email,
                UserName = email,
                EmailConfirmed = false,
                Salt = Guid.NewGuid().ToString(),
                RegistrationConfirmed = false
            };

            var password = _randomStringGeneratorService.Generate(10);

            var creationResult = await _userManager.CreateAsync(user, password + user.Salt);

            if (creationResult.Succeeded)
            {
                response.Result = user;

                var htmlTemplate = Configuration.GetSection("RegistrationEmail").Value;

                var body = File.ReadAllText(htmlTemplate).Replace("#password#", password);

                var attachment = _pdfGeneratorService.Generate(htmlTemplate);

                var mail = _emailGeneratorService.CreateEmail(body, email, "Registration", attachment, "Registration Confirmation PDF");

                var emailSent = _emailService.Send(mail);

                if (!emailSent.IsValid)
                {
                    response.ErrorMessage = "User was created but email failed to send.";
                }

                return response;
            }

            response.ErrorMessage = "Sorry, something went wrong. Please try again.";

            return response;
        }
    }
}