using FinTrack.Localization;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Facades.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;

namespace FinTrack.Services.Facades
{
    public class AuthFacade : AbstractFacade, IAuthFacade
    {
        private readonly IAuthService _authService;
        private readonly IHashService _hashService;
        private readonly IUserService _userService;
        private readonly LocalizationContextLocator _contextLocator;

        public AuthFacade(ILogger<AuthFacade> logger, IAuthService authService, IHashService hashService, IUserService userService
            , LocalizationContextLocator contextLocator)
            : base(logger)
        {
            _authService = authService;
            _hashService = hashService;
            _userService = userService;
            _contextLocator = contextLocator;
        }

        public async Task SingUp(SignUpDto singUpDto)
        {
            Logger.LogInformation("AuthFacade.SingUp started");

            var resourceProvider = _contextLocator.GetContext<LocaleContext>().ResourceProvider;
            var userDto = await _userService.GetByEmail(singUpDto.Email);

            if (userDto != null)
            {
                Logger.LogWarning($"AuthFacade.SingUp a userDto with that name has already been registered. Username : {singUpDto.Email}");
                throw new ValidationException("User has already been registered.", resourceProvider.Get("UserRegistered"));
            }

            userDto = new UserDto()
            {
                Email = singUpDto.Email,
                Password = await _hashService.CreateHashPassword(singUpDto.Password),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _authService.SignUp(userDto);

            Logger.LogInformation("AuthFacade.SingUp completed");
        }


        public async Task<string> LogIn(LoginDto loginDto)
        {
            Logger.LogInformation("AuthFacade.LogIn started");

            var resourceProvider = _contextLocator.GetContext<LocaleContext>().ResourceProvider;
            var user = await _userService.GetByEmail(loginDto.Email);

            if (user == null)
            {
                Logger.LogWarning($"AuthFacade.SingUp a userDto with that name has already been registered. Username : {loginDto.Email}");
                throw new ValidationException("User was not found.", resourceProvider.Get("UserWasNotFound"));
            }

            var isCorretPassWord = await _hashService.VerifyHashedPassword(user.Password, loginDto.Password);

            if (!isCorretPassWord)
            {
                Logger.LogWarning("AuthFacade.LogIn completed. Invalid password");
                throw new ValidationException("Invalid password", resourceProvider.Get("InvalidPassword"));
            }

            var token = await _authService.CreateToken(user);

            Logger.LogInformation("AuthFacade.LogIn completed");
            return token;
        }
    }
}
