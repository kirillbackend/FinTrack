using FinTrack.Enums;

namespace FinTrack.Services.Dtos
{
    public class UserDto : UserBaseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate{ get; set; }
        public UserRole UserRole { get; set; }
    }
}
