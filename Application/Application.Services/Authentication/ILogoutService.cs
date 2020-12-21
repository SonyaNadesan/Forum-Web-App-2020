using Application.Domain;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface ILogoutService
    {
        Task<ServiceResponse<ApplicationUser>> Logout(string email);
    }
}