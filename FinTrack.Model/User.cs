using FinTrack.Enums;

namespace FinTrack.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public UserRole UserRole { get; set; }

        public List<Finance> Finances { get; set; }
    }
}
