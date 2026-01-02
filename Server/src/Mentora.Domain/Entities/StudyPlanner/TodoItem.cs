using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities.StudyPlanner
{
    /// <summary>
    /// TodoItem entity for Study Planner Module
    /// Per SRS Study Planner - Feature 2: ToDo List
    /// FR-TD-01 to FR-TD-05
    /// </summary>
    public class TodoItem : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}
