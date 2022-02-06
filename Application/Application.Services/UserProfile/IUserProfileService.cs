using Application.Domain.ApplicationEntities;
using Microsoft.AspNetCore.Http;
using Sonya.AspNetCore.Common;
using Sonya.AspNetCore.Common.Files;
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
