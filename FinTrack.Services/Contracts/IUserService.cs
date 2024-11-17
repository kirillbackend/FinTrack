using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetByEmail(string email);
        Task<UserDto> GetById(int id);
        Task<IEnumerable<UserDto>> GetUsers();
        Task Delete(int id);
        Task<UserDto> Update(UserDto userDto);
        Task AddUser(UserDto userDto);
    }
}
