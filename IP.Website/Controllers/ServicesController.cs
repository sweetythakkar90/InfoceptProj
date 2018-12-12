using System.Web.Mvc;

namespace IP.Website.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}