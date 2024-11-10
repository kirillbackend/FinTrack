using Microsoft.AspNetCore.Mvc;
using FinTrack.Services.Exceptions;

namespace FinTrack.RestApi.Controllers
{
    public class AbstractController : ControllerBase
    {
        public ILogger Logger { get; set; }

        public AbstractController(ILogger logger)
        {
            Logger = logger;
        }

        protected BadRequestObjectResult BadRequest(ValidationException exception)
        {
            return new BadRequestObjectResult(ModelState)
            {
                Value = new
                {
                    message = exception.Message,
                    uiMessage = exception.UIMessage,
                    source = exception.ValidationSource
                }
            };
        }
    }
}
