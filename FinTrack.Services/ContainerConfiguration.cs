using Autofac;
using FinTrack.Services.Contracts;
using FinTrack.Services.Facades.Contracts;
using FinTrack.Services.Facades;
using FinTrack.Services.Mappers;
using FinTrack.Services.Context.Contracts;
using FinTrack.Services.Context;
using FinTrack.Services.Wrappers.Contracts;
using FinTrack.Services.Wrappers;
using FinTrack.Services.Kafka;
using FinTrack.Services.Kafka.Contracts;

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
            builder.RegisterType<ContextFactory>().As<IContextFactory>();
            builder.RegisterType<ContextLocator>().As<IContextLocator>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyExchangeKafkaProducer>().As<ICurrencyExchangeKafkaProducer>();
            //builder.RegisterType<CurrencyExchangeKafkaConsumer>().As<ICurrencyExchangeKafkaConsumer>();

            //register wrapper
            builder.RegisterType<FixerAPIWrapper>().As<IFixerAPIWrapper>();

            //register facade
            builder.RegisterType<AuthFacade>().As<IAuthFacade>();
            builder.RegisterType<UserFacade>().As<IUserFacade>();

            Data.ContainerConfiguration.RegisterTypes(builder, settings.ConnectionStrings);
        }
    }
}
