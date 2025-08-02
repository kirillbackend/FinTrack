using FinTrack.Model;

namespace FinTrack.Services.Contracts
{
    public interface IValidatorService
    {
        Task CategoryValidate(Category category);
        Task CurrencyValidate(Currency currency);
    }
}
