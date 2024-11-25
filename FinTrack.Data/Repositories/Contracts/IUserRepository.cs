using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IUserRepository : IRepository
    {
        Task Add(User entity);
        Task Delete(Guid id);
        Task<User> GetById(Guid id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetByEmail(string email);
    }
}
