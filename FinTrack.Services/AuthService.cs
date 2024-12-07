using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Context;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrack.Services
{
    public class AuthService : AbstractService, IAuthService
    {
        private readonly FinTrackServiceSettings _dailyPlannerServiceSettings;

        public AuthService(ILogger<AuthService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, LocalizationContextLocator localization, ContextLocator contextLocator
            , FinTrackServiceSettings dailyPlannerServiceSettings)
            : base(logger, mapperFactory, dataContextManager, localization, contextLocator)
        {
            _dailyPlannerServiceSettings = dailyPlannerServiceSettings;
        }

        public async Task SignUp(UserDto signUpDto)
        {
            Logger.LogInformation("AuthService.SignUp started");

            var repo = DataContextManager.CreateRepository<IUserRepository>();
            var signUpMapper = MapperFactory.GetMapper<IUserMapper>();

            var user = signUpMapper.MapFromDto(signUpDto);

            repo.Add(user);

            Logger.LogInformation("AuthService.SignUp completed");
        }

        public async Task<string> CreateToken(UserDto userDto)
        {
            Logger.LogInformation("AuthService.CreateToken started");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.Role, userDto.UserRole.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_dailyPlannerServiceSettings.Auth.Secret));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: authClaims,
                    expires: DateTime.Now.AddMinutes(_dailyPlannerServiceSettings.Auth.TokenExpireMinutes),
                    signingCredentials: signinCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            Logger.LogInformation("AuthService.CreateToken completed");
            return tokenString;
        }
    }
}
