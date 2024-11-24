namespace FinTrack.Services.Context
{
    public class UserContext : Contracts.IContext
    {
        public static UserContext Create(string email, int id, string role)
        {
            return new UserContext(email, id, role);
        }

        public UserContext(string email, int id, string role)
        {
            Id = id;
            Role = role;
            Email = email;
        }

        public string Email { get; set; }

        public int Id { get; set; }

        public string Role { get; set; }
    }
}
