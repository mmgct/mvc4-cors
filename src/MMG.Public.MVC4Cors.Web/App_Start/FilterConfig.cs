using System.Web;
using System.Web.Mvc;

namespace MMG.Public.MVC4Cors.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
