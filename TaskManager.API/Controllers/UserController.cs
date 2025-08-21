using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs;

using TaskManager.Infrastructure.Services;
using TaskManager.Application.Handlers.Users;//email verification için

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
        public UserController(RegisterUserHandler registerUserHandler, LoginUserHandler loginUserHandler, VerifyEmailHandler verifyEmailHandler, GetAllUsersHandler getAllUsersHandler)
        {
            _registerUserHandler = registerUserHandler;
            _loginUserHandler = loginUserHandler;
            _verifyEmailHandler = verifyEmailHandler;
            _getAllUsersHandler = getAllUsersHandler;
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

            return Ok(result);
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _getAllUsersHandler.HandleAsync();

            return Ok(users);
        }
    }
}
