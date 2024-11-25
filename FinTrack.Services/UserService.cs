using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Context.Contracts;

namespace FinTrack.Services
{
    public class UserService : AbstractService, IUserService
    {
        public UserService(ILogger<UserService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, LocalizationContextLocator localizationContext, IContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager, localizationContext, contextLocator)
        {
        }

        public async Task AddUser(UserDto userDto)
        {
            Logger.LogInformation("UserService.AddUser started");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var user = mapper.MapFromDto(userDto);
            repo.Add(user);

            Logger.LogInformation("UserService.AddUser completed");
        }

        public async Task Delete(Guid id)
        {
            Logger.LogInformation($"UserService.Delete({id} started)");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            if (UserContext.Id != id)
            {
                Logger.LogWarning($"UserService.Delete there is no access to the data.");
                throw new ValidationException("No access data.", resourceProvider.Get("NoAccessData"));
            }

            var user = await repo.GetById(id);

            if (user == null)
            {
                Logger.LogWarning($"UserService.Delete the user was not found. Id : {id}");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            await repo.Delete(id);

            Logger.LogInformation($"UserService.Delete({id}) completed");
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            Logger.LogInformation("UserService.GetByEmail started");
            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserAuthMapper>();

            var user = await repo.GetByEmail(email);
            var userDto = mapper.MapToDto(user);

            Logger.LogInformation("UserService.GetByEmail completed");
            return userDto;
        }

        public async Task<UserDto> GetById(Guid id)
        {
            Logger.LogInformation($"UserService.GetById({id} started)");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            if (UserContext.Id != id)
            {
                Logger.LogWarning($"UserService.GetById there is no access to the data.");
                throw new ValidationException("No access data.", resourceProvider.Get("NoAccessData"));
            }

            var user = await repo.GetById(id);

            if (user == null)
            {
                Logger.LogWarning($"UserService.GetById the user was not found. Id : {id}");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            var userDto = mapper.MapToDto(user);

            Logger.LogInformation($"UserService.GetById({id}) completed");
            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            Logger.LogInformation("UserService.GetUsers started");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var users = await repo.GetUsers();

            var usersDto = mapper.MapCollectionToDto(users);

            Logger.LogInformation($"UserService.GetUsers completed");
            return usersDto;
        }

        public async Task<UserDto> Update(UserDto userDto)
        {
            Logger.LogInformation($"UserService.Update started)");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            if (UserContext.Id != userDto.Id)
            {
                Logger.LogWarning($"UserService.Update there is no access to the data.");
                throw new ValidationException("No access data.", resourceProvider.Get("NoAccessData"));
            }

            var user = await repo.GetById(userDto.Id);

            if (user == null)
            {
                Logger.LogWarning($"UserService.Update the user was not found.");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            mapper.MapFromDto(userDto, destination: user);
            await DataContextManager.SaveAsync();

            Logger.LogInformation($"UserService.Update completed");
            return userDto;
        }
    }
}
