using System.Web.Mvc;


namespace IP.Website.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        
        public ActionResult ThreeColumn()
        {
            return View();
        }
    }
}
