using Mentora.Application.DTOs.StudyPlanner;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;

namespace Mentora.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Attendance operations
    /// Per SRS Study Planner - Feature 4: Attendance & Overall Progress Percentage
    /// </summary>
    public class AttendanceService : IAttendanceService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IPlannerRepository _plannerRepository;

        public AttendanceService(ITodoRepository todoRepository, IPlannerRepository plannerRepository)
        {
            _todoRepository = todoRepository;
            _plannerRepository = plannerRepository;
        }

        public async Task<AttendanceSummaryDto> GetSummaryAsync(Guid userId)
        {
            // Get Todo statistics
            var totalTasks = await _todoRepository.GetTotalCountAsync(userId);
            var completedTasks = await _todoRepository.GetCompletedCountAsync(userId);
            var pendingTasks = totalTasks - completedTasks;

            // Get Event statistics
            var totalPastEvents = await _plannerRepository.GetTotalPastEventsCountAsync(userId);
            var attendedEvents = await _plannerRepository.GetAttendedEventsCountAsync(userId);

            // Get upcoming events
            var upcomingEvents = (await _plannerRepository.GetUpcomingEventsAsync(userId)).Count();

            // Calculate progress percentage (50% tasks + 50% events)
            int progressPercentage = 0;
            int tasksContribution = 0;
            int eventsContribution = 0;

            if (totalTasks > 0 || totalPastEvents > 0)
            {
                if (totalTasks > 0)
                {
                    tasksContribution = (int)Math.Round(((double)completedTasks / totalTasks) * 50);
                }

                if (totalPastEvents > 0)
                {
                    eventsContribution = (int)Math.Round(((double)attendedEvents / totalPastEvents) * 50);
                }

                progressPercentage = tasksContribution + eventsContribution;
            }

            var taskCompletionRate = totalTasks > 0 
                ? (int)Math.Round(((double)completedTasks / totalTasks) * 100) 
                : 0;

            var attendanceRate = totalPastEvents > 0 
                ? (int)Math.Round(((double)attendedEvents / totalPastEvents) * 100) 
                : 0;

            return new AttendanceSummaryDto
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                TaskCompletionRate = taskCompletionRate,
                TotalPastEvents = totalPastEvents,
                AttendedEvents = attendedEvents,
                UpcomingEvents = upcomingEvents,
                AttendanceRate = attendanceRate,
                ProgressPercentage = progressPercentage,
                Breakdown = new ProgressBreakdownDto
                {
                    TasksContribution = tasksContribution,
                    EventsContribution = eventsContribution
                }
            };
        }

        public async Task<AttendanceHistoryDto> GetHistoryAsync(Guid userId, int days = 30)
        {
            var startDate = DateTime.UtcNow.AddDays(-days).Date;
            var endDate = DateTime.UtcNow;

            // Get past events in the range
            var allEvents = await _plannerRepository.GetUserEventsAsync(userId);
            var eventsInRange = allEvents
                .Where(e => e.EventDateTimeUtc >= startDate && e.EventDateTimeUtc <= endDate)
                .OrderByDescending(e => e.EventDateTimeUtc)
                .ToList();

            // Get completed tasks in the range
            var allTodos = await _todoRepository.GetUserTodosAsync(userId, "completed");
            var tasksInRange = allTodos
                .Where(t => t.UpdatedAt >= startDate && t.UpdatedAt <= endDate)
                .OrderByDescending(t => t.UpdatedAt)
                .ToList();

            // Calculate summary
            var tasksTotal = await _todoRepository.GetTotalCountAsync(userId);
            var tasksCompleted = tasksInRange.Count;
            var tasksPending = tasksTotal - (await _todoRepository.GetCompletedCountAsync(userId));

            var eventsTotal = eventsInRange.Count;
            var eventsAttended = eventsInRange.Count(e => e.IsAttended);
            var eventsMissed = eventsTotal - eventsAttended;

            return new AttendanceHistoryDto
            {
                Period = new PeriodDto
                {
                    From = startDate.ToString("yyyy-MM-dd"),
                    To = endDate.ToString("yyyy-MM-dd"),
                    Days = days
                },
                Events = eventsInRange.Select(e => new AttendanceEventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    EventDateTime = e.EventDateTimeUtc,
                    IsAttended = e.IsAttended
                }).ToList(),
                Tasks = tasksInRange.Select(t => new AttendanceTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    CompletedAt = t.UpdatedAt,
                    IsCompleted = t.IsCompleted
                }).ToList(),
                Summary = new AttendanceHistorySummaryDto
                {
                    TasksTotal = tasksTotal,
                    TasksCompleted = tasksCompleted,
                    TasksPending = tasksPending,
                    EventsTotal = eventsTotal,
                    EventsAttended = eventsAttended,
                    EventsMissed = eventsMissed
                }
            };
        }

        public async Task<WeeklyProgressDto> GetWeeklyProgressAsync(Guid userId)
        {
            var today = DateTime.UtcNow.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var weeklyData = new List<DailyProgressDto>();

            var allTodos = await _todoRepository.GetUserTodosAsync(userId, "all");
            var allEvents = await _plannerRepository.GetUserEventsAsync(userId);

            for (int i = 0; i < 7; i++)
            {
                var currentDay = startOfWeek.AddDays(i);
                var nextDay = currentDay.AddDays(1);

                var dayTasks = allTodos.Count(t => t.CreatedAt >= currentDay && t.CreatedAt < nextDay);
                var dayCompletedTasks = allTodos.Count(t => t.IsCompleted && t.UpdatedAt >= currentDay && t.UpdatedAt < nextDay);

                var dayEvents = allEvents.Count(e => e.EventDateTimeUtc >= currentDay && e.EventDateTimeUtc < nextDay);
                var dayAttendedEvents = allEvents.Count(e => e.IsAttended && e.EventDateTimeUtc >= currentDay && e.EventDateTimeUtc < nextDay);

                weeklyData.Add(new DailyProgressDto
                {
                    Date = currentDay.ToString("yyyy-MM-dd"),
                    DayOfWeek = currentDay.DayOfWeek.ToString(),
                    Tasks = dayTasks,
                    CompletedTasks = dayCompletedTasks,
                    Events = dayEvents,
                    AttendedEvents = dayAttendedEvents
                });
            }

            return new WeeklyProgressDto
            {
                WeekStart = startOfWeek.ToString("yyyy-MM-dd"),
                WeekEnd = endOfWeek.ToString("yyyy-MM-dd"),
                Days = weeklyData
            };
        }
    }
}
