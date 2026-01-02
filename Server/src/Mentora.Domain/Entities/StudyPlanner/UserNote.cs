using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities.StudyPlanner
{
    /// <summary>
    /// UserNote entity for Study Planner Module
    /// Per SRS Study Planner - Feature 5: Notes
    /// FR-NO-01 to FR-NO-03
    /// </summary>
    public class UserNote : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
