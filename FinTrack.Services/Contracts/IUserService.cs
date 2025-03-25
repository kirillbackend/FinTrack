using FinTrack.Model;
using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> GetByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetUsers();
        Task DeleteAsync(Guid id);
        Task<UserDto> UpdateAsync(UserDto userDto);
        Task AddUserAsync(UserDto userDto);
    }
}
