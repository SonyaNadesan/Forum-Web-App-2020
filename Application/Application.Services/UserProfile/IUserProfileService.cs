using Application.Domain.ApplicationEntities;
using Application.Services.Files;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Services.UserProfile
{
    public interface IUserProfileService
    {
        User Get(string userId);
        ServiceResponse<User> AddUserProfile(string userId, string firstName, string lastName);
        Task<ServiceResponse<FileInfo>> UpdateUserProfile(User user, IFormFile profilePicture = null);
    }
}
