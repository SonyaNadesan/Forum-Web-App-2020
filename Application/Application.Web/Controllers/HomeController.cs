using Application.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Register()
        {
            await _registrationService.RegisterAccount("sonya@redford-avenue.co.uk");
            return View();
        }
    }
}
