using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using Domain.Extensions;

namespace SalesHub.WebApi.ActionFilterAtributes
{
    public class RoleDiscoveryFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var data = GetUserFromToken(context);
                if (data != null)
                {
                    context.ActionArguments["userRole"] = data.Value.Role;
                    context.ActionArguments["userId"] = data.Value.Id;
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

        private (UserRole Role, Guid? Id)? GetUserFromToken(ActionExecutingContext context)
        {
            var bearerToken = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(bearerToken) as JwtSecurityToken;

            var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value;
            var userRole = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
            var role = userRole.ToEnum<UserRole>();

            return !string.IsNullOrEmpty(userId) ? (role, Guid.Parse(userId)) : null;
        }

    }
}
