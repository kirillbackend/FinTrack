using FinTrack.Model;
using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IFinanceService
    {
        Task<FinanceDto> GetFinanceById(int id);
        Task<IEnumerable<FinanceDto>> GetFinances();
        Task AddFinance(FinanceDto currencyDto);
        Task Delete(int id);
        Task<FinanceDto> Update(FinanceDto currencyDto);
    }
}
