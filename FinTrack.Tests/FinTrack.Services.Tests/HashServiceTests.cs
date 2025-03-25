using Autofac;
using FinTrack.Localization;
using FinTrack.Localization.Conrtacts;
using FinTrack.Services;
using FinTrack.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace FinTrack.Tests.FinTrack.Services.Tests
{
    public class HashService
    {
        private IContainer _container;
        private IHashService _hashService;
        private IContextFactory _contextFactory;

        public HashService()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false)
               .Build();

            var builder = new ContainerBuilder();
            var settings = config.Get<FinTrackServiceSettings>();

           
            ContainerConfiguration.ResisterTypes(builder, settings);


            _container = builder.Build();
            _hashService = _container.Resolve<IHashService>();
            _contextFactory = _container.Resolve<IContextFactory>();

            var localizationContextLocator = _container.Resolve<LocalizationContextLocator>();
            var localeContext = _contextFactory.CreateLocaleContext("en");
            localizationContextLocator.AddContext(localeContext);
        }

        [Fact]
        public async Task CreateHashPassword_GeneratePassword123_A0FCBE9152B3FA32A352E0ECC2DAA5B1F8D28227E63348FFDF33C258C7B0E0ED()
        {
            //Arrange
            var passvord = "123";
            var hashPassword = "A0FCBE9152B3FA32A352E0ECC2DAA5B1F8D28227E63348FFDF33C258C7B0E0ED";

            //Act
            var actual = await _hashService.CreateHashPassword(passvord);

            //Assert
            Assert.Equal(hashPassword, actual.Split(':')[1]);
        }

        [Fact]
        public async Task VerifyHashedPassword_HashPasswordAndPassvord_ReturnTrue()
        {
            //Arrange
            var passvord = "123";
            var hashPassword = "32D3CF:A0FCBE9152B3FA32A352E0ECC2DAA5B1F8D28227E63348FFDF33C258C7B0E0ED";

            //Act
            var actual = await _hashService.VerifyHashedPassword(hashPassword, passvord);

            //Assert
            Assert.True(actual);
        }
    }
}
