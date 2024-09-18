using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace LegacyTest.Tools
{
    public class ClaimDataFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var claims = user.Claims.ToDictionary(c => c.Type, c => c.Value);
                context.HttpContext.Items["UserClaims"] = claims;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No necesitamos hacer nada después de ejecutar la acción
        }
    }
}
