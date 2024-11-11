using Autofac;
using FinTrack.Localization.Conrtacts;
using FinTrack.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using FinTrack.RestApi.Auth;

namespace FinTrack.RestApi.ActionFilters
{
    public class LocaleActionFilter : IAsyncResourceFilter
    {
        private readonly ILogger<LocaleActionFilter> _logger;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IContextFactory _contextFactory;

        public LocaleActionFilter(ILogger<LocaleActionFilter> logger,
            ILifetimeScope lifetimeScope,
            IContextFactory contextFactory)
        {
            _logger = logger;
            _lifetimeScope = lifetimeScope;
            _contextFactory = contextFactory;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation($"{context.HttpContext.User}");

            context.HttpContext.Request.Headers.TryGetValue(FinTrackHeaderNames.Locale, out var locale);

            _logger.LogInformation("Setting up scope");
            var contextLocator = _lifetimeScope.Resolve<ContextLocator>();
            var localeContext = _contextFactory.CreateLocaleContext(!string.IsNullOrEmpty(locale.FirstOrDefault()) ? locale.Single() : "en");
            contextLocator.AddContext(localeContext);

            await next();
        }
    }
}
