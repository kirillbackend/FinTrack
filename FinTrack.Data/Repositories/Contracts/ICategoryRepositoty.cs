using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface ICategoryRepositoty : IRepository
    {
        Task AddAssync(Category entity);
        Task DeleteAsync(Guid id);
        Task<Category> GetAsync(Guid id);
        Task<IEnumerable<Category>> GetAsync();
    }
}
