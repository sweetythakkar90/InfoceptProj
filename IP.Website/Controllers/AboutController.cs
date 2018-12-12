using System.Web.Mvc;

namespace IP.Website.Controllers
{
    [Authorize]
    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}