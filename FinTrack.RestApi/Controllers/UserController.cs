﻿using FinTrack.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Services.Exceptions;
using FinTrack.Services.Dtos;
using FinTrack.Services.Facades.Contracts;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : AbstractController
    {
        private readonly IUserService _userService;
        private readonly IUserFacade _userFacade;

        public UserController(ILogger<UserController> logger, IUserService userService, IUserFacade userFacade)
            : base(logger)
        {
            _userService = userService;
            _userFacade = userFacade;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Logger.LogInformation($"UserController.Get({id}) started");

                var user = await _userService.GetByIdAsync(id);

                Logger.LogInformation($"UserController.Get({id}) completed");
                return Ok(user);
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation($"UserController.Get({id}) completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get()
        {
            try
            {
                Logger.LogInformation("UserController.Get started");

                var users = await _userService.GetUsers();  

                Logger.LogInformation("UserController.Get completed");
                return Ok(users);
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation("UserController.Get completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            try
            {
                Logger.LogInformation("UserController.Post started");

                await _userService.AddUserAsync(userDto);

                Logger.LogInformation("UserController.Post completed");
                return Ok();
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation("UserController.Post completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("updatePassword")]
        public async Task<IActionResult> UpdatePassword(string oldPassword, string newPassword)
        {
            try
            {
                Logger.LogInformation("UserController.UpdatePasswordAsync started");

                await _userFacade.UpdatePasswordAsync(oldPassword, newPassword);

                Logger.LogInformation("UserController.UpdatePasswordAsync completed");
                return Ok();
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation("UserController.UpdatePasswordAsync completle; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserDto userDto)
        {
            try
            {
                Logger.LogInformation("UserController.Put started");

                var user = await _userService.UpdateAsync(userDto);

                Logger.LogInformation("UserController.Put completed");
                return Ok(user);
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation("UserController.Put completed; invalid request");
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Logger.LogInformation("UserController.DeleteAsync started");

                await _userService.DeleteAsync(id);

                Logger.LogInformation("UserController.DeleteAsync completed");
                return NoContent();
            }
            catch (ValidationException ex)
            {
                Logger.LogInformation("UserController.DeleteAsync completed; invalid request");
                return BadRequest(ex);
            }
        }
    }
}
