
namespace FinTrack.Services.Context.Contracts
{
    public interface IContextFactory
    {
        UserContext CreateLocaleContext(string email, Guid id, string role);
    }
}
