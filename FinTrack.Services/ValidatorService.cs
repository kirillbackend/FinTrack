using FinTrack.Model;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Exceptions;

namespace FinTrack.Services
{
    public class ValidatorService : IValidatorService
    {
        public ValidatorService()
        {
        }

        public async Task CategoryValidate(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrEmpty(category.Name))
                throw new ArgumentNullException(nameof(category.Name));
        }

        public async Task CurrencyValidate(Currency currency)
        {
            if (currency == null)
            {
                throw new ValidationException("Currency was not found.");
            }
        }

        public async Task FinanceValidate(Guid userContextId, Finance finance)
        {
            if (finance == null)
            {
                throw new ValidationException("Finance is null.", nameof(Finance));
            }

            if (userContextId != finance.UserId)
            {
                throw new ValidationException("UserContextId not equal Finance.UserId", nameof(Finance.UserId));
            }
        }

        public async Task HashValidate(string hashPassword)
        {
            if (string.IsNullOrEmpty(hashPassword))
            {
                throw new ArgumentNullException("Hash");
            }
        }

        public async Task PasswordValidate(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password");
            }
        }
    }
}
