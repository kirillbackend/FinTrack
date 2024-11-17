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

        public void Add(Finance entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var finance = await Context.Finances.FirstOrDefaultAsync(i => i.Id == id);

            if (finance != null)
            {
                finance.IsDeleted = true;
                finance.UpdatedDate = DateTime.UtcNow;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<Finance> GetFinanceById(int id)
        {
            var finance = await Context.Finances.FirstOrDefaultAsync(i => i.Id == id);

            return finance;
        }

        public async Task<IEnumerable<Finance>> GetFinances()
        {
            var finances = await Context.Finances.Where(i => i.IsDeleted == false).ToListAsync();

            return finances;
        }
    }
}
