using FinTrack.Model;
using FinTrack.Services.Dtos;
using System.Security.Claims;

namespace FinTrack.Services.Contracts
{
    public interface IAuthService
    {
        Task SignUp(UserDto userDto);
        Task<string> CreateToken(IEnumerable<Claim> authClaims);
        Task<string> GenerateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<IEnumerable<Claim>> CreateClaims(UserDto userDto);
        Task<AuthToken> GetTokenByRefreshToken(string refreshToken);
        Task UpdateRefreshToken(AuthToken authToken);
        Task AddAuthToken(AuthToken authToken);
    }
}
