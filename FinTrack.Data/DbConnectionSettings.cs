
namespace FinTrack.Data
{
    public class DbConnectionSettings
    {
        public string MSSQLDatabase { get; set; }
        public string RedisDatabase { get; set; }
        public string RedisInstanceName { get; set; }
        public string RabbitMqHost { get; set; }
        public string RabbitMqQueueName { get; set; }
        public string RabbitMqUser { get; set; }
        public string RabbitMqPass { get; set; }
       
    }
}
