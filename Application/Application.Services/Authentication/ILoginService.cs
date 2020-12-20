using Application.Domain;
using System.Threading.Tasks;
using static Application.Domain.Enums;

namespace Application.Services.Authentication
{
    public interface ILoginService
    {
        Task<ValidationResult<ApplicationUser, LoginStatus>> Login(string username, string password);
    }
}
