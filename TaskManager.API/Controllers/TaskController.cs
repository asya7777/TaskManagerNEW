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
        private readonly FinishTaskHandler _finishTaskHandler;
        private readonly AddTagHandler _addTagHandler;
        
        public TaskController(GetTasksHandler getTasksHandler, CreateTaskHandler createTaskHandler, FinishTaskHandler finishTaskHandler,AddTagHandler addTagHandler)
        {
            _getTasksHandler = getTasksHandler;
            _createTaskHandler = createTaskHandler;
            _finishTaskHandler = finishTaskHandler;
            _addTagHandler = addTagHandler;
        }


        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetTasks(int userId)
        {
            var tasks = await _getTasksHandler.HandleAsync(userId); 

            return Ok(tasks);
        }

        //[Authorize(Roles ="User")]
        [HttpPost("create-task")]//will handle POST requests
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO dto)
        {
            await _createTaskHandler.HandleAsync(dto);
            return Ok("Task created successfully!");

        }

        [HttpPut("{taskId}/finish-task")]
        public async Task<IActionResult> FinishTask(int taskId, [FromBody] UpdateTaskDTO dto)
        {
            try
            {
                await _finishTaskHandler.HandleAsync(taskId, dto.isFinished);
                return Ok(new { message = "Task status updated", isFinished = dto.isFinished });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("add-tag")]
        public async Task<IActionResult> AddTagToTask([FromBody] AddTagDTO dto)
        {
            var tag = await _addTagHandler.HandleAsync(dto.tagName);
            return Ok(tag);
        }

        [HttpGet("get-all-tags")]
        public async Task<IActionResult> GetAllTags()
        {
            var tags=await _addTagHandler.GetAllTagsAsync();
            return Ok(tags);
        }
    }
}