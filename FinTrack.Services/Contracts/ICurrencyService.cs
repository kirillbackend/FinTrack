using FinTrack.Model;

namespace FinTrack.Services.Contracts
{
    public interface ICurrencyService
    {
        Task<Currency> GetCurrencyById(int id);
        Task<IEnumerable<Currency>> GetCurrencies();
    }
}
