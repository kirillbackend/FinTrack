using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Model;
using FinTrack.Services.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;

namespace FinTrack.Services
{
    public class CurrencyService : AbstractService, ICurrencyService
    {
        public CurrencyService(ILogger<CurrencyService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, ContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager, contextLocator)
        {
        }

        public async Task<Currency> GetCurrencyById(int id)
        {
            Logger.LogInformation($"CurrencyService.GetCurrencyById({id}) started;");

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();

            var currency = await currencyRepository.GetCurrencyById(id);

            if (currency == null)
            {
                Logger.LogWarning($"CurrencyService.GetCurrencyById the currency was not found. Id : {id}");
                throw new ValidationException("Currency was not found.", resourceProvider.Get("CurrencyWasNotFound"));
            }

            Logger.LogInformation($"CurrencyService.GetCurrencyById({id}) completed;");
            return currency;
        }

        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
            Logger.LogInformation($"CurrencyService.GetCurrencies started;");

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;

            var currencyRepository = DataContextManager.CreateRepository<ICurrencyRepository>();

            var currency = await currencyRepository.GetCurrencies();

            Logger.LogInformation($"CurrencyService.GetCurrencies completed;");
            return currency;
        }  
    }
}
