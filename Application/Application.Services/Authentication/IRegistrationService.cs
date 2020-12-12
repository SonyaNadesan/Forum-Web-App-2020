using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface IRegistrationService
    {
        Task<IdentityResult> RegisterAccount(string email);
        Task<ServiceResponse<IdentityResult>> ConfirmRegistration(string userId, string password);
    }
}
