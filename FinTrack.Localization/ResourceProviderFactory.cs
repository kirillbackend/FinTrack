using FinTrack.Localization.Conrtacts;

namespace FinTrack.Localization
{
    internal class ResourceProviderFactory : IResourceProviderFactory
    {
        public IResourceProvider CreateResourceProvider(string locale)
        {
            return new ResourceProvider(locale);
        }
    }
}
