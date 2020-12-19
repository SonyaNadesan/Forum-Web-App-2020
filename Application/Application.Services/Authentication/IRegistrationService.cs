using Application.Domain;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface IRegistrationService
    {
        Task<ServiceResponse<ApplicationUser>> RegisterAccount(string email);
    }
}
