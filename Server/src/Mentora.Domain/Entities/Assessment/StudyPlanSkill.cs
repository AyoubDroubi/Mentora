using Mentora.Domain.Common;

namespace Mentora.Domain.Entities.Assessment
{
    /// <summary>
    /// Many-to-Many relationship between Study Plan and Skills per SRS 3.3
    /// Tracks required skills for study plan completion with target proficiency
    /// Integrates with UserProfileSkill for gap analysis per SRS 3.3.3
    /// </summary>
    public class StudyPlanSkill : BaseEntity
    {
        /// <summary>
        /// Study plan requiring this skill
        /// </summary>
        public Guid StudyPlanId { get; set; }
        public AiStudyPlan StudyPlan { get; set; } = null!;

        /// <summary>
        /// Skill required
        /// </summary>
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;

        /// <summary>
        /// Target proficiency level for this skill in this plan
        /// </summary>
        public SkillLevel TargetProficiency { get; set; }

        /// <summary>
        /// Importance/priority of this skill (1-5)
        /// </summary>
        public int Importance { get; set; } = 3;

        /// <summary>
        /// Order/sequence for skill acquisition
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// Whether this is a prerequisite skill
        /// </summary>
        public bool IsPrerequisite { get; set; } = false;

        /// <summary>
        /// Optional notes about why this skill is needed
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Current skill status relative to the plan per SRS 3.3.3
        /// </summary>
        public SkillStatus Status { get; set; } = SkillStatus.Missing;

        /// <summary>
        /// Gap analysis result: difference between user's current and target level
        /// Positive = needs improvement, Negative = exceeds requirement, 0 = matches
        /// </summary>
        public int ProficiencyGap { get; set; } = 0;
    }
}
