using Application.Data;
using Application.Domain.ApplicationEntities;
using Application.Services.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

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

        public User Get(string email)
        {
            return _unitOfWork.UserRepository.GetAll().SingleOrDefault(x => x.Email == email);
        }

        public ServiceResponse<User> AddUserProfile(string userId, string email, string firstName, string lastName)
        {
            var newUser = new User()
            {
                Id = userId,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            _unitOfWork.UserRepository.Add(newUser);
            _unitOfWork.Save();

            return new ServiceResponse<User>(newUser);
        }

        public async Task<ServiceResponse<FileInfo>> UpdateUserProfile(User user, IFormFile profilePicture = null)
        {
            ServiceResponse<FileInfo> response = null;

            var userFromDb = _unitOfWork.UserRepository.Get(user.Id);

            if (profilePicture == null)
            {
                user.ProfilePictureImageSrc = userFromDb.ProfilePictureImageSrc;

                response = new ServiceResponse<FileInfo>(null);
            }
            else
            {
                var fileNameUponUpload = "ProfilePicture_" + user.Id;

                response = await _imageUploadService.Upload(profilePicture, Configuration.GetSection("UserImageUploadPath").Value, fileNameUponUpload);

                user.ProfilePictureImageSrc = response.IsValid ? response.Result.FileName : string.Empty;
            }

            _unitOfWork.UserRepository.Edit(user);
            _unitOfWork.Save();

            return response;
        }
    }
}
