using Application.Data;
using Application.Services.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;

        public ProfileController(IUnitOfWork unitOfWork, IUserProfileService userProfileService)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
        }

        [Authorize]
        public ActionResult Index()
        {
            var user = _unitOfWork.UserRepository.Get(User.Identity.Name);

            return View(user);
        }

        [Authorize]
        public async Task<ActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var user = _unitOfWork.UserRepository.Get(User.Identity.Name);

            await _userProfileService.UpdateUserProfile(user, profilePicture);

            return RedirectToAction("Index");
        }
    }
}
