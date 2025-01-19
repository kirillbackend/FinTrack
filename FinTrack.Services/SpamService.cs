using FinTrack.Data.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Context;
using FinTrack.Services.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FinTrack.Services
{
    public class SpamService : AbstractService, ISpamService
    {
        private readonly IConfiguration _configuration;

        public SpamService(ILogger<SpamService> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager
            , LocalizationContextLocator localizationContext, ContextLocator contextLocator, IConfiguration configuration) 
            : base(logger, mapperFactory, dataContextManager, localizationContext, contextLocator)
        {
            _configuration = configuration;
        }

        public async Task Start(string text)
        {
            var settings = _configuration.Get<FinTrackServiceSettings>();

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var factory = new ConnectionFactory
            {
                HostName = settings.ConnectionStrings.RabbitMqHost,
                UserName = settings.ConnectionStrings.RabbitMqUser,
                Password = settings.ConnectionStrings.RabbitMqPass
            };


            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: settings.ConnectionStrings.RabbitMqQueueName
                , durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(text);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: settings.ConnectionStrings.RabbitMqQueueName, body: body);
        }

        public async Task<List<string>> GetSpam()
        {
            var settings = _configuration.Get<FinTrackServiceSettings>();

            var factory = new ConnectionFactory
            {
                HostName = settings.ConnectionStrings.RabbitMqHost,
                UserName = settings.ConnectionStrings.RabbitMqUser,
                Password = settings.ConnectionStrings.RabbitMqPass
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: settings.ConnectionStrings.RabbitMqQueueName, durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            var messages = new List<string>();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                messages.Add(message);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(settings.ConnectionStrings.RabbitMqQueueName, autoAck: true, consumer: consumer);

            return messages;
        }
    }
}
