using System.Web.Mvc;

namespace IP.Website.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}