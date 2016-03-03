// *************************************************
// MMG.Public.MVC4Cors.CorsEnabledAttribute.cs
// Last Modified: 03/03/2016 3:50 PM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class CorsEnabledAttribute : ActionFilterAttribute
    {
        public HashSet<string> AllowedDomains { get; }

        public CorsEnabledAttribute()
        {
            //TODO: Should no domains defined allow all domains?
            //_allowedDomains = new HashSet<string>() { "*" };
            AllowedDomains = new HashSet<string>();
        }

        public CorsEnabledAttribute(string pAllowedDomain)
        {
            AllowedDomains = new HashSet<string>();
            initialize(pAllowedDomain);
        }

        public CorsEnabledAttribute(params string[] pDomains)
        {
            AllowedDomains = new HashSet<string>();
            initialize(pDomains);
        }

        public override void OnActionExecuting(ActionExecutingContext pFilterContext)
        {
            var httpContext = pFilterContext.HttpContext;
            var origin = httpContext.Request.Headers[Headers.Origin] ?? "";
            if (origin.Length > 0)
            {
                if (!string.IsNullOrEmpty(origin) && (AllowedDomains.Contains(origin) || AllowedDomains.Contains("*")))
                {
                    httpContext.Response.Headers.Add
                        (
                            Headers.AccessControlAllowOrigin, origin
                        );
                }
            }
        }

        private void initialize(string pAllowedDomains)
        {
            if (string.IsNullOrEmpty(pAllowedDomains))
                return;

            initialize(pAllowedDomains.Split(',', ';'));
        }

        private void initialize(params string[] pAllowedDomains)
        {
            foreach (var allowedDomain in pAllowedDomains)
                AllowedDomains.Add(allowedDomain);
        }
    }

    public static class Headers
    {
        public static string Origin = "Origin";
        public static string AccessControlRequestMethod = "Access-Control-Request-Method";
        public static string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        public static string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        public static string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
        public static string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
    }
}