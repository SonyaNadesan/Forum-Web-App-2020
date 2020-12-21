using Application.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class LogoutService : ILogoutService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ServiceResponse<ApplicationUser>> Logout(string email)
        {
            var response = new ServiceResponse<ApplicationUser>();

            var user = _userManager.FindByNameAsync(email);

            if (user.Result != null)
            {
                response.Result = user.Result;
                await _signInManager.SignOutAsync();
                return response;
            }

            response.ErrorMessage = "Not signed in.";
            return response;
        }
    }
}