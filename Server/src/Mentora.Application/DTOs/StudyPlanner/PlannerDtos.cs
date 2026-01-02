namespace Mentora.Application.DTOs.StudyPlanner
{
    /// <summary>
    /// DTO for creating a new event
    /// </summary>
    public class CreateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime EventDateTime { get; set; }
    }

    /// <summary>
    /// DTO for planner event response
    /// </summary>
    public class PlannerEventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDateTime { get; set; }
        public bool IsAttended { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
