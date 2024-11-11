using FinTrack.Data.Contracts;
using FinTrack.Model;
using FinTrack.Services.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services
{
    public class CurrencyService : AbstractService, ICurrencyService
    {
        public CurrencyService(ILogger<CurrencyService> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager)
            : base(logger, mapperFactory, dataContextManager)
        {
        }

        public async Task<Currency> GetCurrencyById(int id)
        {
            Logger.LogInformation($"CurrencyService.GetCurrencyById({id}) started;");
            Logger.LogInformation($"CurrencyService.GetCurrencyById({id}) completed;");
        }
    }
}
