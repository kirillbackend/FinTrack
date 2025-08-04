using FinTrack.Enums;
using FinTrack.Model;
using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IFinanceService
    {
        Task<FinanceDto> GetAsync(Guid id);
        Task<IEnumerable<FinanceDto>> GetAsync();
        Task AddAsync(FinanceDto currencyDto);
        Task DeleteAsync(Guid id);
        Task<FinanceDto> UpdateAsync(FinanceDto currencyDto);
        Task AddCategoryAsync(Guid financeId, Guid categoryId);
        Task<Report> GetReportAsync(Guid userId, ReportType reportType);
    }
}
