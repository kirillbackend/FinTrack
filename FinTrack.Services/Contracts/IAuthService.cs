using FinTrack.Services.Dtos;
using System.Security.Claims;

namespace FinTrack.Services.Contracts
{
    public interface IAuthService
    {
        Task SignUp(UserDto userDto);
        Task<string> CreateToken(UserDto userDto);
        Task<string> CreateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    }
}
