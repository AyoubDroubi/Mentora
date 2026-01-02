using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities.StudyPlanner
{
    /// <summary>
    /// StudyQuizAttempt entity for Study Planner Module
    /// Per SRS Study Planner - Feature 6: Study Quiz (Diagnostic)
    /// FR-QZ-01 to FR-QZ-03
    /// </summary>
    public class StudyQuizAttempt : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string AnswersJson { get; set; } = string.Empty;
        public string GeneratedPlan { get; set; } = string.Empty;
    }
}
