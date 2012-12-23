using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using MvcApplication.CustomAttributes;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [RequireTokenAuthenticationAttribute]
        [RequireClaims(ClaimTypes.NameIdentifier)]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Error(string message)
        {
            return View("Error", message);
        }
    }
}