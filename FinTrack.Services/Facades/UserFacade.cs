using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Context;
using FinTrack.Services.Contracts;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Facades.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services.Facades
{
    public class UserFacade : AbstractFacade, IUserFacade
    {
        private readonly IUserService _userService;
        private readonly IHashService _hashService;

        public UserFacade(ILogger<UserFacade> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager
            , LocalizationContextLocator localizationContext, ContextLocator contextLocator, IUserService userService, IHashService hashService)
            : base(logger, mapperFactory, dataContextManager, localizationContext, contextLocator)
        {
            _userService = userService;
            _hashService = hashService;
        }

        public async Task UpdatePasswordAsync(string oldPassword, string newPassword)
        {
            Logger.LogInformation("UserFacade.UpdatePasswordAsync started)");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var user = await repo.GetByIdAsync(UserContext.Id);

            if (user == null)
            {
                Logger.LogWarning($"UserFacade.UpdatePasswordAsync a user was not found");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            var isCorretPassWord = await _hashService.VerifyHashedPassword(user.Password, oldPassword);

            if (!isCorretPassWord)
            {
                Logger.LogWarning("UserFacade.UpdatePasswordAsync completed. Invalid password");
                throw new ValidationException("Invalid password", resourceProvider.Get("InvalidPassword"));
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
