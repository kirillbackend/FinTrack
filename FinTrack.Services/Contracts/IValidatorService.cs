using FinTrack.Model;

namespace FinTrack.Services.Contracts
{
    public interface IValidatorService
    {
        Task CategoryValidate(Category category);
        Task CurrencyValidate(Currency currency);
        Task FinanceValidate(Guid userContextId, Finance finance);
        Task HashValidate(string hashPassword);
        Task PasswordValidate(string password);
        Task UserIdValidate(Guid userId, Guid userContextId);
        Task UserValidate(User user);
    }
}
