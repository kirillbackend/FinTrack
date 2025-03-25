using FinTrack.Services.Context.Contracts;

namespace FinTrack.Services.Context
{
    public class UserContext : IContext
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Role { get; set; }

        public UserContext() { }

        public UserContext(string email, Guid id, string role)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            Id = id;
            Role = role;
            Email = email;
        }
    }
}
