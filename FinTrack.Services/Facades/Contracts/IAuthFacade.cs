using FinTrack.Services.Dtos;

namespace FinTrack.Services.Facades.Contracts
{
    public interface IAuthFacade
    {
        Task SingUp(SignUpDto singUpDto);
        Task<(string, string)> LogIn(LoginDto loginDto);
        Task<(string, string)> RefreshToken(string refreshToken);
    }
}
