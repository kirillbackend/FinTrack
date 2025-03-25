
namespace FinTrack.Localization.Conrtacts
{
    public interface IResourceProviderFactory
    {
        IResourceProvider CreateResourceProvider(string locale);
    }
}
