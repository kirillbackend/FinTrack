
namespace FinTrack.Services.Facades.Contracts
{
    public interface IUserFacade
    {
        Task UpdatePassword(string oldPassword, string newPassword);
    }
}
