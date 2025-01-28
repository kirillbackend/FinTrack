using Microsoft.AspNetCore.Mvc;
using FinTrack.RestApi.Models;
using FinTrack.Services.Dtos;
using FinTrack.Services.Facades.Contracts;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Contracts;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : AbstractController
    {
        private readonly IAuthFacade _authFacade;

        public AuthController(ILogger<AuthController> logger, IAuthFacade authFacade, IAuthService authService)
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
                Logger.LogInformation("AuthController.SingUp started");

                if (!ModelState.IsValid)
                {
                    Logger.LogWarning($"AuthController.SingUp failed: {ModelState}");
                    return BadRequest(ModelState);
                }

                await _authFacade.SingUp(new SignUpDto()
                {
                    Email = model.Email,
                    Password = model.Password,
                });

                Logger.LogInformation("AuthController.SingUp completed; success");
                return Ok("The user is registered.");
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation("AuthController.SingUp completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(LoginModel loginModel)
        {
            try
            {
                Logger.LogInformation("AuthController.LogIn started");

                if (!ModelState.IsValid)
                {
                    Logger.LogWarning($"AuthController.LogIn failed: {ModelState}");
                    return BadRequest(ModelState);
                }

                var (token, refreshToken) = await _authFacade.LogIn(new LoginDto()
                {
                    Email = loginModel.Email,
                    Password = loginModel.Password,
                });

                Logger.LogInformation("AuthController.LogIn completed");
                return Ok(new
                {
                    Token = token,
                    RefreshToken = refreshToken
                });
            }
            catch (ValidationException ex)
            {
                Logger.LogWarning("AuthController.LogIn completed; invalid request");
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

                var (token, refreshToken) = await _authFacade.RefreshToken(model.RefreshToken);

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
