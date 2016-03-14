// *************************************************
// MMG.Public.MVC4Cors.CorsEnabledAttribute.cs
// Last Modified: 03/14/2016 9:16 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;

    public class CorsEnabledAttribute : ActionFilterAttribute
    {
        public HashSet<string> AllowedDomains { get; private set; }

        public CorsEnabledAttribute()
        {
            _isDomainAllowedFunction = isDomainAllowed;
            initialize();
        }

        public CorsEnabledAttribute(string pAllowedDomain)
        {
            _isDomainAllowedFunction = isDomainAllowed;
            initialize(pAllowedDomain);
        }

        public CorsEnabledAttribute(params string[] pDomains)
        {
            _isDomainAllowedFunction = isDomainAllowed;
            initialize(pDomains);
        }

        public CorsEnabledAttribute(Func<string, bool> pIsDomainAllowedFunction, string pAllowedDomain)
        {
            _isDomainAllowedFunction = pIsDomainAllowedFunction;
            initialize(pAllowedDomain);
        }


        public CorsEnabledAttribute(Func<string, bool> pIsDomainAllowedFunction, params string[] pDomains)
        {
            _isDomainAllowedFunction = pIsDomainAllowedFunction;
            initialize(pDomains);
        }

        private readonly Func<string, bool> _isDomainAllowedFunction;

        private bool isDomainAllowed(string pOrigin)
        {
            return !string.IsNullOrEmpty(pOrigin) && (AllowedDomains.Contains(pOrigin) || AllowedDomains.Contains("*"));
        }

        public override void OnActionExecuting(ActionExecutingContext pFilterContext)
        {
            var httpContext = pFilterContext.HttpContext;
            var origin = httpContext.Request.Headers[Headers.Origin] ?? "";

            if (!string.IsNullOrWhiteSpace(origin) && _isDomainAllowedFunction(origin))
            {
                httpContext.Response.Headers.Add
                    (
                        Headers.AccessControlAllowOrigin, origin
                    );
            }
            else
            {
                pFilterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Failed Cross-Origin Request");
            }
        }

        private void initialize(string pAllowedDomains)
        {
            if (string.IsNullOrEmpty(pAllowedDomains))
            {
                initialize();
            }
            else
            {
                initialize(pAllowedDomains.Split(',', ';'));
            }
        }

        private void initialize(params string[] pAllowedDomains)
        {
            //AllowedDomains = new HashSet<string>() { "*" };
            AllowedDomains = new HashSet<string>();

            foreach (var allowedDomain in pAllowedDomains)
                if (!string.IsNullOrWhiteSpace(allowedDomain))
                {
                    AllowedDomains.Add(allowedDomain);
                }
        }
    }
}