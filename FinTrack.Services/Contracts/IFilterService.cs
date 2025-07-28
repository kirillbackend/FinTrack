using FinTrack.Enums;
using FinTrack.Model;
using System.Linq.Expressions;

namespace FinTrack.Services.Contracts
{
    public interface IFilterService
    {
        Task<Expression<Func<Finance, bool>>> CreateReportFilter(Guid userId, ReportType reportType);
    }
}
