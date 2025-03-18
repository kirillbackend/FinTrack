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
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.Get({id}) started");

                var currency = await _currencyService.GetCurrencyByIdAsync(id);

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

                var currency = await _currencyService.GetCurrenciesAsync();

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

                await _currencyService.AddCurrencyAsync(currencyDto);

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

                var currency = await _currencyService.UpdateAsync(currencyDto);

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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.DeleteAsync({id}) started");

                await _currencyService.DeleteAsync(id);

                Logger.LogInformation($"CurrencyController.DeleteAsync({id}) completed");
                return NoContent();
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.DeleteAsync completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("convertCurrency")]
        public async Task<IActionResult> ConvertCurrency(string to, string from, string amount)
        {
            try
            {
                Logger.LogInformation($"CurrencyController.ConvertCurrencyAsync started");

                var result = await _currencyService.ConvertCurrencyAsync(to, from, amount);

                Logger.LogInformation($"CurrencyController.ConvertCurrencyAsync completed");
                return Ok(new { result = result, from = from, to = to, amount = amount });
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"CurrencyController.ConvertCurrencyAsync completed; invalid request");
                return BadRequest(ex);
            }
        }
    }
}
