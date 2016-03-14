// *************************************************
// MMG.Public.MVC4Cors.IAllowableDomains.cs
// Last Modified: 03/14/2016 10:05 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors
{
    using System.Collections.Generic;

    public interface IAllowableDomains
    {
        bool IsDomainAllowed(IEnumerable<string> pAllowedDomains, string pOrigin);
    }
}