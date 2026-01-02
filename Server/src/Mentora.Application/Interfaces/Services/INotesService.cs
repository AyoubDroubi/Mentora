using Mentora.Application.DTOs.StudyPlanner;

namespace Mentora.Application.Interfaces.Services
{
    /// <summary>
    /// Service interface for Notes operations
    /// Contains business logic for note management
    /// </summary>
    public interface INotesService
    {
        Task<IEnumerable<UserNoteDto>> GetUserNotesAsync(Guid userId);
        Task<UserNoteDto?> GetByIdAsync(Guid id, Guid userId);
        Task<UserNoteDto> CreateNoteAsync(Guid userId, CreateNoteDto dto);
        Task<UserNoteDto?> UpdateNoteAsync(Guid id, Guid userId, UpdateNoteDto dto);
        Task<bool> DeleteNoteAsync(Guid id, Guid userId);
    }
}
