using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services
{
    public class UserService : AbstractService, IUserService
    {
        public UserService(ILogger<UserService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, ContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager, contextLocator)
        {
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            Logger.LogInformation("UserService.GetUserByUserName started");
            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var user = await repo.GetUserByEmail(email);
            var userDto = mapper.MapToDto(user);

            Logger.LogInformation("UserService.GetUserByUserName completed");
            return userDto;
        }
    }
}
