using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;

namespace Mentora.Domain.Entities
{
    public class StudyPlan : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string StrategyName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}