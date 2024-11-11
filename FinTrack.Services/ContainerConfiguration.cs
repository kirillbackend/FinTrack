using Autofac;
using FinTrack.Services.Contracts;
using FinTrack.Services.Facades.Contracts;
using FinTrack.Services.Facades;
using FinTrack.Localization;
using FinTrack.Services.Mappers;

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

            //register facade
            builder.RegisterType<AuthFacade>().As<IAuthFacade>();

            Localization.ContainerConfiguration.RegisterTypes(builder);
            builder.RegisterType<ContextLocator >().AsSelf().InstancePerLifetimeScope();

            Data.ContainerConfiguration.RegisterTypes(builder, settings.ConnectionStrings);
        }
    }
}
