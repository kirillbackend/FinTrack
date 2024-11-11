using FinTrack.Data;

namespace FinTrack.Services
{
    public class FinTrackServiceSettings
    {
        public DbConnectionSettings ConnectionStrings { get; set; }
        public JwtSettings Auth { get; set; }
    }

    public class JwtSettings
    {
        public string? Secret { get; set; }

        public int TokenExpireMinutes { get; set; }
    }
}
