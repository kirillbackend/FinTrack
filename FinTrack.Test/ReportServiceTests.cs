using FinTrack.Model;
using FinTrack.Services;
using FinTrack.Services.Contracts;

namespace FinTrack.Test
{
    public class ReportServiceTests
    {
        private readonly IReportService _reportService;

        public ReportServiceTests()
        {
            _reportService = new ReportService();
        }

        [Fact]
        public async Task CreateReport_Exsists_ReturnsNotError()
        {
            //Arrange
            var testFinance = new List<Finance>()
            {
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay1" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay2" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay3" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay4" } },
            };

            //Act
            var exception = await Record.ExceptionAsync(() => _reportService.CreateReport(testFinance));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task CreateReport_AddFiveRecord_Returns500Amount()
        {
            //Arrange
            var testFinance = new List<Finance>()
            {
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay1" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay2" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay3" } },
                new Finance() { Id = new Guid(), Amount = 100.10m, Category = new Category() { Id = new Guid(), IsDeleted = false, Name = "Pay4" } },
            };

            var expected = 500.50m;

            //Act
            var actual = await _reportService.CreateReport(testFinance);

            //Assert
            Assert.Equal(expected, actual.Amount);
        }

        [Fact]
        public async Task CreateReport_AddFiveCategory_ReturnsFiveCategory()
        {
            //Arrange 
            var testFinance = new List<Finance>()
            {
                new Finance() 
                {
                    Id = new Guid("a3d8f9b2-4c6e-4d55-b8f1-7e3a2b1c0d9f"), 
                    Amount = 100.10m, 
                    Category = new Category()
                    { 
                        Id = new Guid("a3d8f9b2-4c6e-4d55-b8f1-7e3a2b1c0d9f"), 
                        IsDeleted = false, 
                        Name = "Pay" } 
                },
                new Finance()
                { 
                    Id = new Guid("1f4e7a90-23b1-4e8c-9d2a-5f6b3c8d1e0a"), 
                    Amount = 100.10m, 
                    Category = new Category() 
                    { 
                        Id = new Guid("1f4e7a90-23b1-4e8c-9d2a-5f6b3c8d1e0a"), 
                        IsDeleted = false, 
                        Name = "Pay1" 
                    } 
                },
                new Finance() 
                { 
                    Id = new Guid("7c2e1d0f-8a3b-4567-8912-3c4d5e6f7a8b"), 
                    Amount = 100.10m, 
                    Category = new Category() 
                    { 
                        Id = new Guid("7c2e1d0f-8a3b-4567-8912-3c4d5e6f7a8b"), 
                        IsDeleted = false, 
                        Name = "Pay2" 
                    } 
                },
                new Finance() 
                { 
                    Id = new Guid("e5d4c3b2-a1f0-4987-6e5d-4c3b2a1f0e9d"), 
                    Amount = 100.10m, 
                    Category = new Category() 
                    { 
                        Id = new Guid("e5d4c3b2-a1f0-4987-6e5d-4c3b2a1f0e9d"), 
                        IsDeleted = false, 
                        Name = "Pay3" 
                    } 
                },
                new Finance() 
                { 
                    Id = new Guid("9b8a7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d"), 
                    Amount = 100.10m, 
                    Category = new Category() 
                    { 
                        Id = new Guid("9b8a7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d"), 
                        IsDeleted = false, 
                        Name = "Pay4" 
                    } 
                },
            };

            var expected = 5;

            //Act
            var actual = await _reportService.CreateReport(testFinance);

            //Assert
            Assert.Equal(expected, actual.Categories.Count());
        }

        [Fact]
        public async Task CreateReport_AddFiveCategory_RetusnsUniqueCategory()
        {
            //Arrange
            var id = new Guid();
            var testFinance = new List<Finance>()
            {
                new Finance()
                {
                    Id = new Guid("a3d8f9b2-4c6e-4d55-b8f1-7e3a2b1c0d9f"),
                    Amount = 100.10m,
                    Category = new Category()
                    {
                        Id = id,
                        IsDeleted = false,
                        Name = "Pay" }
                },
                new Finance()
                {
                    Id = new Guid("1f4e7a90-23b1-4e8c-9d2a-5f6b3c8d1e0a"),
                    Amount = 100.10m,
                    Category = new Category()
                    {
                        Id = id,
                        IsDeleted = false,
                        Name = "Pay1"
                    }
                },
                new Finance()
                {
                    Id = new Guid("7c2e1d0f-8a3b-4567-8912-3c4d5e6f7a8b"),
                    Amount = 100.10m,
                    Category = new Category()
                    {
                        Id = id,
                        IsDeleted = false,
                        Name = "Pay2"
                    }
                },
                new Finance()
                {
                    Id = new Guid("e5d4c3b2-a1f0-4987-6e5d-4c3b2a1f0e9d"),
                    Amount = 100.10m,
                    Category = new Category()
                    {
                        Id = new Guid("e5d4c3b2-a1f0-4987-6e5d-4c3b2a1f0e9d"),
                        IsDeleted = false,
                        Name = "Pay3"
                    }
                },
                new Finance()
                {
                    Id = new Guid("9b8a7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d"),
                    Amount = 100.10m,
                    Category = new Category()
                    {
                        Id = new Guid("9b8a7c6d-5e4f-3a2b-1c0d-9e8f7a6b5c4d"),
                        IsDeleted = false,
                        Name = "Pay4"
                    }
                },
            };

            var expected = 3;

            //Act
            var actual = await _reportService.CreateReport(testFinance);

            //Assert
            Assert.Equal(expected, actual.Categories.Count());
        }
    }
}
