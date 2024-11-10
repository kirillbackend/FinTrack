using FinTrack.Services;

namespace FinTrack.RestApi
{
    public class ApiSettings : FinTrackSettings
    {
        public string[] AllowedOrigins { get; set; }
        public JwtSettings Auth { get; set; }
    }

    public class JwtSettings
    {
        public string? Secret { get; set; }

        public int TokenExpireMinutes { get; set; }
    }
}
