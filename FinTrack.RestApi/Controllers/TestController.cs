using FinTrack.Services.Contracts;
using FinTrack.Services.Kafka.Contracts;
using FinTrack.Services.Wrappers.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : AbstractController
    {
        private readonly IFixerAPIWrapper _fixerAPIWrapper;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyExchangeKafkaProducer _kafkaProducer;

        public TestController(
            ILogger<TestController> logger
            , IFixerAPIWrapper fixerAPIWrapper
            , ICurrencyService currencyService
            , ICurrencyExchangeKafkaProducer kafkaProducer
            )
            : base(logger)
        {
            _fixerAPIWrapper = fixerAPIWrapper;
            _currencyService = currencyService;
            _kafkaProducer = kafkaProducer;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(string testData)
        {
            try
            {
                Logger.LogInformation("TestController.Index started");
                await _kafkaProducer.ProduceAsync("fintrackcurrencyexchanger-topic", new Confluent.Kafka.Message<string, string>
                {
                    Key = DateTime.Now.ToString(),
                    Value = testData
                });

                Logger.LogInformation("TestController.Index completed");

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogWarning("TestController.Index error!");
                return BadRequest(ex.Message);
            }
        }
    }
}
