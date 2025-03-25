
namespace FinTrack.Services.Facades.Contracts
{
    public interface IUserFacade
    {
        Task UpdatePasswordAsync(string oldPassword, string newPassword);
    }
}
