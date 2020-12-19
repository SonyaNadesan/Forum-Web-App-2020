using Application.Domain;
using Application.Services.Email;
using Application.Services.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IConfiguration Configuration;
        private readonly IEmailSenderService _emailService;
        private readonly IEmailGeneratorService _emailGeneratorService;
        private readonly IRandomStringGeneratorService _randomStringGeneratorService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegistrationService(IConfiguration configuration, IEmailSenderService emailService,
                                   IEmailGeneratorService emailGeneratorService, IUserStore<ApplicationUser> userStore,
                                   UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            Configuration = configuration;
            _emailService = emailService;
            _emailGeneratorService = emailGeneratorService;
            _userManager = userManager;
            _signInManager = signInManager;
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

                var message = "Please log into your account by using the following password: " + password;

                var mail = _emailGeneratorService.CreateEmail(message, email, "Registration", new System.IO.MemoryStream());

                _emailService.Send(mail);
            }

            return response;
        }
    }
}