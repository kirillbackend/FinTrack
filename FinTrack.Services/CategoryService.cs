using FinTrack.Data.Contracts;
using FinTrack.Data.Repositories.Contracts;
using FinTrack.Model;
using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services
{
    public class CategoryService : AbstractService, ICategoryService
    {
        private readonly IValidatorService _validatorService;

        public CategoryService(ILogger<CategoryService> logger, IMapperFactory mapperFactory
            , IDataContextManager dataContextManager, IValidatorService validatorService)
            : base(logger, mapperFactory, dataContextManager)
        {
            _validatorService = validatorService;
        }

        public async Task AddAsync(CategoryDto categoryDto)
        {
            var categoryRepository = DataContextManager.CreateRepository<ICategoryRepositoty>();
            var categotyMapper = MapperFactory.GetMapper<ICategoryMapper>();

            var categoty = categotyMapper.MapFromDto(categoryDto);
            await _validatorService.CategoryValidate(categoty);
            await categoryRepository.AddAssync(categoty);
        }

        public async Task DeleteAsync(Guid id)
        {
            var categoryRepository = DataContextManager.CreateRepository<ICategoryRepositoty>();
            await categoryRepository.DeleteAsync(id);
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var categoryRepository = DataContextManager.CreateRepository<ICategoryRepositoty>();
            var categotyMapper = MapperFactory.GetMapper<ICategoryMapper>();

            var categoty = await categoryRepository.GetAsync(id);
            await _validatorService.CategoryValidate(categoty);
            var categotyDto = categotyMapper.MapToDto(categoty);

            return categotyDto;
        }

        public async Task<IEnumerable<CategoryDto>> GetAsync()
        {
            var categoryRepository = DataContextManager.CreateRepository<ICategoryRepositoty>();
            var categotyMapper = MapperFactory.GetMapper<ICategoryMapper>();

            var categories = await categoryRepository.GetAsync();
            await CategoriesValidate(categories);
            var categoriesDto = categotyMapper.MapCollectionToDto(categories);

            return categoriesDto;
        }

        public async Task<CategoryDto> UpdateAsync(CategoryDto categoryDto)
        {
            var categoryRepository = DataContextManager.CreateRepository<ICategoryRepositoty>();
            var categotyMapper = MapperFactory.GetMapper<ICategoryMapper>();

            var categoty = await categoryRepository.GetAsync(categoryDto.Id);
            await _validatorService.CategoryValidate(categoty);
            categotyMapper.MapFromDto(categoryDto, destination: categoty);
            categoty.UpdatedDate = DateTime.UtcNow;
            await DataContextManager.SaveAsync();

            return categoryDto;
        }


        #region private metods
        private async Task CategoriesValidate(IEnumerable<Category> categories)
        {
            foreach (var category in categories)
            {
                await _validatorService.CategoryValidate(category);
            } 
        }

        #endregion
    }
}
