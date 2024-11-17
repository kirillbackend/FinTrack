using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IUserRepository : IRepository
    {
        void Add(User entity);
        Task Delete(int id);
        Task<User> GetById(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetByEmail(string email);
    }
}
