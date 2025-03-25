﻿using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Context;

namespace FinTrack.Services
{
    public class UserService : AbstractService, IUserService
    {
        public UserService(ILogger<UserService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, LocalizationContextLocator localizationContext, ContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager, localizationContext, contextLocator)
        {
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

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            if (UserContext.Id != id)
            {
                Logger.LogWarning($"UserService.DeleteAsync there is no access to the data.");
                throw new ValidationException("No access data.", resourceProvider.Get("NoAccessData"));
            }

            var user = await repo.GetByIdAsync(id);

            if (user == null)
            {
                Logger.LogWarning($"UserService.DeleteAsync the user was not found. Id : {id}");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            await repo.DeleteAsync(id);

            Logger.LogInformation($"UserService.DeleteAsync({id}) completed");
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            Logger.LogInformation("UserService.GetByEmailAsync started");
            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserAuthMapper>();

            var user = await repo.GetByEmailAsync(email);
            var userDto = mapper.MapToDto(user);

            Logger.LogInformation("UserService.GetByEmailAsync completed");
            return userDto;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            Logger.LogInformation($"UserService.GetByIdAsync({id} started)");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            if (UserContext.Id != id)
            {
                Logger.LogWarning($"UserService.GetByIdAsync there is no access to the data.");
                throw new ValidationException("No access data.", resourceProvider.Get("NoAccessData"));
            }

            var user = await repo.GetByIdAsync(id);

            if (user == null)
            {
                Logger.LogWarning($"UserService.GetByIdAsync the user was not found. Id : {id}");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            var userDto = mapper.MapToDto(user);

            Logger.LogInformation($"UserService.GetByIdAsync({id}) completed");
            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            Logger.LogInformation("UserService.GetUsersAsync started");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            var users = await repo.GetUsersAsync();

            var usersDto = mapper.MapCollectionToDto(users);

            Logger.LogInformation($"UserService.GetUsersAsync completed");
            return usersDto;
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            Logger.LogInformation("UserService.UpdateAsync started)");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var mapper = MapperFactory.GetMapper<IUserMapper>();

            if (UserContext.Id != userDto.Id)
            {
                Logger.LogWarning("UserService.UpdateAsync there is no access to the data.");
                throw new ValidationException("No access data.", resourceProvider.Get("NoAccessData"));
            }

            var user = await repo.GetByIdAsync(userDto.Id);

            if (user == null)
            {
                Logger.LogWarning($"UserService.UpdateAsync the user was not found.");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            mapper.MapFromDto(userDto, destination: user);
            await DataContextManager.SaveAsync();

            Logger.LogInformation($"UserService.UpdateAsync completed");
            return userDto;
        }
    }
}
