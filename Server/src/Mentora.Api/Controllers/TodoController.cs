using Mentora.Api.Controllers;
using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// TodoController for Study Planner Module
    /// Per SRS Study Planner - Feature 2: ToDo List
    /// FR-TD-01 to FR-TD-05
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : BaseController
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        /// <summary>
        /// Get all todo items with optional filtering
        /// GET /api/todo?filter=all|active|completed
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] string filter = "all")
        {
            var userId = GetUserId();
            var todos = await _todoService.GetUserTodosAsync(userId, filter);
            return Ok(new { success = true, data = todos });
        }

        /// <summary>
        /// Get todo summary statistics
        /// GET /api/todo/summary
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = GetUserId();
            var summary = await _todoService.GetSummaryAsync(userId);
            return Ok(new { success = true, data = summary });
        }

        /// <summary>
        /// Create a new todo item
        /// POST /api/todo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] CreateTodoDto dto)
        {
            try
            {
                var userId = GetUserId();
                var todo = await _todoService.CreateTodoAsync(userId, dto);
                return Ok(new
                {
                    success = true,
                    message = "Todo created successfully",
                    data = todo
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Toggle todo completion status
        /// PATCH /api/todo/{id}
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<IActionResult> ToggleTodo(Guid id)
        {
            var userId = GetUserId();
            var todo = await _todoService.ToggleTodoAsync(id, userId);

            if (todo == null)
            {
                return NotFound(new { success = false, message = "Todo not found" });
            }

            return Ok(new
            {
                success = true,
                message = "Todo updated successfully",
                data = todo
            });
        }

        /// <summary>
        /// Delete a todo item
        /// DELETE /api/todo/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var userId = GetUserId();
            var success = await _todoService.DeleteTodoAsync(id, userId);

            if (!success)
            {
                return NotFound(new { success = false, message = "Todo not found" });
            }

            return Ok(new { success = true, message = "Todo deleted successfully" });
        }
    }
}
