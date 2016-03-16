// *************************************************
// MMG.Public.MVC4Cors.Web.FilterConfig.cs
// Last Modified: 03/14/2016 10:25 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using App_Start;
    using Controllers;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            var bogusCorsFilter = new CorsEnabledAttribute(new BogusAllowableDomainsImplementation(), "http://ignored.com");

            // http://haacked.com/archive/2011/04/25/conditional-filters.aspx/
            IEnumerable<Func<ControllerContext, ActionDescriptor, object>> conditions =
                new Func<ControllerContext, ActionDescriptor, object>[]
                {

                    (c, a) => c.Controller.GetType() == typeof (HomeController) && a.ActionName == "TestCorsInjection"
                        ? bogusCorsFilter
                        : null,
                };

            var provider = new ConditionalFilterProvider(conditions);
            FilterProviders.Providers.Add(provider);
        }
    }
}