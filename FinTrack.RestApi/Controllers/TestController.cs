using FinTrack.Services.Wrappers;
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
        public TestController(ILogger<TestController> logger, IFixerAPIWrapper fixerAPIWrapper)
            : base(logger)
        {
            _fixerAPIWrapper = fixerAPIWrapper;
        }


        [AllowAnonymous]
        [HttpPost, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string testData)
        {
            try
            {
                Logger.LogInformation("TestController.Index started");

                var result = "Profit" + testData + '!';

                await _fixerAPIWrapper.LatestCurrency("RUB", new List<string> { "EUR", "USD" });

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
