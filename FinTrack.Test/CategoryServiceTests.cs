using FinTrack.Data.Contracts;
using FinTrack.Model;
using FinTrack.Services;
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

        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _logMock = new Mock<ILogger<CategoryService>>();
            _iMapperFactoryMock = new Mock<IMapperFactory>();
            _iDataContextManagerMock = new Mock<IDataContextManager>();

            _categoryService = new CategoryService(_logMock.Object, _iMapperFactoryMock.Object, _iDataContextManagerMock.Object);
        }

        public ArgumentNullException Exception { get; private set; }

        [Fact]
        public async Task CategoryValidate_Exsists_ReturnsZeroErrors()
        {
            //Arrange
            var category = new Category() { Id = new Guid(), Name = "Pay"};

            //Act
            var exception = await Record.ExceptionAsync(() => _categoryService.CategoryValidate(category));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task CategoryValidate_CategoryIsNull_ReturnArgumentNullException()
        {
            //Arrange
            Category category = null;
            Exception = new ArgumentNullException(nameof(Category));

            //Act
            var exception = await Record.ExceptionAsync(() => _categoryService.CategoryValidate(category));

            //Act
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task CategoryValidate_NameIsNull_ReturnArgumentNullException()
        {
            //Arrange
            var category = new Category() { Id = new Guid()};
            var expectedParamName = nameof(category.Name);

            //Act
            var exception = await Record.ExceptionAsync(() => _categoryService.CategoryValidate(category));

            //Assert
            var argumentNullEx = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(expectedParamName, argumentNullEx.ParamName);
        }
    }
}
