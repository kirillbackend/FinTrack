
namespace FinTrack.Data.Repositories
{
    public abstract class AbstractRepository<T>
    {
        protected readonly FinTrackDataContext Context;

        public AbstractRepository(FinTrackDataContext context)
        {
            Context = context;
        }
    }
}
