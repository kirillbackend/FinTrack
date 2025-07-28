using FinTrack.Enums;
using FinTrack.Model;
using FinTrack.Services.Contracts;
using System.Linq.Expressions;

namespace FinTrack.Services
{
    public class FilterService : IFilterService
    {
        public async Task<Expression<Func<Finance, bool>>> CreateReportFilter(Guid userId, ReportType reportType)
        {
            var predicate = PredicateBuilder.True<Finance>();

            var dateFilters = new Dictionary<ReportType, Action>
            {
                [ReportType.Day] = () => predicate = predicate.And(i => i.CreatedDate >= DateTime.Today && i.CreatedDate < DateTime.Today.AddDays(1)),
                [ReportType.Week] = () => predicate = predicate.And(i => i.CreatedDate >= DateTime.Today && i.CreatedDate < DateTime.Today.AddDays(7)),
                [ReportType.Month] = () => predicate = predicate.And(i => i.CreatedDate >= DateTime.Today && i.CreatedDate < DateTime.Today.AddMonths(1)),
                [ReportType.Year] = () => predicate = predicate.And(i => i.CreatedDate >= DateTime.Today && i.CreatedDate < DateTime.Today.AddYears(1)),
                [ReportType.All] = () => { }
            };

            predicate = predicate.And(i => i.UserId == userId);
            predicate = predicate.And(i => !i.IsDeleted);

            if (dateFilters.TryGetValue(reportType, out var filterAction))
            {
                filterAction();
            }

            return predicate;
        }
    }
}
