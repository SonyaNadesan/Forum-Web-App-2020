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

        public async Task<IdentityResult> RegisterAccount(string email)
        {
            var user = new ApplicationUser(email)
            {
                Email = email,
                UserName = email,
                EmailConfirmed = false,
                Salt = Guid.NewGuid().ToString(),
                RegistrationConfirmed = false
            };

            var password = RandomStringGeneratorService.Generate(20);

            var response = await _userManager.CreateAsync(user, password + user.Salt);

            if (response.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");

                var fromEmail = Configuration.GetSection("FromEmail").Value;
                var message = "Please confirm your account by logging in using the following password: " + password;
                var mail = _emailGeneratorService.CreateEmail(email, fromEmail, "Registration", message);

                _emailService.Send(mail);
            }

            return response;
        }

        public async Task<ServiceResponse<IdentityResult>> ConfirmRegistration(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var response = new ServiceResponse<IdentityResult>();

            if (user != null)
            {
                var confirmEmail = await _userManager.ConfirmEmailAsync(user, password + user.Salt);

                if (confirmEmail.Succeeded)
                {
                    response.Result = confirmEmail;
                    await _signInManager.SignInAsync(user, false);
                    return response;
                }

                response.Result = await _userManager.AccessFailedAsync(user);

                var isUserLockedOut = await _userManager.IsLockedOutAsync(user);

                response.ErrorMessage = isUserLockedOut ? "User has been locked out." : "Sorry, please try again.";

                return response;
            }

            response.ErrorMessage = "Sorry, somrthing went wrong.";

            return response;
        }
    }
}