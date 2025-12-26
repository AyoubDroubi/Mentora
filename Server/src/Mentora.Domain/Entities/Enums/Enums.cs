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

    public enum TaskPriority
    { Low, Medium, High }

    public enum TaskStatus
    { Pending, InProgress, Done }

    public enum SkillLevel
    { Beginner, Intermediate, Advanced }
}