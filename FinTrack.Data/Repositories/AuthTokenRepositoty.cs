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

        public async Task Add(AuthToken entity)
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<AuthToken> GetByRefreshToken(string refreshToken)
        {
            var token = await Context.AuthTokens.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            return token;
        }
    }
}
