using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities.StudyPlanner
{
    /// <summary>
    /// PlannerEvent entity for Study Planner Module
    /// Per SRS Study Planner - Feature 3: Planner (Calendar & Events)
    /// FR-PL-01 to FR-PL-06
    /// </summary>
    public class PlannerEvent : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Title { get; set; } = string.Empty;
        public DateTime EventDateTimeUtc { get; set; }
        public bool IsAttended { get; set; } = false;
    }
}
