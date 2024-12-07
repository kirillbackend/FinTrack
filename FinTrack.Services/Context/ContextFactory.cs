using FinTrack.Services.Context.Contracts;

namespace FinTrack.Services.Context
{
    public class ContextFactory : IContextFactory
    {
        public ContextFactory() { }

        public UserContext CreateLocaleContext(string email, Guid id, string role)
        {
            return new UserContext(email, id, role);
        }
    }
}
