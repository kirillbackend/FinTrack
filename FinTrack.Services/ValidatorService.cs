using FinTrack.Model;
using FinTrack.Services.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Services.Exceptions;

namespace FinTrack.Services
{
    public class ValidatorService : IValidatorService
    {
        private readonly ILogger _logger;

        public ValidatorService(ILogger<ValidatorService> logger)
        {
            _logger = logger;
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
    }
}
