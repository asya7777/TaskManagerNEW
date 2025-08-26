using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs;

using TaskManager.Infrastructure.Services;
using TaskManager.Application.Handlers.Users;
using Microsoft.AspNetCore.Authorization;//email verification için

namespace TaskManager.Controllers
{

    [ApiController]
    [Route("api/[controller]")]


    public class UserController : ControllerBase
    {
        private readonly RegisterUserHandler _registerUserHandler;
        private readonly LoginUserHandler _loginUserHandler;
        private readonly VerifyEmailHandler _verifyEmailHandler;
        private readonly GetAllUsersHandler _getAllUsersHandler;
        private readonly DeleteUserHandler _deleteUserHandler;
        public UserController(RegisterUserHandler registerUserHandler, LoginUserHandler loginUserHandler, VerifyEmailHandler verifyEmailHandler, GetAllUsersHandler getAllUsersHandler, DeleteUserHandler deleteUserHandler)
        {
            _registerUserHandler = registerUserHandler;
            _loginUserHandler = loginUserHandler;
            _verifyEmailHandler = verifyEmailHandler;
            _getAllUsersHandler = getAllUsersHandler;
            _deleteUserHandler = deleteUserHandler;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO dto)
        {
            await _registerUserHandler.HandleAsync(dto);

            return Ok("User registered successfully! Please check your email to verify your account.");
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            await _verifyEmailHandler.HandleAsync(token);

            return Redirect("http://localhost:5173/login?verified=true");//query param

        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO dto)
        {
            var result = await _loginUserHandler.HandleAsync(dto);

            if (result == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            if (result.GetType().GetProperty("error") != null)
            {
                return BadRequest(result);
            }   
            return Ok(result);
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _getAllUsersHandler.HandleAsync();

            return Ok(users);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _deleteUserHandler.HandleAsync(id);

            if (!result)
            {
                return NotFound("User not found.");
            }
            return Ok("User deleted successfully.");
        }
    }
}
