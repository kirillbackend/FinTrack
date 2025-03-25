using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Services.Context;
using FinTrack.Services.Context.Contracts;
using FinTrack.Services.Contracts;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Facades.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services.Facades
{
    public class UserFacade : AbstractFacade, IUserFacade
    {
        private readonly IHashService _hashService;
        private readonly IContextLocator _contextLocator;

        public UserFacade(ILogger<UserFacade> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager
            , IHashService hashService, IContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager)
        {
            _hashService = hashService;
            _contextLocator = contextLocator;
        }

        public async Task UpdatePasswordAsync(string oldPassword, string newPassword)
        {
            Logger.LogInformation("UserFacade.UpdatePasswordAsync started)");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var userContext = _contextLocator.Get<UserContext>();
            var user = await repo.GetByIdAsync(userContext.Id);

            if (user == null)
            {
                Logger.LogWarning($"UserFacade.UpdatePasswordAsync a user was not found");
                throw new ValidationException("User was not found.");
            }

            var isCorretPassWord = await _hashService.VerifyHashedPassword(user.Password, oldPassword);

            if (!isCorretPassWord)
            {
                Logger.LogWarning("UserFacade.UpdatePasswordAsync completed. Invalid password");
                throw new ValidationException("Invalid password");
            }
            else
            {
                var newHashPassword = await _hashService.CreateHashPassword(newPassword);
                user.Password = newHashPassword;
                await DataContextManager.SaveAsync();
            }
        }
    }
}
