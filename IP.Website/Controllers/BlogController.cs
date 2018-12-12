using System.Web.Mvc;


namespace IP.Website.Controllers
{

    [Authorize]
    public class BlogController : Controller
    {
        
        public ActionResult Home2()
        {
            return View();
        }
    }
}
