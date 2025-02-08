using FinTrack.Services.Contracts;
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
        private readonly ISpamService _spamService;
        private readonly ICurrencyService _currencyService; 

        public TestController(
            ILogger<TestController> logger
            , IFixerAPIWrapper fixerAPIWrapper
            , ISpamService spamService
            , ICurrencyService currencyService)
            : base(logger)
        {
            _fixerAPIWrapper = fixerAPIWrapper;
            _spamService = spamService;
            _currencyService = currencyService;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(string testData)
        {
            try
            {
                Logger.LogInformation("TestController.Index started");

               await _currencyService.ProduceAsync(testData);
               var result =  await _currencyService.StartAsync();

                Logger.LogInformation("TestController.Index completed");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("TestController.Index error!");
                return BadRequest(ex.Message);
            }
        }
    }
}
