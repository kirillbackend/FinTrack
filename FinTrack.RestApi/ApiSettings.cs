using FinTrack.Services;

namespace FinTrack.RestApi
{
    public class ApiSettings : FinTrackServiceSettings
    {
        public string[] AllowedOrigins { get; set; }
    }
}
