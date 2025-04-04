using Autofac;
using Microsoft.AspNetCore.Mvc.Filters;
using FinTrack.Services.Context;
using System.Security.Claims;
using FinTrack.Services.Context.Contracts;

namespace FinTrack.RestApi.ActionFilters
{
    public class UserContextActionFilter : IAsyncAuthorizationFilter
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IContextFactory _contextFactory;
        private readonly IContextLocator _iContextLocator;

        public UserContextActionFilter(ILifetimeScope lifetimeScope, IContextFactory contextFactory, IContextLocator iContextLocator)
        {
            _lifetimeScope = lifetimeScope;
            _contextFactory = contextFactory;
            _iContextLocator = iContextLocator;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
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

                var localeContext = _contextFactory.CreateLocaleContext(emailClaim.Value, Guid.Parse(idClaim.Value), roleClaim.Value);
                _iContextLocator.AddContext(localeContext);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
