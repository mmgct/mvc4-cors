// *************************************************
// MMG.Public.MVC4Cors.CorsEnabledAttribute.cs
// Last Modified: 03/14/2016 10:13 AM
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

        public CorsEnabledAttribute()
        {
            _allowableDomains = new AllowableDomains();
            initialize();
        }

        public CorsEnabledAttribute(string pAllowedDomain)
        {
            _allowableDomains = new AllowableDomains();
            initialize(pAllowedDomain);
        }

        public CorsEnabledAttribute(params string[] pDomains)
        {
            _allowableDomains = new AllowableDomains();
            initialize(pDomains);
        }

        public CorsEnabledAttribute(IAllowableDomains pAllowableDomains, string pAllowedDomain)
        {
            _allowableDomains = pAllowableDomains;
            initialize(pAllowedDomain);
        }


        public CorsEnabledAttribute(IAllowableDomains pAllowableDomains, params string[] pDomains)
        {
            _allowableDomains = pAllowableDomains;
            initialize(pDomains);
        }

        public override void OnActionExecuting(ActionExecutingContext pFilterContext)
        {
            var httpContext = pFilterContext.HttpContext;
            var origin = httpContext.Request.Headers[Headers.Origin] ?? "";

            if (!string.IsNullOrWhiteSpace(origin) && _allowableDomains.IsDomainAllowed(this.AllowedDomains, origin))
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