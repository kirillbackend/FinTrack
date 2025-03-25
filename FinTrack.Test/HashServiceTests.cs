using FinTrack.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinTrack.Test
{
    public class HashServiceTests
    {
        private readonly Mock<ILogger<HashService>> _loggerMock;
        private readonly HashService _hashService;

        public HashServiceTests()
        {
            _loggerMock = new Mock<ILogger<HashService>>();
            _hashService = new HashService(_loggerMock.Object);
        }   


        [Fact]
        public async Task CreateHashPassword_RandomTestPasswopd_ReturnsNotNull()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();

            //Act
            var result = await _hashService.CreateHashPassword(testPasswopd);

            //Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task CreateHashPassword_RandomTestPasswopd_ReturnsNotEmpty()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();

            //Act
            var result = await _hashService.CreateHashPassword(testPasswopd);

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CreateHashPassword_TestPasswopdIsNull_ReturnsThrowArgumentNullException()
        {
            //Arrange
            string testPasswopd = null;

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _hashService.CreateHashPassword(testPasswopd));
        }

        [Fact]
        public async Task CreateHashPassword_RandomTestPasswopd_RetutnsContainsSeparator()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();
            var separator = ':';

            //Act
            var result = await _hashService.CreateHashPassword(testPasswopd);

            //Assert
            Assert.Contains(separator, result);
        }

        [Fact]
        public async Task CreateHashPassword_RandomTestPasswopd_ReturnsSaltConsistentFormat()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();
            var separator = ':';

            //Act
            var result = await _hashService.CreateHashPassword(testPasswopd);
            var parts = result.Split(separator);

            //Assert
            Assert.True(IsHexString(parts[0]));
        }

        [Fact]
        public async Task CreateHashPassword_RandomTestPasswopd_ReturnsHashConsistentFormat()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();
            var separator = ':';

            //Act
            var result = await _hashService.CreateHashPassword(testPasswopd);
            var parts = result.Split(separator);

            //Assert
            Assert.True(IsHexString(parts[1]));
        }

        [Fact]
        public async Task CreateHashPassword_RandomTestPasswopd_ReturnsDifferentHashForSamePassword()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();
            var separator = ':';

            //Act
            var firstHashPassword = await _hashService.CreateHashPassword(testPasswopd);
            var firstPasswordParts = firstHashPassword.Split(separator);

            var secondHashPassword = await _hashService.CreateHashPassword(testPasswopd);
            var secondPasswordParts = secondHashPassword.Split(separator);


            //Assert
            Assert.NotEqual(firstPasswordParts[0], secondPasswordParts[0]);
        }

        [Fact]
        public async Task CreateHashPassword_RandomTestPasswopds_ReturnsDifferentHashForSamePassword()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();
            var anotherTestPasswopd = "anotherTestPasswopd" + Guid.NewGuid().ToString();
            var separator = ':';

            //Act
            var firstHashPassword = await _hashService.CreateHashPassword(testPasswopd);
            var firstPasswordParts = firstHashPassword.Split(separator);

            var secondHashPassword = await _hashService.CreateHashPassword(anotherTestPasswopd);
            var secondPasswordParts = secondHashPassword.Split(separator);


            //Assert
            Assert.NotEqual(firstPasswordParts[0], secondPasswordParts[0]);
        }

        [Fact]
        public async Task VerifyHashedPassword_RandomTestPasswopdEqualHashedPassword_ReturnsTrue()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();

            //Act
            var hashPassword = await _hashService.CreateHashPassword(testPasswopd);
            var result = await _hashService.VerifyHashedPassword(hashPassword, testPasswopd);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyHashedPassword_RandomTestPasswopdNotEqualHashedPassword_ReturnsFalse()
        {
            //Arrange
            var testPasswopd = "test" + Guid.NewGuid().ToString();
            var anotherTestPasswopd = "anotherTestPasswopd" + Guid.NewGuid().ToString();

            //Act
            var anotherHashPassword = await _hashService.CreateHashPassword(anotherTestPasswopd);
            var result = await _hashService.VerifyHashedPassword(anotherHashPassword, testPasswopd);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task VerifyHashedPassword_VerifyPasswordIsNull_ReturnsFalse()
        {
            //Arrage
            var testPasswopd = "test" + Guid.NewGuid().ToString();

            //Act
            var hashTestPasswopd = await _hashService.CreateHashPassword(testPasswopd);
            var result = await _hashService.VerifyHashedPassword(hashTestPasswopd, null);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task VerifyHashedPassword_HashPasswordIsNull_ReturnsThrowArgumentNullException()
        {
            //Arrage
            var testPasswopd = "test" + Guid.NewGuid().ToString();

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _hashService.VerifyHashedPassword(null, testPasswopd));
        }

        private bool IsHexString(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"\A\b[0-9a-fA-F]+\b\Z");
        }
    }
}