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
    public class UserService : AbstractService, IUserService
    {
        public UserService(ILogger<UserService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, ContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager, contextLocator)
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

        public async Task Delete(int id)
        {
            Logger.LogInformation($"UserService.Delete({id} started)");

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

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
            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var users = await repo.GetByEmail(email);
            var usersDto = mapper.MapToDto(users);

            Logger.LogInformation("UserService.GetByEmail completed");
            return usersDto;
        }

        public async Task<UserDto> GetById(int id)
        {
            Logger.LogInformation($"UserService.GetById({id} started)");

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

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

            var resourceProvider = ContextLocator.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

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
