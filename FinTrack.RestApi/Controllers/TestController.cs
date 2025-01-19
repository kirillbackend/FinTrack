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
        public TestController(ILogger<TestController> logger, IFixerAPIWrapper fixerAPIWrapper, ISpamService spamService)
            : base(logger)
        {
            _fixerAPIWrapper = fixerAPIWrapper;
            _spamService = spamService;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(string testData)
        {
            try
            {
                Logger.LogInformation("TestController.Index started");

                await _spamService.Start(testData);
                //var answer = await _spamService.GetSpam();

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
