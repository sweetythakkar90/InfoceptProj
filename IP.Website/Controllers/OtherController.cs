using System.Web.Mvc;


namespace IP.Website.Controllers
{

    [Authorize]
    public class OtherController : Controller
    {
        
        public ActionResult FullWidth()
        {
            return View();
        }
        
        public ActionResult SideBar()
        {
            return View();
        }
        
        public ActionResult Faq()
        {
            return View();
        }
        
        public ActionResult FourOhFour()
        {
            return View();
        }
    }
}
