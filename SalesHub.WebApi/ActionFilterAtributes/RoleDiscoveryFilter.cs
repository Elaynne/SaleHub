using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SalesHub.WebApi.ActionFilterAtributes
{
    public class RoleDiscoveryFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (roleClaim != null)
                {
                    // Adicionar a role como argumento de ação
                    context.ActionArguments["userRole"] = roleClaim.Value;
                }
                else
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
