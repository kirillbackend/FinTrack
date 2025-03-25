
namespace FinTrack.Services.Contracts
{
    public interface IHashService
    {
        Task<string> CreateHashPassword(string password);
        Task<bool> VerifyHashedPassword(string hashPassword, string password);
    }
}
