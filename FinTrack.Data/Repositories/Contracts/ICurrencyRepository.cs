using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface ICurrencyRepository : IRepository
    {
        Task AddAsync(Currency entity);
        Task DeleteAsync(Guid id);
        Task<Currency> GetCurrencyByIdAsync(Guid id);
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
    }
}
