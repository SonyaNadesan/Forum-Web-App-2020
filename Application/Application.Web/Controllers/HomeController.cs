using Application.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRegistrationService _registrationService;

        public HomeController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public IActionResult Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            return View();
        }

        public IActionResult Register()
        {
            _registrationService.RegisterAccount("sonya@redford-avenue.co.uk");
            return View();
        }

        public IActionResult Confirmn(string userId, string password)
        {
            _registrationService.ConfirmRegistration(userId, password);
            return View();
        }
    }
}
