using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetAsync(Guid id);
        Task<IEnumerable<CategoryDto>> GetAsync();
        Task AddAsync(CategoryDto currencyDto);
        Task DeleteAsync(Guid id);
        Task<CategoryDto> UpdateAsync(CategoryDto currencyDto);
    }
}
