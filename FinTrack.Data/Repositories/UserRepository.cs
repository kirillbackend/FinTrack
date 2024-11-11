using FinTrack.Data.Repositories.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Data.Repositories
{
    public class UserRepository : AbstractRepository<User>, IUserRepository
    {
        public UserRepository(FinTrackDataContext context)
            : base(context)
        {

        }

        public void Add(User entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var author = await Context.Users.FirstOrDefaultAsync(a => a.Email == email);
            return author;
        }
    }
}
