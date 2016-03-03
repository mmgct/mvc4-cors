﻿// *************************************************
// MMG.Public.MVC4Cors.CorsEnabledAttribute.cs
// Last Modified: 03/03/2016 3:12 PM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors
{
    using System.Linq;
    using System.Web.Mvc;

    public class CorsEnabledAttribute : ActionFilterAttribute
    {
        public string[] AllowedDomains { get; set; }

        public CorsEnabledAttribute()
        {
            AllowedDomains = new string[] {"*"};
        }

        public CorsEnabledAttribute(params string[] domains)
        {
            AllowedDomains = domains;
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