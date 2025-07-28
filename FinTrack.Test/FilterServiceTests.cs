using FinTrack.Enums;
using FinTrack.Model;
using FinTrack.Services;

namespace FinTrack.Test
{
    public class FilterServiceTests
    {
        private readonly FilterService _filterService;

        public FilterServiceTests()
        {
            _filterService = new FilterService();
        }

        [Fact]
        public async Task CreateReportFilter_ReportTypeDay_ReturnsDayPredicate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reportType = ReportType.Day;

            var testData = new List<Finance>
            {
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today },
                new Finance { UserId = userId, IsDeleted = true, CreatedDate = DateTime.Today },
                new Finance { UserId = Guid.NewGuid(), IsDeleted = false, CreatedDate = DateTime.Today }
            }.AsQueryable();

            // Act
            var predicate = await _filterService.CreateReportFilter(userId, reportType);
            var filtered = testData.Where(predicate.Compile()).ToList();

            // Assert
            Assert.Equal(DateTime.Today, filtered[0].CreatedDate.Date);
        }

        [Fact]
        public async Task CreateReportFilter_ReportTypeDay_ReturnsNotEmptyFilter()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reportType = ReportType.Day;

            var testData = new List<Finance>
            {
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today },
                new Finance { UserId = userId, IsDeleted = true, CreatedDate = DateTime.Today },
                new Finance { UserId = Guid.NewGuid(), IsDeleted = false, CreatedDate = DateTime.Today }
            }.AsQueryable();

            // Act
            var predicate = await _filterService.CreateReportFilter(userId, reportType);
            var filtered = testData.Where(predicate.Compile()).ToList();

            // Assert
            Assert.NotEmpty(filtered);
        }

        [Fact]
        public async Task CreateReportFilter_ReportTypeDay_ReturnsIsDeleteFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reportType = ReportType.Day;

            var testData = new List<Finance>
            {
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today },
                new Finance { UserId = userId, IsDeleted = true, CreatedDate = DateTime.Today },
                new Finance { UserId = Guid.NewGuid(), IsDeleted = false, CreatedDate = DateTime.Today }
            }.AsQueryable();

            // Act
            var predicate = await _filterService.CreateReportFilter(userId, reportType);
            var filtered = testData.Where(predicate.Compile()).ToList();

            // Assert
            Assert.False(filtered[0].IsDeleted);
        }

        [Fact]
        public async Task CreateReportFilter_ReportTypeDayAndNewUserId_ReturnsUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reportType = ReportType.Week;

            var testData = new List<Finance>
            {
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today },
                new Finance { UserId = userId, IsDeleted = true, CreatedDate = DateTime.Today },
                new Finance { UserId = Guid.NewGuid(), IsDeleted = false, CreatedDate = DateTime.Today }
            }.AsQueryable();

            // Act
            var predicate = await _filterService.CreateReportFilter(userId, reportType);
            var filtered = testData.Where(predicate.Compile()).ToList();

            // Assert
            Assert.Equal(userId, filtered[0].UserId);
        }

        [Fact]
        public async Task CreateReportFilter_ReportTypeWeek_ReturnsWeekRange()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var reportType = ReportType.Week;
            var count = 2;

            var testData = new List<Finance>
            {
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(15) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(13) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(3) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(6) },
                new Finance { UserId = userId, IsDeleted = true, CreatedDate = DateTime.Today },
                new Finance { UserId = Guid.NewGuid(), IsDeleted = false, CreatedDate = DateTime.Today }
            };

            // Act
            var predicate = await _filterService.CreateReportFilter(userId, reportType);
            var filtered = testData.Where(predicate.Compile()).ToList();

            // Assert
            Assert.Equal(count, filtered.Count());
        }

        [Fact]
        public async Task CreateReportFilter_ReportTypeYear_ReturnsYearRange()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var reportType = ReportType.Year;
            var count = 3;

            var testData = new List<Finance>
            {
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(366) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(364) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(3) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(6) },
                new Finance { UserId = userId, IsDeleted = true, CreatedDate = DateTime.Today },
                new Finance { UserId = Guid.NewGuid(), IsDeleted = false, CreatedDate = DateTime.Today }
            };

            //Act
            var predicate = await _filterService.CreateReportFilter(userId, reportType);
            var filtered = testData.Where(predicate.Compile()).ToList();

            //Assert
            Assert.Equal(count, filtered.Count());
        }

        [Fact]
        public async Task CreateReportFilter_ReportTypeAll_ReturnsAllCorectData()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var reportType = ReportType.All;
            var count = 4;

            var testData = new List<Finance>
            {
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(366) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(364) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(3) },
                new Finance { UserId = userId, IsDeleted = false, CreatedDate = DateTime.Today.AddDays(6) },
                new Finance { UserId = userId, IsDeleted = true, CreatedDate = DateTime.Today },
                new Finance { UserId = Guid.NewGuid(), IsDeleted = false, CreatedDate = DateTime.Today }
            };

            //Act
            var predicate = await _filterService.CreateReportFilter(userId, reportType);
            var filtered = testData.Where(predicate.Compile()).ToList();

            //Assert
            Assert.Equal(count, filtered.Count());
        }
    }
}
