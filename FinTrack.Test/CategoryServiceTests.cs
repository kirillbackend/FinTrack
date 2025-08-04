using FinTrack.Data.Contracts;
using FinTrack.Services;
using FinTrack.Services.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinTrack.Test
{
    public class CategoryServiceTests
    {
        private readonly Mock<ILogger<CategoryService>> _logMock;
        private readonly Mock<IMapperFactory> _iMapperFactoryMock;
        private readonly Mock<IDataContextManager> _iDataContextManagerMock;
        private readonly Mock<IValidatorService> _iValidatorServiceMock;

        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _logMock = new Mock<ILogger<CategoryService>>();
            _iMapperFactoryMock = new Mock<IMapperFactory>();
            _iDataContextManagerMock = new Mock<IDataContextManager>();
            _iValidatorServiceMock = new Mock<IValidatorService>();

            _categoryService = new CategoryService(_logMock.Object, _iMapperFactoryMock.Object, 
                _iDataContextManagerMock.Object, _iValidatorServiceMock.Object);
        }       
    }
}
