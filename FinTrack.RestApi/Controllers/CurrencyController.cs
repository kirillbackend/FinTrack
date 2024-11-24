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

        [HttpPost, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Post(CurrencyDto currencyDto)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.Post started");

                await _currencyService.AddCurrency(currencyDto);

                Logger.LogInformation($"CurrencyController.Post completed");
                return Ok();
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.Post completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPut, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put(CurrencyDto currencyDto)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.Put started");

                var currency = await _currencyService.Update(currencyDto);

                Logger.LogInformation($"CurrencyController.Put completed");
                return Ok(currency);
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.Put completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpDelete, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.Delete({id}) started");

                await _currencyService.Delete(id);

                Logger.LogInformation($"CurrencyController.Delete({id}) completed");
                return NoContent();
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.Delete completed; invalid request");
                return BadRequest(ex);
            }
        }
    }
}
