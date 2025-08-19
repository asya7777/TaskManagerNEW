using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using TaskManager.Application.Handlers.Tasks;
using TaskManager.Data;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

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


        [HttpGet("get_tasks/my-tasks")]
        public async Task<IActionResult> GetTasks(int userId)
        {
            var tasks = await _getTasksHandler.HandleAsync(userId); 

            return Ok(tasks);
        }

        //now we define the actions that this controller will handle
        [HttpPost("create-task")]//will handle POST requests
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO dto)
        {
            await _createTaskHandler.HandleAsync(dto);
            return Ok("Task created successfully!");

        }
    }
}