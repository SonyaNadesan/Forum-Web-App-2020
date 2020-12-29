using Application.Data;
using Application.Domain.ApplicationEntities;
using Application.Services.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services.UserProfile
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IConfiguration Configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploadService _imageUploadService;

        public UserProfileService(IConfiguration configuration, IUnitOfWork unitOfWork, IImageUploadService imageUploadService)
        {
            Configuration = configuration;
            _unitOfWork = unitOfWork;
            _imageUploadService = imageUploadService;
        }

        public User Get(string userId)
        {
            return _unitOfWork.UserRepository.Get(userId);
        }

        public void AddUserProfile(string userId, string firstName, string lastName)
        {
            var newUser = new User()
            {
                Id = userId,
                FirstName = firstName,
                LastName = lastName
            };

            _unitOfWork.UserRepository.Add(newUser);
            _unitOfWork.Save();
        }

        public async void UpdateUserProfile(User user, IFormFile profilePicture = null)
        {
            var userFromDb = _unitOfWork.UserRepository.Get(user.Id);

            if (profilePicture == null)
            {
                user.ProfilePictureImageSrc = userFromDb.ProfilePictureImageSrc;
            }
            else
            {
                var fileNameUponDownload = "ProfilePicture_" + user.FirstName + user.LastName;
                var response = await _imageUploadService.Upload(profilePicture, Configuration.GetSection("UserImageUploadPath").Value, fileNameUponDownload);
                user.ProfilePictureImageSrc = response.IsValid ? response.Result.FileName : string.Empty;
            }

            _unitOfWork.UserRepository.Edit(user);
            _unitOfWork.Save();
        }
    }
}
