using Application.Domain.ApplicationEntities;
using Application.Services.Files;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Services.UserProfile
{
    public interface IUserProfileService
    {
        ServiceResponse<User> Get(string email);
        ServiceResponse<User> AddUserProfile(string userId, string email, string firstName, string lastName);
        Task<ServiceResponse<FileInfo>> UpdateUserProfile(string email, IFormFile profilePicture = null);
    }
}
