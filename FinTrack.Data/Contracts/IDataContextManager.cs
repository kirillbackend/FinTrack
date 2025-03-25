
namespace FinTrack.Data.Contracts
{
    public interface IDataContextManager
    {
        TRepository CreateRepository<TRepository>(string id = "default")
           where TRepository : class, IRepository;

        Task SaveAsync(string id = "default");
    }
}
