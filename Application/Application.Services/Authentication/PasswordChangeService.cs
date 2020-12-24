using Application.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class PasswordChangeService : IPasswordChangeService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordChangeService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResponse<ApplicationUser>> ChangePassword(string username, string password, string newPassword, string confirmPassword)
        {
            var response = new ServiceResponse<ApplicationUser>();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                response.ErrorMessage = "User not found.";
                return response;
            }

            response.Result = user;

            if(newPassword != confirmPassword)
            {
                response.ErrorMessage = "Passwords do not match.";
                return response;
            }

            if (password != newPassword)
            {
                var passwordWithSalt = PasswordSaltService.GetPasswordWithSalt(password, user.Salt);
                var newPasswordWithSalt = PasswordSaltService.GetPasswordWithSalt(newPassword, user.Salt);

                var passwordChangeResponse = await _userManager.ChangePasswordAsync(user, passwordWithSalt, newPasswordWithSalt);

                if (!passwordChangeResponse.Succeeded)
                {
                    response.ErrorMessage = "Something went wrong.";
                    return response;
                }
            }

            return response;
        }

    }
}
