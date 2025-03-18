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
    public class FinanceController : AbstractController
    {
        private readonly IFinanceService _financeService;

        public FinanceController(ILogger<FinanceController> logger, IFinanceService financeService)
            : base(logger)
        {
            _financeService = financeService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Logger.LogInformation($"FinanceController.Get({id}) started");

                var finance = await _financeService.GetFinanceByIdAsync(id); 

                Logger.LogInformation($"FinanceController.Get({id}) completed");
                return Ok(finance);
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning($"FinanceController.Get({id}) completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Logger.LogInformation($"FinanceController.Get started");

                var finances = await _financeService.GetFinancesAsync();

                Logger.LogInformation($"FinanceController.Get completed");
                return Ok(finances);
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("FinanceController.Get completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(FinanceDto financeDto)
        {
            try
            {
                Logger.LogInformation("FinanceController.Post started");

                await _financeService.AddFinanceAsync(financeDto);

                Logger.LogInformation("FinanceController.Post completed");
                return Ok();
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("FinanceController.Post completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(FinanceDto financeDto)
        {
            try
            {
                Logger.LogInformation("FinanceController.Put started");

                var finance = await _financeService.UpdateAsync(financeDto);

                Logger.LogInformation("FinanceController.Put completed");
                return Ok(finance);
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("FinanceController.Put completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Logger.LogInformation($"FinanceController.DeleteAsync({id}) started");

                await _financeService.DeleteAsync(id);

                Logger.LogInformation($"FinanceController.DeleteAsync({id}) completed");
                return NoContent();
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning($"FinanceController.DeleteAsync({id}) completed; invalid request");
                return BadRequest(ex);
            }
        }
    }
}
