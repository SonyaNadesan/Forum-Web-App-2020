﻿using Application.Domain;
using Application.Services.Email;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class AccountRecoveryService : IAccountRecoveryService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailGeneratorService _emailGeneratorService;
        private readonly IPasswordChangeService _passwordChangeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRecoveryService(IEmailSenderService emailSenderService, IEmailGeneratorService emailGeneratorService,
                                   IPasswordChangeService passwordChangeService, UserManager<ApplicationUser> userManager)
        {
            _emailSenderService = emailSenderService;
            _emailGeneratorService = emailGeneratorService;
            _passwordChangeService = passwordChangeService;
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

            var passwordAssignmentResponse = _passwordChangeService.AssignRandomlyGeneratedPassword(user, out string password);

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
