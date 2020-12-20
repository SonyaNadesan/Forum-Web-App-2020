using Application.Domain;
using Application.Services.Email;
using Application.Services.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class AccountRecoveryService : IAccountRecoveryService
    {
        private readonly IConfiguration Configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailGeneratorService _emailGeneratorService;
        private readonly IRandomStringGeneratorService _randomStringGeneratorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRecoveryService(IConfiguration configuration, IEmailSenderService emailSenderService, IEmailGeneratorService emailGeneratorService,
                                   IRandomStringGeneratorService randomStringGeneratorService, UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _emailSenderService = emailSenderService;
            _emailGeneratorService = emailGeneratorService;
            _randomStringGeneratorService = randomStringGeneratorService;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<ApplicationUser>> Recover(string email)
        {
            var response = new ServiceResponse<ApplicationUser>();

            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                response.ErrorMessage = "User not found.";
                return response;
            }

            var newPassword = _randomStringGeneratorService.Generate(10);

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, PasswordSaltService.GetPasswordWithSalt(newPassword, user.Salt));

            var userUpdateResponse = await _userManager.UpdateAsync(user);

            if (userUpdateResponse.Succeeded)
            {
                response = SendEmail(user, newPassword);
            }

            return response;
        }

        private ServiceResponse<ApplicationUser> SendEmail(ApplicationUser user, string newPassword)
        {
            var response = new ServiceResponse<ApplicationUser>(user);

            var message = "Your new password is: " + newPassword;

            var mail = _emailGeneratorService.SetBody(message, Enums.EmailBodyType.RegularString)
                                             .SetRecipients(user.Email)
                                             .SetSubject("Account Recovery")
                                             .CreateEmail();

            var emailResponse = _emailSenderService.Send(mail);

            if (!emailResponse.IsValid)
            {
                response.ErrorMessage = "Email failed to send.";
                return response;
            }

            return response;
        }
    }
}
