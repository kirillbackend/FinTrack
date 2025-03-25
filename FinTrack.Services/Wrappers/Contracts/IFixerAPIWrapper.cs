using FinTrack.Model;

namespace FinTrack.Services.Wrappers.Contracts
{
    public interface IFixerAPIWrapper
    {
        Task<Symbol> GetSymbolsAsync();
        Task<decimal> ConvertCurrencyAsync(string to, string from, string amount);
        Task LatestCurrencyAsync(string baseCurrency, List<string> symbols);
    }
}
