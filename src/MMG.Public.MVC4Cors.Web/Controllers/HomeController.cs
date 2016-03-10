// *************************************************
// MMG.Public.MVC4Cors.Web.HomeController.cs
// Last Modified: 03/10/2016 12:46 PM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [CorsEnabled("http://localhost:62628, http://MMG-GREENB3:62628")]
        public JsonResult TestCors()
        {
            return Json("Result: true", JsonRequestBehavior.AllowGet);
        }

        [CorsEnabled("http://MMG-GREENB3:62628")]
        public JsonResult TestCorsBlocked()
        {
            return Json("Result: true", JsonRequestBehavior.AllowGet);
        }
    }
}