using FinTrack.Data.Repositories.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinTrack.Data.Repositories
{
    public class FinanceRepository : AbstractRepository<Finance>, IFinanceRepository
    {
        public FinanceRepository(FinTrackDataContext context)
            : base(context)
        {
        }

        public async Task AddAssync(Finance entity)
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            IQueryable<Finance> query = Context.Finances;

            var finance = await query.FirstOrDefaultAsync(i => i.Id == id);

            if (finance != null)
            {
                finance.IsDeleted = true;
                finance.UpdatedDate = DateTime.UtcNow;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<Finance> GetAsync(Guid id)
        {
            IQueryable<Finance> query = Context.Finances;
            var finance = await query.FirstOrDefaultAsync(i => i.Id == id);

            return finance;
        }

        public async Task<IEnumerable<Finance>> GetAsync(Expression<Func<Finance, bool>> expression = null)
        {
            IQueryable<Finance> query = Context.Finances;
            var finances = await query.Where(expression).ToListAsync();

            return finances;
        }
    }
}
