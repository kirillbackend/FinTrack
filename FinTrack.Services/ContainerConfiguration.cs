using Autofac;

namespace FinTrack.Services
{
    public class ContainerConfiguration
    {
        public static void RegisterTypes(ContainerBuilder builder, FinTrackSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            builder.RegisterInstance(settings);
            //Add MapperFactory 
            //MapperFactory.Configure(builder);


            //builder.RegisterType<AuthService>().As<IAuthService>();
        }
    }
}
