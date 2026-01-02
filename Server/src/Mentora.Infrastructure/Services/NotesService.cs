using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities.StudyPlanner;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Notes operations
    /// Contains all business logic for note management
    /// Per SRS Study Planner - Feature 5: Notes
    /// </summary>
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;

        public NotesService(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task<IEnumerable<UserNoteDto>> GetUserNotesAsync(Guid userId)
        {
            var notes = await _notesRepository.GetUserNotesAsync(userId);
            return notes.Select(MapToDto);
        }

        public async Task<UserNoteDto?> GetByIdAsync(Guid id, Guid userId)
        {
            var note = await _notesRepository.GetByIdAsync(id, userId);
            return note == null ? null : MapToDto(note);
        }

        public async Task<UserNoteDto> CreateNoteAsync(Guid userId, CreateNoteDto dto)
        {
            // Business Rule: Title and Content are required
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required", nameof(dto.Title));

            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Content is required", nameof(dto.Content));

            var note = new UserNote
            {
                UserId = userId,
                Title = dto.Title.Trim(),
                Content = dto.Content.Trim()
            };

            var createdNote = await _notesRepository.CreateAsync(note);
            return MapToDto(createdNote);
        }

        public async Task<UserNoteDto?> UpdateNoteAsync(Guid id, Guid userId, UpdateNoteDto dto)
        {
            var note = await _notesRepository.GetByIdAsync(id, userId);

            if (note == null)
                return null;

            // Business Logic: Update only provided fields
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                note.Title = dto.Title.Trim();
            }

            if (!string.IsNullOrWhiteSpace(dto.Content))
            {
                note.Content = dto.Content.Trim();
            }

            var updatedNote = await _notesRepository.UpdateAsync(note);
            return MapToDto(updatedNote);
        }

        public async Task<bool> DeleteNoteAsync(Guid id, Guid userId)
        {
            return await _notesRepository.DeleteAsync(id, userId);
        }

        private UserNoteDto MapToDto(UserNote note)
        {
            return new UserNoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }
    }
}
