using Autofac;
using FinTrack.Localization.Conrtacts;

namespace FinTrack.Localization
{
    public class ContainerConfiguration
    {
        public static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<ResourceProvider>().As<IResourceProvider>();
            builder.RegisterType<ResourceProviderFactory>().As<IResourceProviderFactory>();
            builder.RegisterType<ContextFactory>().As<IContextFactory>();
        }
    }
}
