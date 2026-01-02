namespace Mentora.Domain.Entities
{
    public enum UserRole
    { Student, Admin }

    public enum StudyLevel
    { Freshman, Sophomore, Junior, Senior, Graduate }

    public enum PlanStatus
    { Active, Completed, Archived }

    public enum StepStatus
    { Locked, NotStarted, InProgress, Completed, Skipped }

    public enum CareerStepStatus
    { NotStarted, InProgress, Completed }

    public enum TaskPriority
    { Low, Medium, High }

    public enum TaskStatus
    { Pending, InProgress, Done }

    public enum SkillLevel
    { Beginner, Intermediate, Advanced }

    // Career Builder Enums per SRS
    public enum CareerPlanStatus
    { Generated, InProgress, Completed, Outdated }

    public enum CareerQuizStatus
    { Draft, Completed, Outdated }

    public enum SkillStatus
    { Missing, InProgress, Achieved }

    public enum SkillCategory
    { Technical, Soft }
}