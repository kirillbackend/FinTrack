using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IFinanceRepository : IRepository
    {
        Task AddAssync(Finance entity);
        Task DeleteAsync(Guid id);
        Task<Finance> GetAsync(Guid id);
        Task<IEnumerable<Finance>> GetAsync();
    }
}
