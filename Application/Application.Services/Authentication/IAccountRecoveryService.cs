using Application.Domain;
using Sonya.AspNetCore.Common;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface IAccountRecoveryService
    {
        Task<ServiceResponse<ApplicationUser>> Recover(string email);
    }
}
