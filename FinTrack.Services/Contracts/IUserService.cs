using FinTrack.Model;
using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetByEmail(string email);
        Task<UserDto> GetById(Guid id);
        Task<IEnumerable<UserDto>> GetUsers();
        Task Delete(Guid id);
        Task<UserDto> Update(UserDto userDto);
        Task AddUser(UserDto userDto);
    }
}
