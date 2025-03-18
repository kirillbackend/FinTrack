using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IUserRepository : IRepository
    {
        Task AddAsync(User entity);
        Task DeleteAsync(Guid id);
        Task<User> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetByEmailAsync(string email);
    }
}
