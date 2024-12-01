using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IAuthService
    {
        Task SignUp(UserDto userDto);
        Task<string> CreateToken(UserDto userDto);
    }
}
