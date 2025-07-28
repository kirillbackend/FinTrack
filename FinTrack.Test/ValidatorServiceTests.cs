using FinTrack.Model;
using FinTrack.Services;
using FinTrack.Services.Contracts;

namespace FinTrack.Test
{
    public class ValidatorServiceTests
    {
        private readonly IValidatorService _validatorService;

        public ValidatorServiceTests()
        {
            _validatorService = new ValidatorService();
        }


        public ArgumentNullException Exception { get; private set; }

        [Fact]
        public async Task CategoryValidate_Exsists_ReturnsZeroErrors()
        {
            //Arrange
            var category = new Category() { Id = new Guid(), Name = "Pay" };

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.CategoryValidate(category));

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
            var exception = await Record.ExceptionAsync(() => _validatorService.CategoryValidate(category));

            //Act
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task CategoryValidate_NameIsNull_ReturnArgumentNullException()
        {
            //Arrange
            var category = new Category() { Id = new Guid() };
            var expectedParamName = nameof(category.Name);

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.CategoryValidate(category));

            //Assert
            var argumentNullEx = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(expectedParamName, argumentNullEx.ParamName);
        }
    }
}
