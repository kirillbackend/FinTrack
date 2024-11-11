using FinTrack.Localization.Conrtacts;

namespace FinTrack.Localization
{
    public class ContextLocator
    {
        private Dictionary<Type, IContext> _contexts = new Dictionary<Type, IContext>();

        public IContextFactory ContextFactory;

        public ContextLocator(IContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public T GetContext<T>() where T : class, IContext
        {
            if (!_contexts.ContainsKey(typeof(T)))
            {
                throw new Exception("No context " + typeof(T).Name + " set");
            }

            return (T)_contexts[typeof(T)];
        }

        public bool TryGetContext<T>(out T context) where T : class, IContext
        {
            if (!_contexts.ContainsKey(typeof(T)))
            {
                context = null;
                return false;
            }

            context = (T)_contexts[typeof(T)];
            return true;
        }

        public void AddContext<T>(T context) where T : class, IContext
        {
            if (_contexts.ContainsKey(typeof(T)))
            {
                throw new Exception($"Context {typeof(T)} already registered in current container");
            }

            _contexts.Add(typeof(T), context);
        }

        public void RemoveContext<T>() where T : class, IContext
        {
            if (!_contexts.ContainsKey(typeof(T)))
            {
                throw new Exception($"Context {typeof(T)} is not registered in current container");
            }

            _contexts.Remove(typeof(T));
        }
    }
}
