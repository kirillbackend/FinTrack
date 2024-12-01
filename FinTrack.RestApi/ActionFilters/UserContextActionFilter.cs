using FinTrack.Services.Context.Contracts;
using FinTrack.Services.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FinTrack.RestApi.ActionFilters
{
    public class UserContextActionFilter : IAuthorizationFilter
    {
        private IContextLocator _contextLocator;

        public UserContextActionFilter(IContextLocator contextLocator)
        {
            _contextLocator = contextLocator;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User == null)
            {
                return;
            }

            var emailClaim = context.HttpContext.User.FindFirst(ClaimTypes.Email);
            var idClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = context.HttpContext.User.FindFirst(ClaimTypes.Role);

            if (emailClaim == null || idClaim == null || roleClaim == null)
            {
                return;
            }

            _contextLocator.AddContext(new UserContext(emailClaim.Value, Guid.Parse(idClaim.Value), roleClaim.Value)
            {
                Id = Guid.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Email = context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value,
                Role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value,
            });
        }
    }
}
