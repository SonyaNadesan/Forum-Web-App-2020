using Application.Domain.ApplicationEntities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.UserProfile
{
    public interface IUserProfileService
    {
        User Get(string userId);
        void AddUserProfile(string userId, string firstName, string lastName);
        void UpdateUserProfile(User user, IFormFile profilePicture = null);
    }
}
