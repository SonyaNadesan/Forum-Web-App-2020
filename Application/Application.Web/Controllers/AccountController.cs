using Application.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Domain;

namespace Application.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILoginService _loginService;
        private readonly IAccountRecoveryService _accountRecoveryService;
        private readonly IPasswordChangeService _passwordChangeService;

        public AccountController(IRegistrationService registrationService, ILoginService loginService, IAccountRecoveryService accountRecoveryService,
                                 IPasswordChangeService passwordChangeService)
        {
            _registrationService = registrationService;
            _loginService = loginService;
            _accountRecoveryService = accountRecoveryService;
            _passwordChangeService = passwordChangeService;
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Profile");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> RegisterAccount(string email, string firstName, string lastName)
        {
            var response = await _registrationService.RegisterAccount(email);

            if (response.IsValid)
            {
                TempData["Registration Status"] = "Registration Successful!";
            }
            else
            {
                TempData["Registration Status"] = "Registration Failed. Please try again later.";
            }

            return RedirectToAction("Index", "Home");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            var response = await _loginService.Login(username, password);
            switch (response.Status)
            {
                case Enums.LoginStatus.ConfirmedButNeedsPasswordChange:
                    return RedirectToAction("ChangePassword", "Account");
                case Enums.LoginStatus.Failed:
                    TempData["Login"] = "Login Failed.";
                    return RedirectToAction("Index");
                case Enums.LoginStatus.Success:
                    return RedirectToAction("Index", "Account");
                case Enums.LoginStatus.LockedOut:
                    TempData["Login"] = "Account has been locked out.";
                    return RedirectToAction("Index");
                default:
                    return RedirectToAction("Index");
            };
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> RecoverAccount(string username)
        {
            var response = await _accountRecoveryService.Recover(username);

            if (response.IsValid)
            {
                TempData["Account Recovery"] = "Please check your email for your new password";
            }
            else
            {
                TempData["Account Recovery"] = "Something went wrong. Please try again later.";
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(string password, string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword)
            {
                var response = await _passwordChangeService.ChangePassword(User.Identity.Name, password, newPassword);

                if (response.IsValid)
                {
                    TempData["Password Change"] = "Your password has been changed successfully.";
                    return RedirectToAction("Index", "Account");
                }

                TempData["Password Change"] = "Sorry, something went wrong.";
            }
            else
            {
                TempData["Password Change"] = "Passwords do not match.";
            }

            return View();
        }
    }
}
