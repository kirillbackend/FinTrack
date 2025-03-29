using FinTrack.Data.Repositories.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Finance> GetFinanceByIdAsync(Guid id)
        {
            IQueryable<Finance> query = Context.Finances;
            var finance = await query.FirstOrDefaultAsync(i => i.Id == id);

            return finance;
        }

        public async Task<IEnumerable<Finance>> GetFinancesAsync()
        {
            IQueryable<Finance> query = Context.Finances;
            var finances = await query.Where(i => !i.IsDeleted).ToListAsync();

            return finances;
        }
    }
}
