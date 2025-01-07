using FinTrack.Model;

namespace FinTrack.Services.Wrappers.Contracts
{
    public interface IFixerAPIWrapper
    {
        Task<Symbol> GetSymbols();
        Task<decimal> ConvertCurrency(string to, string from, string amount);
        Task LatestCurrency(string baseCurrency, List<string> symbols);
    }
}
