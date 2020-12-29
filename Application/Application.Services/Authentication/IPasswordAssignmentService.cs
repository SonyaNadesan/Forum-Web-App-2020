using Application.Domain;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface IPasswordAssignmentService
    {
        Task<ServiceResponse<ApplicationUser>> ChangePassword(string email, string password, string newPassword, string confirmPassword);

        ServiceResponse<ApplicationUser> AssignRandomlyGeneratedPassword(ApplicationUser user, out string password);
    }
}
