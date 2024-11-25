using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface ICurrencyRepository : IRepository
    {
        Task Add(Currency entity);
        Task Delete(Guid id);
        Task<Currency> GetCurrencyById(Guid id);
        Task<IEnumerable<Currency>> GetCurrencies();
    }
}
