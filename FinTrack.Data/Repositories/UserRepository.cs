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

        public async Task AddAsync(User entity)
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            IQueryable<User> query = Context.Users;

            var user = await query.FirstOrDefaultAsync(i => i.Id == id);

            if (user != null)
            {
                user.IsDeleted = true;
                user.UpdatedDate = DateTime.UtcNow;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            IQueryable<User> query = Context.Users;
            var user = await query.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            IQueryable<User> query = Context.Users;
            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IQueryable<User> query = Context.Users.Where(u => u!.IsDeleted);
            var users = await query.ToListAsync();

            return users;
        }
    }
}
