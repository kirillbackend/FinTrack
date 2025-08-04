using FinTrack.Model;
using FinTrack.Services;
using FinTrack.Services.Contracts;
using FinTrack.Services.Exceptions;

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
        public ValidationException ValidationException { get; private set; }

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

        [Fact]
        public async Task FinanceValidate_Exsists_ReturnsZeroErrors()
        {
            //Arrage
            var userContextId = new Guid();
            var financeUserId = new Guid();
            var finance = new Finance() { UserId = financeUserId };

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.FinanceValidate(userContextId, finance));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task FinanceValidate_FinanceNull_ReturnsRerrorValidationExceptionNameofFinance()
        {
            //Arrange
            var userContextId = new Guid();
            Finance finance = null;
            var expectedParamName = nameof(Finance);

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.FinanceValidate(userContextId, finance));

            //Assert
            var argumentValidationException = Assert.IsType<ValidationException>(exception);
            Assert.Equal(expectedParamName, argumentValidationException.ParamName);
        }

        [Fact]
        public async Task FinanceValidate_UserContextIdNotEqualFinanceUserId_ReturnsValidationExceptionNameofFinanceUserId()
        {
            //Arrange
            var userContextId = new Guid("3a9ef1d2-5b7e-4c3a-8d1f-2c6b9e8d7a1f");
            var financeUserId = new Guid();
            var finance = new Finance() { UserId = financeUserId };
            var exceptionParamName = nameof(Finance.UserId);

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.FinanceValidate(userContextId, finance));

            //Assert
            var argumentValidationException = Assert.IsType<ValidationException>(exception);
            Assert.Equal(exceptionParamName, argumentValidationException.ParamName);
        }

        [Fact]
        public async Task HashValidate_Exsists_ReturnsZeroExceptions()
        {
            //Arrange
            var hash = "12312";

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.HashValidate(hash));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task HashValidate_hashPasswordIsNull_Returns_ArgumentNullExceptionNameofHash()
        {
            //Arrange
            string hash = null;
            var exceptionParamName = "Hash";

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.HashValidate(hash));

            //Act
            var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(exceptionParamName, argumentNullException.ParamName);
        }

        [Fact]
        public async Task PasswordValidate_Exsists_ReturnsZeroException()
        {
            //Arrange
            var password = "123";

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.PasswordValidate(password));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task PasswordValidate_hashPasswordIsNull_Returns_ArgumentNullExceptionNameofPassword()
        {
            //Arrange
            string password = null;
            var exceptionParamName = "Password";

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.PasswordValidate(password));

            //Act
            var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal(exceptionParamName, argumentNullException.ParamName);
        }

        [Fact]
        public async Task UserIdValidate_Exsists_ReturnsZeroExceptions()
        {
            //Arrange
            var userId = new Guid("3a9ef1d2-5b7e-4c3a-8d1f-2c6b9e8d7a1f");
            var userContextId = new Guid("3a9ef1d2-5b7e-4c3a-8d1f-2c6b9e8d7a1f");

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.UserIdValidate(userId, userContextId));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task UserIdValidate_UserIdNotEqualUserContextId_ReturnsValidationExceptions()
        {
            //Arrange
            var userId = new Guid();
            var userContextId = new Guid("3a9ef1d2-5b7e-4c3a-8d1f-2c6b9e8d7a1f");
            var exceptionParamName = nameof(Guid);

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.UserIdValidate(userId, userContextId));

            //Assert
            var argumentValidationException = Assert.IsType<ValidationException>(exception);
            Assert.Equal(exceptionParamName, argumentValidationException.ParamName);
        }

        [Fact]
        public async Task UserValidate_Exsists_ReturnsZeroExceptions()
        {
            //Arrange
            var user = new User();  

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.UserValidate(user));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task UserValidate_UserIdNotEqualUserContextId_ReturnsValidationExceptions()
        {
            //Arrange
            User user = null;
            var exceptionParamName = nameof(User);

            //Act
            var exception = await Record.ExceptionAsync(() => _validatorService.UserValidate(user));

            //Assert
            var argumentValidationException = Assert.IsType<ValidationException>(exception);
            Assert.Equal(exceptionParamName, argumentValidationException.ParamName);
        }
    }
}
