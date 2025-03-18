using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace FinTrack.Services.Kafka
{
    public class CurrencyExchangeKafkaConsumer : BackgroundService
    {
        private readonly FinTrackServiceSettings _settings;

        public CurrencyExchangeKafkaConsumer(FinTrackServiceSettings settings)
        {
            _settings = settings;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                _ = ConsumeAsync(_settings.Kafka.FinTrackTopic, stoppingToken);
            }, stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _settings.Kafka.ConvertResponseGroupId,
                BootstrapServers = _settings.Kafka.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerResult = consumer.Consume(stoppingToken);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Key = {consumerResult.Message.Key} \nValue = {consumerResult.Message.Value}");
            }

            consumer.Close();
        }
    }
}
