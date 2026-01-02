using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// NotesController for Study Planner Module
    /// Per SRS Study Planner - Feature 5: Notes
    /// FR-NO-01 to FR-NO-03
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : BaseController
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        /// <summary>
        /// Get all notes for the authenticated user
        /// GET /api/notes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            var userId = GetUserId();
            var notes = await _notesService.GetUserNotesAsync(userId);
            return Ok(new { success = true, data = notes });
        }

        /// <summary>
        /// Get a specific note by ID
        /// GET /api/notes/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(Guid id)
        {
            var userId = GetUserId();
            var note = await _notesService.GetByIdAsync(id, userId);

            if (note == null)
            {
                return NotFound(new { success = false, message = "Note not found" });
            }

            return Ok(new { success = true, data = note });
        }

        /// <summary>
        /// Create a new note
        /// POST /api/notes
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            try
            {
                var userId = GetUserId();
                var note = await _notesService.CreateNoteAsync(userId, dto);
                return Ok(new
                {
                    success = true,
                    message = "Note created successfully",
                    data = note
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing note
        /// PUT /api/notes/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] UpdateNoteDto dto)
        {
            var userId = GetUserId();
            var note = await _notesService.UpdateNoteAsync(id, userId, dto);

            if (note == null)
            {
                return NotFound(new { success = false, message = "Note not found" });
            }

            return Ok(new
            {
                success = true,
                message = "Note updated successfully",
                data = note
            });
        }

        /// <summary>
        /// Delete a note
        /// DELETE /api/notes/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var userId = GetUserId();
            var success = await _notesService.DeleteNoteAsync(id, userId);

            if (!success)
            {
                return NotFound(new { success = false, message = "Note not found" });
            }

            return Ok(new { success = true, message = "Note deleted successfully" });
        }
    }
}
