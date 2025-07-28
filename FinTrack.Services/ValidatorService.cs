using FinTrack.Model;
using FinTrack.Services.Contracts;

namespace FinTrack.Services
{
    public class ValidatorService : IValidatorService
    {
        public async Task CategoryValidate(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrEmpty(category.Name))
                throw new ArgumentNullException(nameof(category.Name));
        }
    }
}
