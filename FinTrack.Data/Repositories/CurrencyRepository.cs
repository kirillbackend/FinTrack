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

        public async Task Add(Currency entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            var currency = await Context.Currencies.FirstOrDefaultAsync(i => i.Id == id);

            if (currency != null)
            {
                currency.IsDeleted = true;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<Currency> GetCurrencyById(Guid id)
        {
            var currency = await Context.Currencies.FirstOrDefaultAsync(i => i.Id == id);
            return currency;
        }

        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
            var currency = await Context.Currencies.Where(i => i.IsDeleted == false).ToListAsync();
            return currency;
        }
    }
}
