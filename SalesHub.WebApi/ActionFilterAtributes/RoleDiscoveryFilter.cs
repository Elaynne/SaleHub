using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace SalesHub.WebApi.ActionFilterAtributes
{
    [ExcludeFromCodeCoverage]
    public class RoleDiscoveryFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userData = GetRoleFromToken(context);
                if (userData != null)
                {
                    context.HttpContext.Items["userRole"] = userData.Value.role;
                    context.HttpContext.Items["userId"] = userData.Value.userId;
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

        private (UserRole? role, string userId)? GetRoleFromToken(ActionExecutingContext context)
        {
            var bearerToken = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(bearerToken) as JwtSecurityToken;
            var userRole = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
            var role = userRole.ToEnum<UserRole>();
            var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value;

            return !string.IsNullOrEmpty(userRole) ? (role, userId) : null;
        }

    }
}
