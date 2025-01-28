using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Context;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinTrack.Services
{
    public class AuthService : AbstractService, IAuthService
    {
        private readonly FinTrackServiceSettings _settings;

        public AuthService(ILogger<AuthService> logger, IMapperFactory mapperFactory, IConfiguration configuration
            , IDataContextManager dataContextManager, LocalizationContextLocator localization, ContextLocator contextLocator
            , FinTrackServiceSettings settings)
            : base(logger, mapperFactory, dataContextManager, localization, contextLocator)
        {
            _settings = settings;
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

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Auth.Secret));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: authClaims,
                    expires: DateTime.Now.AddMinutes(_settings.Auth.TokenExpireMinutes),
                    signingCredentials: signinCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            Logger.LogInformation("AuthService.CreateToken completed");
            return tokenString;
        }

        public async Task<string> CreateRefreshToken()
        {
            var randomNumber = new byte[_settings.Auth.RefreshTokenNumber];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber);
        }

        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Auth.Secret));
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:5001",
                ValidAudience = "https://localhost:5001",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Auth.Secret))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
