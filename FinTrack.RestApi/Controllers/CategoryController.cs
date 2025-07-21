using FinTrack.Services.Contracts;
using FinTrack.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Services.Exceptions;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : AbstractController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) : base(logger)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Logger.LogInformation($"CategoryController.Get({id}) started");

                var finance = await _categoryService.GetAsync(id);

                Logger.LogInformation($"CategoryController.Get({id}) completed");
                return Ok(finance);
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning($"CategoryController.Get({id}) completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Logger.LogInformation($"CategoryController.Get started");

                var finances = await _categoryService.GetAsync();

                Logger.LogInformation($"CategoryController.Get completed");
                return Ok(finances);
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("CategoryController.Get completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryDto categoryDto)
        {
            try
            {
                Logger.LogInformation("CategoryController.Post started");

                await _categoryService.AddAsync(categoryDto);

                Logger.LogInformation("CategoryController.Post completed");
                return Ok();
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("CategoryController.Post completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(CategoryDto categoryDto)
        {
            try
            {
                Logger.LogInformation("CategoryController.Put started");

                var finance = await _categoryService.UpdateAsync(categoryDto);

                Logger.LogInformation("CategoryController.Put completed");
                return Ok(finance);
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("CategoryController.Put completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Logger.LogInformation($"CategoryController.Delete({id}) started");

                await _categoryService.DeleteAsync(id);

                Logger.LogInformation($"CategoryController.Delete({id}) completed");
                return NoContent();
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning($"CategoryController.Delete({id}) completed; invalid request");
                return BadRequest(ex);
            }
        }
    }
}
