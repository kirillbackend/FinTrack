using FinTrack.Model;

namespace FinTrack.Services.Contracts
{
    public interface IReportService
    {
        Task<Report> CreateReport(IEnumerable<Finance> finances);
    }
}
