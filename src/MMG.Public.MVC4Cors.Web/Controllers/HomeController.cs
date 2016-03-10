// *************************************************
// MMG.Public.MVC4Cors.Web.HomeController.cs
// Last Modified: 03/10/2016 11:26 AM
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

        public JsonResult TestCors()
        {
            return Json("Result: true", JsonRequestBehavior.AllowGet);
        }
    }
}