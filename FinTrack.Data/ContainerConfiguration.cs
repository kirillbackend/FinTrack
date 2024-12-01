using Autofac;
using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories;
using FinTrack.Data.Repositories.Contracts;

namespace FinTrack.Data
{
    public class ContainerConfiguration
    {
        public static void RegisterTypes(ContainerBuilder builder, DbConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            builder.RegisterInstance(settings);

            builder.RegisterType<DataContextManager>().As<IDataContextManager>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<CurrencyRepository>().As<ICurrencyRepository>();
            builder.RegisterType<FinanceRepository>().As<IFinanceRepository>();
        }
    }
}
