using Confluent.Kafka;

namespace FinTrack.Services.Kafka.Contracts
{
    public interface ICurrencyExchangeKafkaProducer
    {
        Task ProduceAsync(string topic, Message<string, string> message);
    }
}
