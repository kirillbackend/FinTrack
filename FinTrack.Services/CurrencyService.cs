using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Dtos;
using FinTrack.Services.Context;
using FinTrack.Services.Wrappers.Contracts;
using Confluent.Kafka;

namespace FinTrack.Services
{
    public class CurrencyService : AbstractService, ICurrencyService
    {
        private readonly IFixerAPIWrapper _fixerAPIWrapper;
        private readonly IProducer<Null, string> _producer;
        private readonly IConsumer<Null, string> _consumer;
        private readonly FinTrackServiceSettings _settings;
        private const string RequestTopic = "conwersionRequestTopic";
        private const string ResponceTopic = "conwersionResponceTopic";

        public CurrencyService(ILogger<CurrencyService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, LocalizationContextLocator localization
            , ContextLocator contextLocator, IFixerAPIWrapper fixerAPIWrapper, FinTrackServiceSettings settings)
            : base(logger, mapperFactory, dataContextManager, localization, contextLocator)
        {
            _settings = settings;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _settings.Kafka.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = _settings.Kafka.ClientId,
                GroupId = _settings.Kafka.ConvertResultGroupId,
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };

            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = _settings.Kafka.BootstrapServers,
                ClientId = _settings.Kafka.ClientId,
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };

            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            _fixerAPIWrapper = fixerAPIWrapper;
        }

        public async Task<CurrencyDto> GetCurrencyById(Guid id)
        {
            Logger.LogInformation($"CurrencyService.GetCurrencyById({id}) started");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();

            var currency = await currencyRepository.GetCurrencyById(id);

            if (currency == null)
            {
                Logger.LogWarning($"CurrencyService.GetCurrencyById the currency was not found. Id : {id}");
                throw new ValidationException("Currency was not found.", resourceProvider.Get("CurrencyWasNotFound"));
            }

            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currencyDto = mapper.MapToDto(currency);

            Logger.LogInformation($"CurrencyService.GetCurrencyById({id}) completed");
            return currencyDto;
        }

        public async Task<IEnumerable<CurrencyDto>> GetCurrencies()
        {
            Logger.LogInformation($"CurrencyService.GetCurrencies started");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();

            var currency = await currencyRepository.GetCurrencies();

            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currenciesDto = mapper.MapCollectionToDto(currency);

            Logger.LogInformation($"CurrencyService.GetCurrencies completed");
            return currenciesDto;
        }

        public async Task AddCurrency(CurrencyDto currencyDto)
        {
            Logger.LogInformation("CurrencyService.AddCurrency started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();

            var currency = mapper.MapFromDto(currencyDto);
            await currencyRepository.Add(currency);

            Logger.LogInformation("CurrencyService.AddCurrency completed");
        }

        public async Task Delete(Guid id)
        {
            Logger.LogInformation($"CurrencyService.Delete({id}) started");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();

            var currency = await currencyRepository.GetCurrencyById(id);

            if (currency == null)
            {
                Logger.LogWarning($"CurrencyService.Delete the currency was not found. Id : {id}");
                throw new ValidationException("Currency was not found.", resourceProvider.Get("CurrencyWasNotFound"));
            }

            await currencyRepository.Delete(id);

            Logger.LogInformation($"CurrencyService.Delete({id})  completed");
        }

        public async Task<CurrencyDto> Update(CurrencyDto currencyDto)
        {
            Logger.LogInformation("CurrencyService.Update started");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();

            var currency = await currencyRepository.GetCurrencyById(currencyDto.Id);

            if (currency == null)
            {
                Logger.LogWarning($"CurrencyService.Update the currency was not found. Id : {currencyDto.Id}");
                throw new ValidationException("Currency was not found.", resourceProvider.Get("CurrencyWasNotFound"));
            }

            mapper.MapFromDto(currencyDto, destination: currency);
            await DataContextManager.SaveAsync();

            Logger.LogInformation("CurrencyService.Update completed");
            return currencyDto;
        }

        public async Task<decimal> ConvertCurrency(string to, string from, string amount)
        {
            Logger.LogInformation("CurrencyService.ConvertCurrency started");

            var result = await _fixerAPIWrapper.ConvertCurrency(to, from, amount);

            Logger.LogInformation("CurrencyService.ConvertCurrency completed");
            return result;
        }




        public async Task ProduceAsync(string message)
        {
            var kafkamessage = new Message<Null, string> { Value = message, };

            await _producer.ProduceAsync(RequestTopic, kafkamessage);
        }


        public async Task<string> StartAsync()
        {
            _consumer.Subscribe(ResponceTopic);
            var result = string.Empty;
            try
            {
                do
                {
                    var consumeResult = _consumer.Consume();
                    result = consumeResult.Message.Value;
                } while (string.IsNullOrEmpty(result));

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
