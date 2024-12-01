using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IFinanceService
    {
        Task<FinanceDto> GetFinanceById(Guid id);
        Task<IEnumerable<FinanceDto>> GetFinances();
        Task AddFinance(FinanceDto currencyDto);
        Task Delete(Guid id);
        Task<FinanceDto> Update(FinanceDto currencyDto);
    }
}
