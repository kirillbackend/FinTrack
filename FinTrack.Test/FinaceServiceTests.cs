using Moq;
using FinTrack.Services;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;
using FinTrack.Data.Contracts;
using FinTrack.Services.Context.Contracts;

namespace FinTrack.Test
{
    public class FinaceServiceTests
    {
        private readonly Mock<ILogger<FinanceService>> _logMock;
        private readonly Mock<IMapperFactory> _iMapperFactoryMock;
        private readonly Mock<IDataContextManager> _iDataContextManagerMock;
        private readonly Mock<IContextLocator> _iContextLocatorMock;

        private readonly FinanceService _financeService;

        public FinaceServiceTests()
        {
            _logMock = new Mock<ILogger<FinanceService>>();
            _iMapperFactoryMock = new Mock<IMapperFactory>();
            _iDataContextManagerMock = new Mock<IDataContextManager>();
            _iContextLocatorMock = new Mock<IContextLocator>();

            _financeService = new FinanceService(
                _logMock.Object, _iMapperFactoryMock.Object, 
                _iDataContextManagerMock.Object, _iContextLocatorMock.Object);
        }

        [Fact]
        public async Task AddCategory_Exists_ReturnsTrue()
        {
            //Arrange
            var financeId = new Guid();
            var categoryId = new Guid();

            //Act
            var exception = await Record.ExceptionAsync(() => _financeService.AddCategoryAsync(financeId, categoryId));

            //Assert
            Assert.Null(exception);
        }

        //[Fact]
        //public async Task AddCategory_FinanceAddCategory_ReturnsCategory()
        //{
        //    //Aggange 
        //    var financeId = new Guid();
        //    var categoryId = new Guid("BC725028-B46C-464F-B412-C2D9778E627E");

        //    //Act
        //    var actual = _financeService.AddCategoryAsync(financeId, categoryId);

        //    //Assert
        //    Assert.Equal(categoryId, actual.CategoryId);
        //}

    }
}
