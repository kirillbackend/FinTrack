using FinTrack.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Dtos;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController : AbstractController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyService currencyService)
            : base(logger)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.Get({id}) started");

                var currency = await _currencyService.GetCurrencyById(id);

                Logger.LogInformation($"CurrencyController.Get({id}) completed");
                return Ok(currency);
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.Get({id}) completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Logger.LogInformation($"CurrencyController.Get started");

                var currency = await _currencyService.GetCurrencies();

                Logger.LogInformation($"CurrencyController.Get completed");
                return Ok(currency);
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.Get completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CurrencyDto currencyDto)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.Get started");

                var currency = await _currencyService.GetCurrencies();

                Logger.LogInformation($"CurrencyController.Get completed");
                return Ok(currency);
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.Get completed; invalid request");
                return BadRequest(ex);
            }
        }
    }
}
