namespace Mentora.Application.DTOs.StudyPlanner
{
    /// <summary>
    /// DTO for creating a new note
    /// </summary>
    public class CreateNoteDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for updating a note
    /// </summary>
    public class UpdateNoteDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }

    /// <summary>
    /// DTO for note response
    /// </summary>
    public class UserNoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
