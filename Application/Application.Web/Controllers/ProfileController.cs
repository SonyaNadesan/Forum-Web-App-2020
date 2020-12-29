using Application.Services.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var user = _userProfileService.Get(User.Identity.Name);

            return View(user);
        }

        [Authorize]
        public async Task<ActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var user = _userProfileService.Get(User.Identity.Name);

            await _userProfileService.UpdateUserProfile(user, profilePicture);

            return RedirectToAction("Index");
        }
    }
}
