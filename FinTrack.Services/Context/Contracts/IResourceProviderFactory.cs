
namespace FinTrack.Services.Context.Contracts
{
    public interface IResourceProviderFactory
    {
        IResourceProvider CreateResourceProvider(string email, Guid id, string role);
    }
}
