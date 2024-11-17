using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;

namespace FinTrack.Services
{
    public class FinanceService : AbstractService, IFinanceService
    {
        public FinanceService(ILogger<FinanceService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, ContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager, contextLocator)
        {
        }

        public async Task AddFinance(FinanceDto currencyDto)
        {
            Logger.LogInformation($"FinanceService.AddFinance started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();
            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            var finance = mapper.MapFromDto(currencyDto);
            finance.CreatedDate = DateTime.UtcNow;
            finance.UpdatedDate = DateTime.UtcNow;
            financeRepository.Add(finance);

            Logger.LogInformation($"FinanceService.AddFinance completed");
        }

        public async Task Delete(int id)
        {
            Logger.LogInformation($"FinanceService.Delete started");

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;
            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = financeRepository.Delete(id);

            if (finance == null)
            {
                Logger.LogWarning($"FinanceService.Delete the finance was not found. Id : {id}");
                throw new ValidationException("Finance was not found.", resourceProvider.Get("FinanceWasNotFound"));
            }

            await financeRepository.Delete(id);

            Logger.LogInformation($"FinanceService.Delete({id}) completed");
        }

        public async Task<FinanceDto> GetFinanceById(int id)
        {
            Logger.LogInformation($"FinanceService.GetFinanceById({id}) started");

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetFinanceById(id);  

            if (finance == null)
            {
                Logger.LogWarning($"FinanceService.GetFinanceById the finance was not found. Id : {id}");
                throw new ValidationException("Finance was not found.", resourceProvider.Get("FinanceWasNotFound"));
            }

            var mapper = MapperFactory.GetMapper<IFinanceMapper>();
            var financeDto = mapper.MapToDto(finance);

            Logger.LogInformation($"FinanceService.GetFinanceById({id}) completed");
            return financeDto;
        }

        public async Task<IEnumerable<FinanceDto>> GetFinances()
        {
            Logger.LogInformation($"FinanceService.GetFinances started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();
            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            var finances = await financeRepository.GetFinances();

            var financesDto = mapper.MapCollectionToDto(finances);

            Logger.LogInformation($"FinanceService.GetFinances completed");
            return financesDto;
        }

        public async Task<FinanceDto> Update(FinanceDto financeDto)
        {
            Logger.LogInformation($"FinanceService.Update started");

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetFinanceById(financeDto.Id);

            if (finance == null)
            {
                Logger.LogWarning($"FinanceService.Update the finance was not found. Id : {financeDto.Id}");
                throw new ValidationException("Finance was not found.", resourceProvider.Get("FinanceWasNotFound"));
            }


            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            mapper.MapFromDto(financeDto, destination: finance);
            finance.UpdatedDate = DateTime.UtcNow;
            await DataContextManager.SaveAsync();

            Logger.LogInformation($"FinanceService.Update completed");
            return financeDto;
        }
    }
}
