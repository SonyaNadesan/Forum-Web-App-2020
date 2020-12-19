using Application.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using static Application.Domain.Enums;

namespace Application.Services.Authentication
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ValidationResult<ApplicationUser, LoginStatus>> IsLoginCredentialsValid(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            var response = new ValidationResult<ApplicationUser, LoginStatus>(user);

            if (user == null)
            {
                response.Status = LoginStatus.Failed;
                return response;
            }

            var isUserLockedOut = await _userManager.IsLockedOutAsync(user);

            if (isUserLockedOut)
            {
                response.Status = LoginStatus.LockedOut;
                return response;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, password + user.Salt);

            if (!passwordCheck)
            {
                await _userManager.AccessFailedAsync(user);
                response.Status = LoginStatus.Failed;
                return response;
            }

            await _signInManager.SignInAsync(user, false);
            return response;
        }
    }
}
