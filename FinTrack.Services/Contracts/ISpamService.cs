

namespace FinTrack.Services.Contracts
{
    public interface ISpamService
    {
        Task Start(string text);
        Task<List<string>> GetSpam();
    }
}
