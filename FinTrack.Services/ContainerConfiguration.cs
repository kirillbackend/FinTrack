using Autofac;
using FinTrack.Services.Contracts;
using FinTrack.Services.Facades.Contracts;
using FinTrack.Services.Facades;
using FinTrack.Localization;
using FinTrack.Services.Mappers;
using FinTrack.Services.Context.Contracts;
using FinTrack.Services.Context;
using FinTrack.Services.Wrappers.Contracts;
using FinTrack.Services.Wrappers;

namespace FinTrack.Services
{
    public class ContainerConfiguration
    {
        public static void RegisterTypes(ContainerBuilder builder, FinTrackServiceSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            builder.RegisterInstance(settings);
            MapperFactory.Configure(builder);

            //register service
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<HashService>().As<IHashService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>();
            builder.RegisterType<FinanceService>().As<IFinanceService>();
            builder.RegisterType<Context.ContextFactory>().As<IContextFactory>();
            builder.RegisterType<ContextLocator>().AsSelf().InstancePerLifetimeScope();

            //register wrapper
            builder.RegisterType<FixerAPIWrapper>().As<IFixerAPIWrapper>();

            //register facade
            builder.RegisterType<AuthFacade>().As<IAuthFacade>();
            builder.RegisterType<UserFacade>().As<IUserFacade>();

            Localization.ContainerConfiguration.RegisterTypes(builder);
            builder.RegisterType<LocalizationContextLocator >().AsSelf().InstancePerLifetimeScope();

            Data.ContainerConfiguration.RegisterTypes(builder, settings.ConnectionStrings);
        }
    }
}
