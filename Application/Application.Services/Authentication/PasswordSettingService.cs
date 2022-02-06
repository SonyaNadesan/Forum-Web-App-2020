using Application.Domain;
using Microsoft.AspNetCore.Identity;
using Sonya.AspNetCore.Common;
using Sonya.AspNetCore.Common.Shared;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class PasswordSettingService : IPasswordAssignmentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRandomStringGeneratorService _randomStringGeneratorService;

        public PasswordSettingService(UserManager<ApplicationUser> userManager, IRandomStringGeneratorService randomStringGeneratorService)
        {
            _userManager = userManager;
            _randomStringGeneratorService = randomStringGeneratorService;
        }

        public async Task<ServiceResponse<ApplicationUser>> ChangePassword(string email, string password, string newPassword, string confirmPassword)
        {
            var response = new ServiceResponse<ApplicationUser>();

            var user = await _userManager.FindByNameAsync(email);

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
                var passwordChangeResponse = AssignPassword(user, newPassword);

                if (!passwordChangeResponse.IsValid)
                {
                    response.ErrorMessage = "Something went wrong.";
                    return response;
                }
            }

            var updateResponse = await _userManager.UpdateAsync(user);

            if (!updateResponse.Succeeded)
            {
                response.ErrorMessage = "Something went wrong.";
            }

            return response;
        }

        public ServiceResponse<ApplicationUser> AssignRandomlyGeneratedPassword(ApplicationUser user, out string password)
        {
            password = _randomStringGeneratorService.Generate();
            return AssignPassword(user, password);
        }

        private ServiceResponse<ApplicationUser> AssignPassword(ApplicationUser user, string newPassword)
        {
            var response = new ServiceResponse<ApplicationUser>();

            if(user == null)
            {
                response.ErrorMessage = "User not found.";
                return response;
            }

            user.Salt = _randomStringGeneratorService.Generate();

            var passwordWithSalt = PasswordSaltService.GetPasswordWithSalt(newPassword, user.Salt);

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordWithSalt);

            response.Result = user;
            return response;
        }
    }
}
