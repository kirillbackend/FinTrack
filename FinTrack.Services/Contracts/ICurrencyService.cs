using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface ICurrencyService
    {
        Task<CurrencyDto> GetCurrencyById(int id);
        Task<IEnumerable<CurrencyDto>> GetCurrencies();
        Task AddCurrency(CurrencyDto currencyDto);
        Task Delete(int id);
        Task<CurrencyDto> Update(CurrencyDto currencyDto);
    }
}
