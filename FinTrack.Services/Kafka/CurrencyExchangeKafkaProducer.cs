using Confluent.Kafka;
using FinTrack.Services.Kafka.Contracts;

namespace FinTrack.Services.Kafka
{       
    public class CurrencyExchangeKafkaProducer : ICurrencyExchangeKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly FinTrackServiceSettings _settings;

        public CurrencyExchangeKafkaProducer(FinTrackServiceSettings settings)
        {
            _settings = settings;

            var config = new ConsumerConfig
            {
                GroupId = _settings.Kafka.ConvertRequestGroupId,
                BootstrapServers = _settings.Kafka.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public Task ProduceAsync(string topic, Message<string, string> message)
        {

           return _producer.ProduceAsync(topic, message);
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
