// *************************************************
// MMG.Public.MVC4Cors.CorsEnabledAttribute.cs
// Last Modified: 03/16/2016 10:12 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;

    public class CorsEnabledAttribute : ActionFilterAttribute
    {
        private readonly IAllowableDomains _allowableDomains;
        public HashSet<string> AllowedDomains { get; private set; }
        public string AllowedMethods { get; private set; }

        public CorsEnabledAttribute() : this(null, "", "") {}

        public CorsEnabledAttribute(string pAllowedDomains) : this(null, pAllowedDomains, "") {}

        public CorsEnabledAttribute(string pAllowedDomains, string pAllowedMethods) : this(null, pAllowedDomains, pAllowedMethods) {}

        public CorsEnabledAttribute(IAllowableDomains pAllowableDomains, string pAllowedDomains) : this(pAllowableDomains, pAllowedDomains, "") {}

        public CorsEnabledAttribute(IAllowableDomains pAllowableDomains, string pAllowedDomains, string pAllowedMethods)
        {
            _allowableDomains = pAllowableDomains ?? new AllowableDomains();
            initializeDomains(pAllowedDomains);
            initializeMethods(pAllowedMethods);
        }

        public override void OnActionExecuting(ActionExecutingContext pFilterContext)
        {
            var httpContext = pFilterContext.HttpContext;
            var origin = httpContext.Request.Headers[Headers.Origin] ?? "";
            var method = httpContext.Request.HttpMethod.ToLower();
            if (!string.IsNullOrWhiteSpace(origin) && _allowableDomains.IsDomainAllowed(this.AllowedDomains, origin))
            {
                httpContext.Response.Headers.Add
                    (
                        Headers.AccessControlAllowOrigin, origin
                    );
                // Pre-Flight... if being asked for OPTIONS, simply send back the allowed methods
                // Browser will handle the rest
                if (method == "options")
                {
                    httpContext.Response.Headers.Add
                        (
                            Headers.AccessControlAllowMethods, AllowedMethods
                        );
                }
            }
            // Browser may not send origin if it's a same-origin request
            else if (!string.IsNullOrWhiteSpace(origin))
            {
                pFilterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Failed Cross-Origin Request");
            }
        }

        private void initializeDomains(string pAllowedDomains)
        {
            if (string.IsNullOrEmpty(pAllowedDomains))
            {
                initializeDomains();
            }
            else
            {
                initializeDomains(pAllowedDomains.Split(',', ';'));
            }
        }

        private void initializeDomains(params string[] pAllowedDomains)
        {
            //AllowedDomains = new HashSet<string>() { "*" };
            AllowedDomains = new HashSet<string>();

            foreach (var allowedDomain in pAllowedDomains)
                if (!string.IsNullOrWhiteSpace(allowedDomain))
                {
                    AllowedDomains.Add(allowedDomain);
                }
        }

        private void initializeMethods(string pAllowedMethods)
        {
            AllowedMethods = !string.IsNullOrWhiteSpace(pAllowedMethods) ? pAllowedMethods : "GET,POST";
        }
    }
}