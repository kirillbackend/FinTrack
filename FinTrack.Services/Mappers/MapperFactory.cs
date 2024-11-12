using Autofac;
using FinTrack.Services.Mappers.Contracts;

namespace FinTrack.Services.Mappers
{
    public class MapperFactory : IMapperFactory
    {
        private ILifetimeScope _container;

        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<UserMapper>().As<IUserMapper>().SingleInstance();
            builder.RegisterType<CurrencyMapper>().As<ICurrencyMapper>().SingleInstance();
            builder.RegisterType<IFinanceMapper>().As<IFinanceMapper>().SingleInstance();

            // self-register
            builder.RegisterType<MapperFactory>().As<IMapperFactory>().SingleInstance();
        }

        public T GetMapper<T>() where T : IMapper
        {
            return _container.Resolve<T>();
        }

        public MapperFactory(ILifetimeScope container)
        {
            _container = container;
        }
    }
}
