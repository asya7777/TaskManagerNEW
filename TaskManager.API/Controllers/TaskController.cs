using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using TaskManager.Application.Handlers.Tasks;
using TaskManager.Data;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Controllers
{
    [ApiController]//tells ASP.NET Core that this class is an API controller(receives http requests and sends http responses)

    [Route("api/[controller]")]//controls the url path to access this controller


    public class TaskController: ControllerBase //inherits from ControllerBase provided by ASP.NET Core
    {
        private readonly GetTasksHandler _getTasksHandler;
        private readonly CreateTaskHandler _createTaskHandler;

        public TaskController(GetTasksHandler getTasksHandler, CreateTaskHandler createTaskHandler)
        {
            _getTasksHandler = getTasksHandler;
            _createTaskHandler = createTaskHandler;
        }


        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetTasks(int userId)
        {
            var tasks = await _getTasksHandler.HandleAsync(userId); 

            return Ok(tasks);
        }

        [Authorize(Roles ="User")]
        [HttpPost("create-task")]//will handle POST requests
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO dto)
        {
            await _createTaskHandler.HandleAsync(dto);
            return Ok("Task created successfully!");

        }
    }
}