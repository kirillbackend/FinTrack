using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IFinanceService
    {
        Task<FinanceDto> GetFinanceByIdAsync(Guid id);
        Task<IEnumerable<FinanceDto>> GetFinancesAsync();
        Task AddFinanceAsync(FinanceDto currencyDto);
        Task DeleteAsync(Guid id);
        Task<FinanceDto> UpdateAsync(FinanceDto currencyDto);
    }
}
