using FinTrack.Data.Repositories.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Data.Repositories
{
    public class CurrencyRepository : AbstractRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(FinTrackDataContext context)
            : base(context)
        {

        }

        public async Task AddAsync(Currency entity)
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            IQueryable<Currency> query = Context.Currencies;
            var currency = await query.FirstOrDefaultAsync(i => i.Id == id);

            if (currency != null)
            {
                currency.IsDeleted = true;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<Currency> GetCurrencyByIdAsync(Guid id)
        {
            IQueryable<Currency> query = Context.Currencies;
            var currency = await query.FirstOrDefaultAsync(i => i.Id == id);

            return currency;
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            IQueryable<Currency> query = Context.Currencies.Where(i => !i.IsDeleted);
            var currency = await query.ToListAsync();

            return currency;
        }
    }
}
