using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Enums;
using FinTrack.Model;
using FinTrack.Services.Context;
using FinTrack.Services.Context.Contracts;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services
{
    public class FinanceService : AbstractService, IFinanceService
    {
        private readonly IContextLocator _contextLocator;
        private readonly IFilterService _filterService;
        private readonly IReportService _reportService;

        public FinanceService(ILogger<FinanceService> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager
            , IContextLocator contextLocator, IFilterService filterService, IReportService reportService)
            : base(logger, mapperFactory, dataContextManager)
        {
            _contextLocator = contextLocator;
            _filterService = filterService;
            _reportService = reportService;
        }

        public async Task AddFinanceAsync(FinanceDto financeDto)
        {

            Logger.LogInformation($"FinanceService.AddFinanceAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();
            var currencyRepositoty = DataContextManager.CreateRepository<ICurrencyRepository>();
            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            var currency =  await currencyRepositoty.GetAsync(financeDto.CurrencyId);

            if (currency == null)
            {
                Logger.LogWarning($"FinanceService.AddFinanceAsync the currency was not found. CurrencyId : {financeDto.CurrencyId}");
                throw new ValidationException("Currency was not found.");
            }

            var finance = mapper.MapFromDto(financeDto);
            finance.Id = new Guid();
            finance.CreatedDate = DateTime.UtcNow;
            finance.UpdatedDate = DateTime.UtcNow;
            await financeRepository.AddAssync(finance);

            Logger.LogInformation($"FinanceService.AddFinanceAsync completed");
        }

        public async Task DeleteAsync(Guid id)
        {
            Logger.LogInformation($"FinanceService.DeleteAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetAsync(id);

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
            Logger.LogInformation($"FinanceService.GetAsync({id}) started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetAsync(id);

            var userContext = _contextLocator.Get<UserContext>();

            if (userContext.Id != finance.UserId)
            {
                Logger.LogWarning($"FinanceService.GetAsync there is no access to the data.");
                throw new ValidationException("No access data.");
            }

            if (finance == null)
            {
                Logger.LogWarning($"FinanceService.GetAsync the finance was not found. Id : {id}");
                throw new ValidationException("Finance was not found.");
            }

            var mapper = MapperFactory.GetMapper<IFinanceMapper>();
            var financeDto = mapper.MapToDto(finance);

            Logger.LogInformation($"FinanceService.GetAsync({id}) completed");
            return financeDto;
        }

        public async Task<IEnumerable<FinanceDto>> GetFinancesAsync()
        {
            Logger.LogInformation($"FinanceService.GetAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();
            var mapper = MapperFactory.GetMapper<IFinanceMapper>();

            var finances = await financeRepository.GetAsync();

            var financesDto = mapper.MapCollectionToDto(finances);

            Logger.LogInformation($"FinanceService.GetAsync completed");
            return financesDto;
        }

        public async Task<FinanceDto> UpdateAsync(FinanceDto financeDto)
        {
            Logger.LogInformation($"FinanceService.UpdateAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetAsync(financeDto.Id);

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

        public async Task AddCategoryAsync(Guid financeId, Guid categoryId)
        {
            Logger.LogInformation($"FinanceService.AddCategoryAsync started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var finance = await financeRepository.GetAsync(financeId);
            finance.CategoryId = categoryId;
            finance.UpdatedDate = DateTime.UtcNow;
            await DataContextManager.SaveAsync();

            Logger.LogInformation($"FinanceService.AddCategoryAsync completed");
        }

        public async Task<Report> GetReport(Guid userId, ReportType reportType)
        {
            Logger.LogInformation($"FinanceService.GetReport started");

            var financeRepository = DataContextManager.CreateRepository<IFinanceRepository>();

            var filter = await _filterService.CreateReportFilter(userId, reportType);
            var finances =await financeRepository.GetAsync(filter);
            var report = await _reportService.CreateReport(finances);

            Logger.LogInformation($"FinanceService.GetReport completed");
            return report;
        }
    }
}
