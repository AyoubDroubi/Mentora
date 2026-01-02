namespace Mentora.Application.DTOs.StudyPlanner
{
    /// <summary>
    /// DTO for creating a new todo item
    /// </summary>
    public class CreateTodoDto
    {
        public string Title { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for todo item response
    /// </summary>
    public class TodoItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for todo summary statistics
    /// </summary>
    public class TodoSummaryDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletionRate { get; set; }
    }
}
