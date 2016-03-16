// *************************************************
// MMG.Public.MVC4Cors.Web.BogusAllowableDomainsImplementation.cs
// Last Modified: 03/14/2016 10:43 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors.Web.App_Start
{
    using System.Collections.Generic;

    public class BogusAllowableDomainsImplementation : IAllowableDomains
    {
        public bool IsDomainAllowed(IEnumerable<string> pDomains, string pOrigin)
        {
            return pOrigin == "BOGUS";
        }
    }
}