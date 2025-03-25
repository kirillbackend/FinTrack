using FinTrack.Localization;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Facades.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Mappers.Contracts;
using FinTrack.Data.Contracts;
using FinTrack.Services.Context;
using ServiceStack;
using FinTrack.Model;

namespace FinTrack.Services.Facades
{
    public class AuthFacade : AbstractFacade, IAuthFacade
    {
        private readonly IAuthService _authService;
        private readonly IHashService _hashService;
        private readonly IUserService _userService;

        public AuthFacade(ILogger<AuthFacade> logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager
            , IAuthService authService, IHashService hashService, IUserService userService
            , LocalizationContextLocator localizationContext, ContextLocator contextLocator)
            : base(logger, mapperFactory, dataContextManager, localizationContext, contextLocator)
        {
            _authService = authService;
            _hashService = hashService;
            _userService = userService;
        }

        public async Task SingUpAsync(SignUpDto singUpDto)
        {
            Logger.LogInformation("AuthFacade.SingUpAsync started");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var userDto = await _userService.GetByEmailAsync(singUpDto.Email);

            if (userDto != null)
            {
                Logger.LogWarning($"AuthFacade.SingUpAsync a userDto with that name has already been registered. Username : {singUpDto.Email}");
                throw new ValidationException("User has already been registered.", resourceProvider.Get("UserRegistered"));
            }

            userDto = new UserDto()
            {
                Email = singUpDto.Email,
                Password = await _hashService.CreateHashPassword(singUpDto.Password),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _authService.SignUpAsync(userDto);

            Logger.LogInformation("AuthFacade.SingUpAsync completed");
        }


        public async Task<(string, string)> LogInAsync(LoginDto loginDto)
        {
            Logger.LogInformation("AuthFacade.LogInAsync started");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var user = await _userService.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                Logger.LogWarning($"AuthFacade.SingUpAsync a userDto with that name has already been registered. Username : {loginDto.Email}");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            var isCorretPassWord = await _hashService.VerifyHashedPassword(user.Password, loginDto.Password);

            if (!isCorretPassWord)
            {
                Logger.LogWarning("AuthFacade.LogInAsync completed. Invalid password");
                throw new ValidationException("Invalid password", resourceProvider.Get("InvalidPassword"));
            }

            var claims = await _authService.CreateClaims(user);
            var token = await _authService.CreateToken(claims);

            var authToken = new AuthToken()
            {
                RefreshToken = await _authService.GenerateRefreshToken(),
                RefreshTokenExpireTime = DateTime.UtcNow.AddYears(1),
                UserId = user.Id,
            };

            await _authService.AddAuthTokenAsync(authToken);

            Logger.LogInformation("AuthFacade.LogInAsync completed");
            return (token, authToken.RefreshToken);
        }

        public async Task<(string, string)> RefreshTokenAsync(string refreshToken)
        {
            Logger.LogInformation("AuthFacade.RefreshTokenAsync started");

            var resourceProvider = LocalizationContext.GetContext<LocaleContext>().ResourceProvider;
            var authToken = await _authService.GetTokenByRefreshTokenAsync(refreshToken);

            if (authToken == null)
            {
                Logger.LogWarning($"AuthFacade.RefreshTokenAsync token was not found.");
                throw new ValidationException("Token was not found.", resourceProvider.Get("TokenWasNotFound"));
            }

            authToken.RefreshToken = await _authService.GenerateRefreshToken();
            var user = await _userService.GetByIdAsync(authToken.UserId);

            if (user == null)
            {
                Logger.LogWarning($"AuthFacade.SingUpAsync a userDto with that name has already been registered.");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            var claims = await _authService.CreateClaims(user);
            var token = await _authService.CreateToken(claims);
            await _authService.UpdateRefreshTokenAsync(authToken);

            Logger.LogInformation("AuthFacade.RefreshTokenAsync complited");

            return (token, refreshToken);
        }
    }
}
