namespace Mentora.Application.DTOs.StudyPlanner
{
    // Study Quiz DTOs
    public class StudyQuizResultDto
    {
        public Guid AttemptId { get; set; }
        public DateTime CreatedAt { get; set; }
        public object StudyPlan { get; set; } = new();
    }

    // Study Sessions DTOs
    public class SaveSessionDto
    {
        public int DurationMinutes { get; set; }
        public DateTime? StartTime { get; set; }
        public int PauseCount { get; set; } = 0;
        public int FocusScore { get; set; } = 100;
    }

    public class StudySessionDto
    {
        public Guid Id { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int PauseCount { get; set; }
        public int FocusScore { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StudySessionSummaryDto
    {
        public int TotalMinutes { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string Formatted { get; set; } = string.Empty;
    }

    public class SessionRangeResultDto
    {
        public IEnumerable<StudySessionDto> Sessions { get; set; } = new List<StudySessionDto>();
        public StudySessionSummaryDto Summary { get; set; } = new();
    }

    // Attendance DTOs
    public class AttendanceSummaryDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int TaskCompletionRate { get; set; }
        public int TotalPastEvents { get; set; }
        public int AttendedEvents { get; set; }
        public int UpcomingEvents { get; set; }
        public int AttendanceRate { get; set; }
        public int ProgressPercentage { get; set; }
        public ProgressBreakdownDto Breakdown { get; set; } = new();
    }

    public class ProgressBreakdownDto
    {
        public int TasksContribution { get; set; }
        public int EventsContribution { get; set; }
    }

    public class PeriodDto
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public int Days { get; set; }
    }

    public class AttendanceEventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDateTime { get; set; }
        public bool IsAttended { get; set; }
    }

    public class AttendanceTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class AttendanceHistorySummaryDto
    {
        public int TasksTotal { get; set; }
        public int TasksCompleted { get; set; }
        public int TasksPending { get; set; }
        public int EventsTotal { get; set; }
        public int EventsAttended { get; set; }
        public int EventsMissed { get; set; }
    }

    public class AttendanceHistoryDto
    {
        public PeriodDto Period { get; set; } = new();
        public IEnumerable<AttendanceEventDto> Events { get; set; } = new List<AttendanceEventDto>();
        public IEnumerable<AttendanceTaskDto> Tasks { get; set; } = new List<AttendanceTaskDto>();
        public AttendanceHistorySummaryDto Summary { get; set; } = new();
    }

    public class WeeklyProgressDto
    {
        public string WeekStart { get; set; } = string.Empty;
        public string WeekEnd { get; set; } = string.Empty;
        public IEnumerable<DailyProgressDto> Days { get; set; } = new List<DailyProgressDto>();
    }

    public class DailyProgressDto
    {
        public string Date { get; set; } = string.Empty;
        public string DayOfWeek { get; set; } = string.Empty;
        public int Tasks { get; set; }
        public int CompletedTasks { get; set; }
        public int Events { get; set; }
        public int AttendedEvents { get; set; }
    }
}
