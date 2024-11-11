using FinTrack.Services.Dtos;

namespace FinTrack.Services.Facades.Contracts
{
    public interface IAuthFacade
    {
        Task SingUp(SignUpDto singUpDto);
        Task<string> LogIn(LoginDto loginDto);
    }
}
