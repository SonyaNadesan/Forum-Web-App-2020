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

        public async Task<ServiceResponse<ApplicationUser>> ChangePassword(string username, string password, string newPassword)
        {
            var response = new ServiceResponse<ApplicationUser>();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                response.ErrorMessage = "User not found.";
                return response;
            }

            if (password != newPassword)
            {
                var passwordChangeResponse = await _userManager.ChangePasswordAsync(user, password, newPassword);

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
