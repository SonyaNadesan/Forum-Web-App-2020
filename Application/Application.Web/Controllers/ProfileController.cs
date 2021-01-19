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
        private readonly IUserProfileService _userProfileService;

        public ProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [Authorize]
        public ActionResult Index()
        {
            var response = _userProfileService.Get(User.Identity.Name);

            if (!response.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [Authorize]
        public async Task<ActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            await _userProfileService.UpdateUserProfile(User.Identity.Name, profilePicture);

            return RedirectToAction("Index");
        }
    }
}
