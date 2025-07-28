using FinTrack.Model;
using FinTrack.Services.Contracts;
using ServiceStack;

namespace FinTrack.Services
{
    public class ReportService : IReportService
    {
        public async Task<Report> CreateReport(IEnumerable<Finance> finances)
        {
            var report = new Report() { Categories = new List<Category>() };

            foreach (var finance in finances.ToList())
            {
                report.Amount += finance.Amount;

                if (!await IsUniqueCategory(report.Categories, finance.Category)) 
                {
                    report.Categories.Add(finance.Category);
                }
            }

            return report;
        }

        #region private region

        private async Task<bool> IsUniqueCategory(IEnumerable<Category> categories, Category category)
        {
            return categories.Any(i => i.Id == category.Id);
        }

        #endregion
    }
}
