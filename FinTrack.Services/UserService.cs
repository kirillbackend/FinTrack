using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Context;
using FinTrack.Services.Context.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using FinTrack.Model;
using Newtonsoft.Json;

namespace FinTrack.Services
{
    public class UserService : AbstractService, IUserService
    {
        private readonly IContextLocator _contextLocator;
        private readonly IDistributedCache _cache;
        private readonly IValidatorService _validatorService;

        public UserService(ILogger<UserService> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager, IContextLocator contextLocator, 
            IDistributedCache cache, IValidatorService validatorService)
            : base(logger, mapperFactory, dataContextManager)
        {
            _contextLocator = contextLocator;
            _cache = cache;
            _validatorService = validatorService;
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            Logger.LogInformation("UserService.AddUserAsync started");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var user = mapper.MapFromDto(userDto);
            await repo.AddAsync(user);

            Logger.LogInformation("UserService.AddUserAsync completed");
        }

        public async Task DeleteAsync(Guid id)
        {
            Logger.LogInformation($"UserService.DeleteAsync({id} started)");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var userContext = _contextLocator.Get<UserContext>();
            await _validatorService.UserIdValidate(id, userContext.Id);
            var user = await repo.GetAsync(id);
            await _validatorService.UserValidate(user);
            await repo.DeleteAsync(id);

            Logger.LogInformation($"UserService.DeleteAsync({id}) completed");
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            Logger.LogInformation("UserService.GetByEmailAsync started");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserAuthMapper>();

            User? user = null;
            var cashKey = email;
            var cash = await _cache.GetStringAsync(cashKey);

            if (cash != null)
            {
                user = JsonConvert.DeserializeObject<User>(cash);
            }
            else
            {
                user = await repo.GetByEmailAsync(email);

                if (user != null)
                {
                    cash = JsonConvert.SerializeObject(user);

                    await _cache.SetStringAsync(cashKey, cash, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                }
            }

            var userDto = mapper.MapToDto(user);

            Logger.LogInformation("UserService.GetByEmailAsync completed");
            return userDto;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            Logger.LogInformation($"UserService.GetAsync({id} started)");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();
            var userContext = _contextLocator.Get<UserContext>();

            await _validatorService.UserIdValidate(id, userContext.Id);

            User? user = null;
            var cashKey = id.ToString();
            var cash = await _cache.GetStringAsync(cashKey);

            if (cash != null)
            {
                user = JsonConvert.DeserializeObject<User>(cash);
            }
            else
            {
                user = await repo.GetAsync(id);

                await _validatorService.UserValidate(user);

                cash = JsonConvert.SerializeObject(user);

                await _cache.SetStringAsync(cashKey, cash, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            var userDto = mapper.MapToDto(user);

            Logger.LogInformation($"UserService.GetAsync({id}) completed");
            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            Logger.LogInformation("UserService.GetAsync started");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var users = await repo.GetAsync();
            var usersDto = mapper.MapCollectionToDto(users);

            Logger.LogInformation($"UserService.GetAsync completed");
            return usersDto;
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            Logger.LogInformation("UserService.UpdateAsync started)");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var userContext = _contextLocator.Get<UserContext>();
            await _validatorService.UserIdValidate(userDto.Id, userContext.Id);
            var user = await repo.GetAsync(userDto.Id);
            await _validatorService.UserValidate(user);
            mapper.MapFromDto(userDto, destination: user);
            await DataContextManager.SaveAsync();

            Logger.LogInformation($"UserService.UpdateAsync completed");
            return userDto;
        }
    }
}
