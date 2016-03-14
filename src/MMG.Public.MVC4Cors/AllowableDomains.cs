// *************************************************
// MMG.Public.MVC4Cors.AllowableDomains.cs
// Last Modified: 03/14/2016 10:08 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors
{
    using System.Collections.Generic;
    using System.Linq;

    public class AllowableDomains : IAllowableDomains
    {
        public bool IsDomainAllowed(IEnumerable<string> pAllowedDomains, string pOrigin)
        {
            var list = pAllowedDomains.ToList();
            return !string.IsNullOrEmpty(pOrigin) && (list.Contains(pOrigin) || list.Contains("*"));
        }
    }
}