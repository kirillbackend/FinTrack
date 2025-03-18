
using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IAuthTokenRepositoty : IRepository
    {
        Task<AuthToken> GetByRefreshTokenAsync(string refreshToken);
        Task AddAsync(AuthToken entity);
    }
}
