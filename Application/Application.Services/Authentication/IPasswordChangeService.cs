using Application.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface IPasswordChangeService
    {
        Task<ServiceResponse<ApplicationUser>> ChangePassword(string username, string password, string newPassword);
    }
}
