using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace TicketHandler.Infrastructure.Filters
{
    public class CustomAuthorisationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!IsAuthorised(context.HttpContext.User))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary 
                {
                    { "controller", "Userdatas" },
                    { "action", "login" } 
                });
            }
        }
        private bool IsAuthorised(ClaimsPrincipal user)
        {
            if(user.Identity.Name==null)
               return false;
            else
                return true;
        }
    }
}
