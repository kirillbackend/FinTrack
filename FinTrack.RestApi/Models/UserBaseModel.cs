using System.ComponentModel.DataAnnotations;

namespace FinTrack.RestApi.Models
{
    public abstract class UserBaseModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
