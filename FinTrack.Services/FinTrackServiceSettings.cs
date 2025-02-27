using FinTrack.Data;

namespace FinTrack.Services
{
    public class FinTrackServiceSettings
    {
        public DbConnectionSettings ConnectionStrings { get; set; }
        public KafkaData Kafka { get; set; }
        public JwtSettings Auth { get; set; }
    }

    public class JwtSettings
    {
        public string? Secret { get; set; }

        public int TokenExpireMinutes { get; set; }

        public int RefreshTokenNumber { get; set; }
    }

    public class KafkaData
    {
        public string BootstrapServers { get; set; }
        public string ConvertResponseGroupId { get; set; }
        public string ConvertRequestGroupId { get; set; }
        public string FinTrackTopic { get; set; }
        public string FinTrackCurrencyExchanger { get; set; }
    }
}
