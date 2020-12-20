using Microsoft.AspNetCore.Mvc;

namespace Application.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
