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

        public async Task<ValidationResult<ApplicationUser, LoginStatus>> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return UserNotFound(user);
            }

            var isUserLockedOut = await _userManager.IsLockedOutAsync(user);

            if (isUserLockedOut)
            {
                return UserLockedOut(user);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, PasswordSaltService.GetPasswordWithSalt(password, user.Salt));

            if (!isPasswordValid)
            {
                return await InvalidPassword(user);
            }

            var response = !user.RegistrationConfirmed ? await RequiresPasswordChange(user) : Success(user);

            await _signInManager.SignInAsync(user, false);

            return response;
        }

        private ValidationResult<ApplicationUser, LoginStatus> UserNotFound(ApplicationUser user)
        {
            return new ValidationResult<ApplicationUser, LoginStatus>()
            {
                ValidatedEntity = user,
                Status = LoginStatus.UserNotFound
            };
        }

        private ValidationResult<ApplicationUser, LoginStatus> UserLockedOut(ApplicationUser user)
        {
            return new ValidationResult<ApplicationUser, LoginStatus>()
            {
                ValidatedEntity = user,
                Status = LoginStatus.LockedOut
            };
        }

        private async Task<ValidationResult<ApplicationUser, LoginStatus>> InvalidPassword(ApplicationUser user)
        {
            await _userManager.AccessFailedAsync(user);

            return new ValidationResult<ApplicationUser, LoginStatus>()
            {
                ValidatedEntity = user,
                Status = LoginStatus.Failed
            };
        }

        private async Task<ValidationResult<ApplicationUser, LoginStatus>> RequiresPasswordChange(ApplicationUser user)
        {
            user.RegistrationConfirmed = true;

            await _userManager.UpdateAsync(user);

            return new ValidationResult<ApplicationUser, LoginStatus>()
            {
                ValidatedEntity = user,
                Status = LoginStatus.ConfirmedButNeedsPasswordChange
            };
        }

        private ValidationResult<ApplicationUser, LoginStatus> Success(ApplicationUser user)
        {
            return new ValidationResult<ApplicationUser, LoginStatus>()
            {
                ValidatedEntity = user,
                Status = LoginStatus.Success
            };
        }
    }
}
