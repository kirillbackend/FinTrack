using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : AbstractController
    {
        public TestController(ILogger<TestController> logger)
            : base(logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Index(string testData)
        {
            try
            {
                Logger.LogInformation("TestController.Index started");

                var result = "Profit" + testData + '!';

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
