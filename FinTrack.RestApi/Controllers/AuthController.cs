using Microsoft.AspNetCore.Mvc;
using FinTrack.RestApi.Models;
using FinTrack.Services.Dtos;
using FinTrack.Services.Facades.Contracts;
using FinTrack.Services.Exceptions;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : AbstractController
    {
        private readonly IAuthFacade _authFacade;

        public AuthController(ILogger<AuthController> logger, IAuthFacade authFacade)
            : base(logger)
        {
            _authFacade = authFacade;
        }

        [HttpPost]
        [Route("singUp")]
        public async Task<IActionResult> SingUp(SignUpModel model)
        {
            try
            {
                Logger.LogInformation("AuthController.SingUpAsync started");

                if (!ModelState.IsValid)
                {
                    Logger.LogWarning($"AuthController.SingUpAsync failed: {ModelState}");
                    return BadRequest(ModelState);
                }

                await _authFacade.SingUpAsync(new SignUpDto()
                {
                    Email = model.Email,
                    Password = model.Password,
                });

                Logger.LogInformation("AuthController.SingUpAsync completed; success");
                return Ok("The user is registered.");
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation("AuthController.SingUpAsync completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(LoginModel loginModel)
        {
            try
            {
                Logger.LogInformation("AuthController.LogInAsync started");

                if (!ModelState.IsValid)
                {
                    Logger.LogWarning($"AuthController.LogInAsync failed: {ModelState}");
                    return BadRequest(ModelState);
                }

                var (token, refreshToken) = await _authFacade.LogInAsync(new LoginDto()
                {
                    Email = loginModel.Email,
                    Password = loginModel.Password,
                });

                Logger.LogInformation("AuthController.LogInAsync completed");
                return Ok(new
                {
                    Token = token,
                    RefreshToken = refreshToken
                });
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("AuthController.LogInAsync completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel model)
        {
            try
            {
                Logger.LogInformation("AuthController.Refresh started");

                if (!ModelState.IsValid)
                {
                    Logger.LogWarning($"AuthController.Refresh failed: {ModelState}");
                    return BadRequest(ModelState);
                }

                var (token, refreshToken) = await _authFacade.RefreshTokenAsync(model.RefreshToken);

                Logger.LogInformation("AuthController.Refresh completed");
                return Ok(new
                {
                    Token = token,
                    RefreshToken = refreshToken
                });
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("AuthController.Refresh completed; invalid request");
                return BadRequest(ex);
            }
        }
    }
}
