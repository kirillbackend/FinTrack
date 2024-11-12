using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface ICurrencyRepository : IRepository
    {
        void Add(Currency entity);
        Task Delete(int id);
        Task<Currency> GetCurrencyById(int id);
        Task<IEnumerable<Currency>> GetCurrencies();
    }
}
