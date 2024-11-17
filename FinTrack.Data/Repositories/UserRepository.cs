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

        public async Task Delete(int id)
        {
            var user = await Context.Users.FirstOrDefaultAsync(i => i.Id == id);

            if (user != null)
            {
                user.IsDeleted = true;
                user.UpdatedDate = DateTime.UtcNow;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetById(int id)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await Context.Users.Where(u => u.IsDeleted == false).ToListAsync();
            return users;
        }
    }
}
