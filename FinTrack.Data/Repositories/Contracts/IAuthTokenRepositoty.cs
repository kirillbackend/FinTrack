
using FinTrack.Data.Contracts;
using FinTrack.Model;

namespace FinTrack.Data.Repositories.Contracts
{
    public interface IAuthTokenRepositoty : IRepository
    {
        Task<AuthToken> GetByRefreshToken(string refreshToken);
        Task Add(AuthToken entity);
    }
}
