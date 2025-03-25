using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Context;
using FinTrack.Services.Context.Contracts;

namespace FinTrack.Services
{
    public class FinanceService : AbstractService, IFinanceService
    {
        private readonly IContextLocator _contextLocator;

        public FinanceService(ILogger<FinanceService> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager, IContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager)
        {
            _contextLocator = contextLocator;
        }

        public async Task AddFinanceAsync(FinanceDto currencyDto)
        {
            Logger.LogInformation($"FinanceService.AddFinanceAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();
            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            var finance = mapper.MapFromDto(currencyDto);
            finance.CreatedDate = DateTime.UtcNow;
            finance.UpdatedDate = DateTime.UtcNow;
            await financeRepository.AddAssync(finance);

            Logger.LogInformation($"FinanceService.AddFinanceAsync completed");
        }

        public async Task DeleteAsync(Guid id)
        {
            Logger.LogInformation($"FinanceService.DeleteAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetFinanceByIdAsync(id);

            var userContext = _contextLocator.Get<UserContext>();

            if (userContext.Id != finance.UserId)
            {
                Logger.LogWarning($"FinanceService.DeleteAsync there is no access to the data.");
                throw new ValidationException("No access data.");
            }

            await financeRepository.DeleteAsync(id);

            if (finance == null)
            {
                Logger.LogWarning($"FinanceService.DeleteAsync the finance was not found. Id : {id}");
                throw new ValidationException("Finance was not found.");
            }

            await financeRepository.DeleteAsync(id);

            Logger.LogInformation($"FinanceService.DeleteAsync({id}) completed");
        }

        public async Task<FinanceDto> GetFinanceByIdAsync(Guid id)
        {
            Logger.LogInformation($"FinanceService.GetFinanceByIdAsync({id}) started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetFinanceByIdAsync(id);

            var userContext = _contextLocator.Get<UserContext>();

            if (userContext.Id != finance.UserId)
            {
                Logger.LogWarning($"FinanceService.GetFinanceByIdAsync there is no access to the data.");
                throw new ValidationException("No access data.");
            }

            if (finance == null)
            {
                Logger.LogWarning($"FinanceService.GetFinanceByIdAsync the finance was not found. Id : {id}");
                throw new ValidationException("Finance was not found.");
            }

            var mapper = MapperFactory.GetMapper<IFinanceMapper>();
            var financeDto = mapper.MapToDto(finance);

            Logger.LogInformation($"FinanceService.GetFinanceByIdAsync({id}) completed");
            return financeDto;
        }

        public async Task<IEnumerable<FinanceDto>> GetFinancesAsync()
        {
            Logger.LogInformation($"FinanceService.GetFinancesAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();
            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            var finances = await financeRepository.GetFinancesAsync();

            var financesDto = mapper.MapCollectionToDto(finances);

            Logger.LogInformation($"FinanceService.GetFinancesAsync completed");
            return financesDto;
        }

        public async Task<FinanceDto> UpdateAsync(FinanceDto financeDto)
        {
            Logger.LogInformation($"FinanceService.UpdateAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetFinanceByIdAsync(financeDto.Id);

            var userContext = _contextLocator.Get<UserContext>();

            if (userContext.Id != finance.UserId)
            {
                Logger.LogWarning($"FinanceService.UpdateAsync there is no access to the data.");
                throw new ValidationException("No access data.");
            }

            if (finance == null)
            {
                Logger.LogWarning($"FinanceService.UpdateAsync the finance was not found. Id : {financeDto.Id}");
                throw new ValidationException("Finance was not found.");
            }

            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            mapper.MapFromDto(financeDto, destination: finance);
            finance.UpdatedDate = DateTime.UtcNow;
            await DataContextManager.SaveAsync();

            Logger.LogInformation($"FinanceService.UpdateAsync completed");
            return financeDto;
        }
    }
}
