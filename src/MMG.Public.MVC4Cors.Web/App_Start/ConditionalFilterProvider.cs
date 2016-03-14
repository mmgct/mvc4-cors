// *************************************************
// MMG.Public.MVC4Cors.Web.ConditionalFilterProvider.cs
// Last Modified: 03/14/2016 10:47 AM
// Modified By: Green, Brett (greenb1)
// *************************************************

namespace MMG.Public.MVC4Cors.Web.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    // http://haacked.com/archive/2011/04/25/conditional-filters.aspx/
    public class ConditionalFilterProvider : IFilterProvider
    {
        private readonly
            IEnumerable<Func<ControllerContext, ActionDescriptor, object>> _conditions;

        public ConditionalFilterProvider
            (
            IEnumerable<Func<ControllerContext, ActionDescriptor, object>> conditions)
        {
            _conditions = conditions;
        }

        public IEnumerable<Filter> GetFilters
            (
            ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            return from condition in _conditions
                select condition(controllerContext, actionDescriptor)
                into filter
                where filter != null
                select new Filter(filter, FilterScope.Global, null);
        }
    }
}