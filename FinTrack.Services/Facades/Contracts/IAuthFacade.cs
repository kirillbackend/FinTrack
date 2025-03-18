using FinTrack.Services.Dtos;

namespace FinTrack.Services.Facades.Contracts
{
    public interface IAuthFacade
    {
        Task SingUpAsync(SignUpDto singUpDto);
        Task<(string, string)> LogInAsync(LoginDto loginDto);
        Task<(string, string)> RefreshTokenAsync(string refreshToken);
    }
}
