using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Services.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Dtos;
using FinTrack.Services.Wrappers.Contracts;

namespace FinTrack.Services
{
    public class CurrencyService : AbstractService, ICurrencyService
    {
        private readonly IFixerAPIWrapper _fixerAPIWrapper;

        public CurrencyService(ILogger<CurrencyService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, IFixerAPIWrapper fixerAPIWrapper)
            : base(logger, mapperFactory, dataContextManager)
        {
            _fixerAPIWrapper = fixerAPIWrapper;
        }

        public async Task<CurrencyDto> GetCurrencyByIdAsync(Guid id)
        {
            Logger.LogInformation($"CurrencyService.GetCurrencyByIdAsync({id}) started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var currency = await currencyRepository.GetCurrencyByIdAsync(id);

            if (currency == null)
            {
                Logger.LogWarning($"CurrencyService.GetCurrencyByIdAsync the currency was not found. Id : {id}");
                throw new ValidationException("Currency was not found.");
            }

            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currencyDto = mapper.MapToDto(currency);

            Logger.LogInformation($"CurrencyService.GetCurrencyByIdAsync({id}) completed");
            return currencyDto;
        }

        public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync()
        {
            Logger.LogInformation($"CurrencyService.GetCurrenciesAsync started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var currency = await currencyRepository.GetCurrenciesAsync();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currenciesDto = mapper.MapCollectionToDto(currency);

            Logger.LogInformation($"CurrencyService.GetCurrenciesAsync completed");
            return currenciesDto;
        }

        public async Task AddCurrencyAsync(CurrencyDto currencyDto)
        {
            Logger.LogInformation("CurrencyService.AddCurrencyAsync started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currency = mapper.MapFromDto(currencyDto);
            await currencyRepository.AddAsync(currency);

            Logger.LogInformation("CurrencyService.AddCurrencyAsync completed");
        }

        public async Task DeleteAsync(Guid id)
        {
            Logger.LogInformation($"CurrencyService.DeleteAsync({id}) started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var currency = await currencyRepository.GetCurrencyByIdAsync(id);

            if (currency == null)
            {
                Logger.LogWarning($"CurrencyService.DeleteAsync the currency was not found. Id : {id}");
                throw new ValidationException("Currency was not found.");
            }

            await currencyRepository.DeleteAsync(id);

            Logger.LogInformation($"CurrencyService.DeleteAsync({id})  completed");
        }

        public async Task<CurrencyDto> UpdateAsync(CurrencyDto currencyDto)
        {
            Logger.LogInformation("CurrencyService.UpdateAsync started");

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();
            var mapper = MapperFactory.GetMapper<ICurrencyMapper>();
            var currency = await currencyRepository.GetCurrencyByIdAsync(currencyDto.Id);

            if (currency == null)
            {
                Logger.LogWarning($"CurrencyService.UpdateAsync the currency was not found. Id : {currencyDto.Id}");
                throw new ValidationException("Currency was not found.");
            }

            mapper.MapFromDto(currencyDto, destination: currency);
            await DataContextManager.SaveAsync();

            Logger.LogInformation("CurrencyService.UpdateAsync completed");
            return currencyDto;
        }

        public async Task<decimal> ConvertCurrencyAsync(string to, string from, string amount)
        {
            Logger.LogInformation("CurrencyService.ConvertCurrencyAsync started");

            var result = await _fixerAPIWrapper.ConvertCurrencyAsync(to, from, amount);

            Logger.LogInformation("CurrencyService.ConvertCurrencyAsync completed");
            return result;
        }
    }
}
