
namespace FinTrack.Model
{
    public class AuthToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public string RefreshToken { get; set; }
    }
}
