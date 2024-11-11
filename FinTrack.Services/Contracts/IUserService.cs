using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetUserByEmail(string email);
    }
}
