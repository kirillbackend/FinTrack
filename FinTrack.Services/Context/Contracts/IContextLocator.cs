
namespace FinTrack.Services.Context.Contracts
{
    public interface IContextLocator
    {
        void AddContext<T>(T context) where T : class, IContext;
        T Get<T>() where T : class, IContext;
    }
}
