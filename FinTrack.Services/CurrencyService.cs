using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Services.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using FinTrack.Services.Kafka.Contracts;

namespace FinTrack.Services
{
    public class CurrencyService : AbstractService, ICurrencyService
    {
        private readonly IDistributedCache _cache;
        private readonly ICurrencyExchangeKafkaProducer _kafkaProducer;
        private readonly IValidatorService _validatorService;
        private const char _separator = ':';

        public CurrencyService(ILogger<CurrencyService> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager, IDistributedCache cache,
            ICurrencyExchangeKafkaProducer kafkaProducer, IValidatorService validatorService)
            : base(logger, mapperFactory, dataContextManager)
        {
            _kafkaProducer = kafkaProducer; 
            _cache = cache;
            _validatorService = validatorService;
        }

        public async Task<CurrencyDto> GetCurrencyByIdAsync(Guid id)
        {
            Logger.LogInformation($"CurrencyService.GetAsync({id}) started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var currency = await currencyRepository.GetAsync(id);

            await _validatorService.CurrencyValidate(currency);

            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currencyDto = mapper.MapToDto(currency);

            Logger.LogInformation($"CurrencyService.GetAsync({id}) completed");
            return currencyDto;
        }

        public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync()
        {
            Logger.LogInformation($"CurrencyService.GetAsync started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var currency = await currencyRepository.GetAsync();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currenciesDto = mapper.MapCollectionToDto(currency);

            Logger.LogInformation($"CurrencyService.GetAsync completed");
            return currenciesDto;
        }

        public async Task AddCurrencyAsync(CurrencyDto currencyDto)
        {
            Logger.LogInformation("CurrencyService.AddAsync started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currency = mapper.MapFromDto(currencyDto);

            currency.Id = new Guid();

            await currencyRepository.AddAsync(currency);

            Logger.LogInformation("CurrencyService.AddAsync completed");
        }

        public async Task DeleteAsync(Guid id)
        {
            Logger.LogInformation($"CurrencyService.DeleteAsync({id}) started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var currency = await currencyRepository.GetAsync(id);

            await _validatorService.CurrencyValidate(currency);

            await currencyRepository.DeleteAsync(id);

            Logger.LogInformation($"CurrencyService.DeleteAsync({id})  completed");
        }

        public async Task<CurrencyDto> UpdateAsync(CurrencyDto currencyDto)
        {
            Logger.LogInformation("CurrencyService.UpdateAsync started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currency = await currencyRepository.GetAsync(currencyDto.Id);

            await _validatorService.CurrencyValidate(currency);

            mapper.MapFromDto(currencyDto, destination: currency);
            await DataContextManager.SaveAsync();

            Logger.LogInformation("CurrencyService.UpdateAsync completed");
            return currencyDto;
        }

        public async Task<decimal> ConvertCurrencyAsync(string to, string from, string amount)
        {
            Logger.LogInformation("CurrencyService.ConvertCurrencyAsync started");

            var cashKey = to + _separator + from + _separator + amount;
            var cash = await _cache.GetStringAsync(cashKey);

            if (cash == null)
            {
                await _kafkaProducer.ProduceAsync("fintrackcurrencyexchanger-topic", new Confluent.Kafka.Message<string, string>
                {
                    Key = DateTime.Now.ToString(),
                    Value = cashKey
                });

                _kafkaProducer.Dispose();

                var stoppingToken = new CancellationToken();

                //var consumer = new CurrencyExchangeKafkaConsumer();

                var result = 0; //await _kafkaConsumer.ConsumeAsync("fintrackcurrencyexchanger-topic", stoppingToken);

                await _cache.SetStringAsync(cashKey, result.ToString(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                Logger.LogInformation("CurrencyService.ConvertCurrencyAsync completed");
                return decimal.Parse(result.ToString());
            }
            else
            {
                return decimal.Parse(cash.Replace(".", ","));
            }

        }
    }
}
