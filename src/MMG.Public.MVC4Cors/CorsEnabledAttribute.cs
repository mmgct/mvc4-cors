// *************************************************
// MMG.Public.MVC4Cors.CorsEnabledAttribute.cs
// Last Modified: 03/03/2016 2:33 PM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors
{
    using System.Linq;

    public class CorsEnabledAttribute : ActionFilterAttribute
    {
        private readonly string[] allowedDomains;

        public CorsEnabledAttribute()
        {
            allowedDomains = new string[] {"*"};
        }

        public CorsEnabledAttribute(params string[] domains)
        {
            allowedDomains = domains;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.Headers.Contains(Headers.Origin))
            {
                var origin = actionExecutedContext.Request.Headers.GetValues(Headers.Origin).FirstOrDefault();

                // if origin is not empty, and the allowed domains is either * or contains the origin domain
                // then allow the request
                if (!string.IsNullOrEmpty(origin) && (allowedDomains.Contains(origin) || allowedDomains.Contains("*")))
                {
                    actionExecutedContext.Response.Headers.Add
                        (
                            Headers.AccessControlAllowOrigin, origin
                        );
                }
            }
        }
    }
}