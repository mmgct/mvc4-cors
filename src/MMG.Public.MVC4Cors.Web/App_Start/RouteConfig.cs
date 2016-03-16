// *************************************************
// MMG.Public.MVC4Cors.Web.RouteConfig.cs
// Last Modified: 03/10/2016 11:23 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute
                (
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}