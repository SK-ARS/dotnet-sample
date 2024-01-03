using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace STP.Web.Filters
{
    public class AntiForgeryTokenValidate : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext,
                          ActionDescriptor actionDescriptor)
        {
            List<Filter> filterSet = new List<Filter>();
            // ValidateAntiForgeryToken for POST requests + 
            // Skip Actions with UnValidateAntiForgeryToken
            string verb = controllerContext.HttpContext.Request.HttpMethod;

            if (String.Equals(verb, "POST", StringComparison.OrdinalIgnoreCase)
                && !actionDescriptor.IsDefined(typeof(UnValidateAntiForgeryToken), true))
            {
                filterSet.Add(new Filter(new ValidateAntiForgeryTokenAttribute(),
                              FilterScope.Global, null));
            }

            return filterSet;
        }
    }

    public class UnValidateAntiForgeryToken : ActionFilterAttribute
    {
        // This Filter has no use except skipping AntiForgeryToken Attribute from taking place
    }
}