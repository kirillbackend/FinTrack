using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IFinanceRepository : IRepository
    {
        Task Add(Finance entity);
        Task Delete(Guid id);
        Task<Finance> GetFinanceById(Guid id);
        Task<IEnumerable<Finance>> GetFinances();
    }
}
