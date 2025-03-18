using FinTrack.Model;
using FinTrack.Services.Dtos;
using System.Security.Claims;

namespace FinTrack.Services.Contracts
{
    public interface IAuthService
    {
        Task SignUpAsync(UserDto userDto);
        Task<string> CreateToken(IEnumerable<Claim> authClaims);
        Task<string> GenerateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<IEnumerable<Claim>> CreateClaims(UserDto userDto);
        Task<AuthToken> GetTokenByRefreshTokenAsync(string refreshToken);
        Task UpdateRefreshTokenAsync(AuthToken authToken);
        Task AddAuthTokenAsync(AuthToken authToken);
    }
}
