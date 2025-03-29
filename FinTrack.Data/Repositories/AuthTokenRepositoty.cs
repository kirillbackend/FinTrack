using FinTrack.Data.Repositories.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Data.Repositories
{
    public class AuthTokenRepositoty : AbstractRepository<AuthToken>, IAuthTokenRepositoty
    {
        public AuthTokenRepositoty(FinTrackDataContext context)
            : base(context)
        {
        }

        public async Task AddAsync(AuthToken entity)
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<AuthToken> GetByRefreshTokenAsync(string refreshToken)
        {
            IQueryable<AuthToken> query = Context.AuthTokens;
            var token = await query.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            return token;
        }
    }
}
