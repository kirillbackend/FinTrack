using Autofac;
using FinTrack.Data.Contracts;

namespace FinTrack.Data
{
    internal class DataContextManager : IDataContextManager
    {
        private readonly object _contextLock = new object();
        private Dictionary<string, FinTrackDataContext> _contexts = new Dictionary<string, FinTrackDataContext>();
        private DbConnectionSettings _connectionSettings;
        private readonly ILifetimeScope _container;

        public DataContextManager(ILifetimeScope container, DbConnectionSettings connectionSettings)
        {
            _container = container;
            _connectionSettings = connectionSettings;
        }

        public T CreateRepository<T>(string id = "default")
           where T : class, IRepository
        {
            return _container.Resolve<T>(new TypedParameter(typeof(FinTrackDataContext), GetDataContext(id)));
        }

        #region private metods

        private FinTrackDataContext GetDataContext(string id = "default")
        {
            var contextKey = id;

            lock (_contextLock)
            {

                if (!_contexts.ContainsKey(contextKey))
                {
                    _contexts[contextKey] = new FinTrackDataContext(_connectionSettings);
                }

                return _contexts[contextKey];
            }
        }

        #endregion
    }
}
