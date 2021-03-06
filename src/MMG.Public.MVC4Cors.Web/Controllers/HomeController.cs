﻿// *************************************************
// MMG.Public.MVC4Cors.Web.HomeController.cs
// Last Modified: 03/16/2016 11:04 AM
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

        [CorsEnabled("http://localhost:62628, http://MMG-GREENB3:62628", "GET, POST")]
        public JsonResult TestCors()
        {
            return Json("Result: true", JsonRequestBehavior.AllowGet);
        }

        [CorsEnabled("http://localhost:62628, http://MMG-GREENB3:62628", "PUT")]
        public JsonResult TestCorsPut()
        {
            return Json("Created: true");
        }

        [CorsEnabled("http://MMG-GREENB3:62628")]
        public JsonResult TestCorsBlocked()
        {
            return Json("Result: true", JsonRequestBehavior.AllowGet);
        }

        // Look at FilterConfig... this is wired in conditionally
        public JsonResult TestCorsInjection()
        {
            return Json("Result: true", JsonRequestBehavior.AllowGet);
        }
    }
}