using FinTrack.Services.Context.Contracts;

namespace FinTrack.Services.Context
{
    internal class ContextLocator : IContextLocator
    {
        private Dictionary<Type, IContext> _contexts = new Dictionary<Type, IContext>();

        public void AddContext<T>(T context)
            where T : class, IContext
        {
            if (_contexts.ContainsKey(typeof(T)))
            {
                throw new Exception($"Context {typeof(T)} already registered in current container");
            }

            _contexts.Add(typeof(T), context);
        }

        public T Get<T>() where T : class, IContext
        {
            if (_contexts.TryGetValue(typeof(T), out IContext context))
            {
                return (T)context;
            }

            throw new Exception($"Context {typeof(T)} is not registered in current container");
        }
    }
}
