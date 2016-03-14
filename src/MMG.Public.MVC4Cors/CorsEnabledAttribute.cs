// *************************************************
// MMG.Public.MVC4Cors.CorsEnabledAttribute.cs
// Last Modified: 03/14/2016 8:47 AM
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
            //TODO: Should no domains defined allow all domains?
            //AllowedDomains = new HashSet<string>() { "*" };
            AllowedDomains = new HashSet<string>();
            IsDomainAllowedFunction = isDomainAllowed;
        }

        public CorsEnabledAttribute(string pAllowedDomain)
        {
            AllowedDomains = new HashSet<string>();
            IsDomainAllowedFunction = isDomainAllowed;
            initialize(pAllowedDomain);
        }

        public CorsEnabledAttribute(Func<string, bool> pIsDomainAllowedFunction, string pAllowedDomain)
        {
            AllowedDomains = new HashSet<string>();
            IsDomainAllowedFunction = pIsDomainAllowedFunction;
            initialize(pAllowedDomain);
        }

        public CorsEnabledAttribute(params string[] pDomains)
        {
            AllowedDomains = new HashSet<string>();
            IsDomainAllowedFunction = isDomainAllowed;
            initialize(pDomains);
        }

        public CorsEnabledAttribute(Func<string, bool> pIsDomainAllowedFunction, params string[] pDomains)
        {
            AllowedDomains = new HashSet<string>();
            IsDomainAllowedFunction = pIsDomainAllowedFunction;
            initialize(pDomains);
        }

        public Func<string, bool> IsDomainAllowedFunction;

        private bool isDomainAllowed(string origin)
        {
            return !string.IsNullOrEmpty(origin) && (AllowedDomains.Contains(origin) || AllowedDomains.Contains("*"));
        }

        public override void OnActionExecuting(ActionExecutingContext pFilterContext)
        {
            var httpContext = pFilterContext.HttpContext;
            var origin = httpContext.Request.Headers[Headers.Origin] ?? "";

            if (!string.IsNullOrWhiteSpace(origin))
            {
                if (IsDomainAllowedFunction(origin))
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
}