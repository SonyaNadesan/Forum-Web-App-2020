using Application.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using static Application.Domain.Enums;

namespace Application.Services.Authentication
{
    public interface ILoginService
    {
        Task<ValidationResult<ApplicationUser, LoginStatus>> IsLoginCredentialsValid(string username, string password);
    }
}
