using Application.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Sonya.AspNetCore.Common;
using Sonya.AspNetCore.Common.Email;
using System.Threading.Tasks;
using static Sonya.AspNetCore.Common.Domain.Enums;

namespace Application.Services.Authentication
{
    public class AccountRecoveryService : IAccountRecoveryService
    {
        private readonly IConfiguration Configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IPasswordAssignmentService _passwordAssignmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRecoveryService(IConfiguration configuration, IEmailSenderService emailSenderService, IEmailBuilder emailGeneratorService,
                                   IPasswordAssignmentService passwordAssignmentService, UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _emailSenderService = emailSenderService;
            _emailBuilder = emailGeneratorService;
            _passwordAssignmentService = passwordAssignmentService;
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

            var passwordAssignmentResponse = _passwordAssignmentService.AssignRandomlyGeneratedPassword(user, out string password);

            if (passwordAssignmentResponse.IsValid)
            {
                var userUpdateResponse = await _userManager.UpdateAsync(user);

                if (userUpdateResponse.Succeeded)
                {
                    response = SendEmail(user, password);
                }
            }

            return response;
        }

        private ServiceResponse<ApplicationUser> SendEmail(ApplicationUser user, string newPassword)
        {
            var response = new ServiceResponse<ApplicationUser>(user);

            var message = "Your new password is: " + newPassword;

            var mail = _emailBuilder.SetRecipientsAndFromAddress(user.Email, Configuration.GetSection("FromEmail").Value)
                                    .SetSubject("Account Recovery")
                                    .SetBody(message, EmailBodyType.RegularString)
                                    .Build();

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
