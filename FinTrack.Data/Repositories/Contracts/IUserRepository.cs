using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IUserRepository : IRepository
    {
        void Add(User entity);
        Task<User> GetUserByEmail(string email);
    }
}
