using Application.Services.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
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
        public JsonResult GetUser()
        {
            var response = _userProfileService.Get(User.Identity.Name);

            if (!response.IsValid)
            {
                throw new Exception();
            }

            var json = JsonConvert.SerializeObject(response.Result, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return new JsonResult(json);
        }

        [Authorize]
        public async Task<ActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            await _userProfileService.UpdateUserProfile(User.Identity.Name, profilePicture);

            return RedirectToAction("Index");
        }
    }
}
