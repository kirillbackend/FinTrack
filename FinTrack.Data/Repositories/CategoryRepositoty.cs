using FinTrack.Data.Repositories.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Data.Repositories
{
    public class CategoryRepositoty : AbstractRepository<Category>, ICategoryRepositoty
    {
        public CategoryRepositoty(FinTrackDataContext context) : base(context)
        {
        }

        public async Task AddAssync(Category entity)
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            IQueryable<Category> query = Context.Categories;

            var category = await query.FirstOrDefaultAsync(i => i.Id == id);

            if (category != null)
            {
                category.IsDeleted = true;
                category.UpdatedDate = DateTime.UtcNow;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetAsync(Guid id)
        {
            IQueryable<Category> query = Context.Categories;
             
            var category = await query.FirstOrDefaultAsync<Category>(i => i.Id == id);

            return category;
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            IQueryable<Category> query = Context.Categories;

            var categories = await query.Where(i => !i.IsDeleted).ToListAsync();

            return categories;
        }
    }
}
