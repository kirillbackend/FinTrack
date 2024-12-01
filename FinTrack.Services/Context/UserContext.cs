namespace FinTrack.Services.Context
{
    public class UserContext : Contracts.IContext
    {
        public static UserContext Create(string email, Guid id, string role)
        {
            return new UserContext(email, id, role);
        }

        public UserContext(string email, Guid id, string role)
        {
            Id = id;
            Role = role;
            Email = email;
        }

        public string Email { get; set; }

        public Guid Id { get; set; }

        public string Role { get; set; }
    }
}
