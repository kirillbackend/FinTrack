using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface ICurrencyService
    {
        Task<CurrencyDto> GetCurrencyByIdAsync(Guid id);
        Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync();
        Task AddCurrencyAsync(CurrencyDto currencyDto);
        Task DeleteAsync(Guid id);
        Task<CurrencyDto> UpdateAsync(CurrencyDto currencyDto);
        Task<decimal> ConvertCurrencyAsync(string to, string from, string amount);
    }
}
