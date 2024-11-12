using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IFinanceRepository : IRepository
    {
        void Add(Finance entity);
        Task Delete(int id);
        Task<Finance> GetFinanceById(int id);
        Task<IEnumerable<Finance>> GetFinances();
    }
}
