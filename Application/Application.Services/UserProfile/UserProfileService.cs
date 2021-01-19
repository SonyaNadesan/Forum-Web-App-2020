using Application.Data;
using Application.Domain.ApplicationEntities;
using Application.Services.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
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

        public ServiceResponse<User> Get(string email)
        {
            var response = new ServiceResponse<User>();

            var user = _unitOfWork.UserRepository.Get(email);

            response.Result = user;

            if (user == null)
            {
                response.ErrorMessage = "User not found";
                return response;
            }

            return response;
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

            try
            {
                _unitOfWork.UserRepository.Add(newUser);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return new ServiceResponse<User>(newUser)
                {
                    ErrorMessage = "Sorry, something went wrong."
                };
            }

            return new ServiceResponse<User>(newUser);
        }

        public async Task<ServiceResponse<FileInfo>> UpdateUserProfile(string email, IFormFile profilePicture = null)
        {
            ServiceResponse<FileInfo> response;

            var user = _unitOfWork.UserRepository.Get(email);

            if (profilePicture == null)
            {
                response = new ServiceResponse<FileInfo>(null);
            }
            else
            {
                var fileNameUponUpload = "ProfilePicture_" + user.Id;

                response = await _imageUploadService.Upload(profilePicture, Configuration.GetSection("UserImageUploadPath").Value, fileNameUponUpload);

                user.ProfilePictureImageSrc = response.IsValid ? response.Result.FileName : string.Empty;
            }

            if (!response.IsValid)
            {
                return response;
            }

            try
            {
                _unitOfWork.UserRepository.Edit(user);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }
    }
}
