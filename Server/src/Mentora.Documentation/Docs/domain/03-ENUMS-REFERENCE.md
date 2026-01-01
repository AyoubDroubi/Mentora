# Enums Reference - Mentora Platform

## Table of Contents
- [Overview](#overview)
- [Status Enums](#status-enums)
- [User Enums](#user-enums)
- [Skill Enums](#skill-enums)
- [Task Enums](#task-enums)
- [Best Practices](#best-practices)

---

## Overview

All enumerations are defined in: `Mentora.Domain/Entities/Enums/Enums.cs`

```csharp
namespace Mentora.Domain.Entities
{
    // All enums defined here
}
```

**Benefits**:
- Type safety
- Validation
- Clear intent
- Easy maintenance

---

## Status Enums

### 1. PlanStatus

```csharp
public enum PlanStatus
{
    Active,      // 0
    Completed,   // 1
    Archived     // 2
}
```

**Usage**:
```csharp
// In CareerPlan
public PlanStatus Status { get; set; } = PlanStatus.Active;

// In StudyPlan
public PlanStatus Status { get; set; } = PlanStatus.Active;
```

**Values**:

| Value | Number | Description | Use Case |
|-------|--------|-------------|----------|
| **Active** | 0 | Currently active and being worked on | Default state, user actively working |
| **Completed** | 1 | Finished successfully | All steps done |
| **Archived** | 2 | No longer active | Kept for history |

**Example**:
```csharp
// Check if plan is active
if (careerPlan.Status == PlanStatus.Active)
{
    // Process active plan
}

// Complete a plan
careerPlan.Status = PlanStatus.Completed;
careerPlan.UpdatedAt = DateTime.UtcNow;
await _context.SaveChangesAsync();

// Get all active plans
var activePlans = await _context.CareerPlans
    .Where(p => p.Status == PlanStatus.Active && !p.IsDeleted)
    .ToListAsync();
```

**State Flow**:
```
Active ? Completed
   ?
Archived
```

---

### 2. CareerStepStatus

```csharp
public enum CareerStepStatus
{
    NotStarted,  // 0
    InProgress,  // 1
    Completed    // 2
}
```

**Usage**:
```csharp
// In CareerStep
public CareerStepStatus Status { get; set; } = CareerStepStatus.NotStarted;
```

**Values**:

| Value | Number | Description | Use Case |
|-------|--------|-------------|----------|
| **NotStarted** | 0 | Not yet begun | Initial state |
| **InProgress** | 1 | Currently working on it | Active work |
| **Completed** | 2 | Finished | Step done |

**Example**:
```csharp
// Start a step
step.Status = CareerStepStatus.InProgress;
step.StartedAt = DateTime.UtcNow;

// Complete a step
step.Status = CareerStepStatus.Completed;
step.CompletedAt = DateTime.UtcNow;

// Calculate progress
var totalSteps = plan.Steps.Count;
var completedSteps = plan.Steps
    .Count(s => s.Status == CareerStepStatus.Completed);
var progressPercentage = (completedSteps * 100) / totalSteps;
```

**State Flow**:
```
NotStarted ? InProgress ? Completed
```

---

### 3. StepStatus

```csharp
public enum StepStatus
{
    Locked,      // 0
    NotStarted,  // 1
    InProgress,  // 2
    Completed,   // 3
    Skipped      // 4
}
```

**Values**:

| Value | Number | Description | Use Case |
|-------|--------|-------------|----------|
| **Locked** | 0 | Cannot start yet | Prerequisites not met |
| **NotStarted** | 1 | Ready to start | Available to begin |
| **InProgress** | 2 | Currently working | Active work |
| **Completed** | 3 | Finished | Step done |
| **Skipped** | 4 | Intentionally skipped | Optional step bypassed |

**Example**:
```csharp
// Unlock step after previous completes
if (previousStep.Status == StepStatus.Completed)
{
    currentStep.Status = StepStatus.NotStarted; // Locked ? NotStarted
}

// Skip optional step
if (step.IsOptional)
{
    step.Status = StepStatus.Skipped;
}
```

**State Flow**:
```
Locked ? NotStarted ? InProgress ? Completed
                          ?
                       Skipped
```

---

## User Enums

### 4. UserRole

```csharp
public enum UserRole
{
    Student,  // 0
    Admin     // 1
}
```

**Values**:

| Value | Number | Description | Permissions |
|-------|--------|-------------|-------------|
| **Student** | 0 | Regular user | Standard features |
| **Admin** | 1 | System administrator | Full access |

**Example**:
```csharp
// Check admin access
[Authorize(Roles = "Admin")]
public async Task<IActionResult> DeleteUser(Guid userId)
{
    // Only admin can delete
}

// In code
if (user.Role == UserRole.Admin)
{
    // Admin actions
}
```

---

### 5. StudyLevel

```csharp
public enum StudyLevel
{
    Freshman,   // 0 - Year 1
    Sophomore,  // 1 - Year 2
    Junior,     // 2 - Year 3
    Senior,     // 3 - Year 4
    Graduate    // 4 - Post-grad
}
```

**Values**:

| Value | Number | Academic Year | AI Complexity |
|-------|--------|---------------|---------------|
| **Freshman** | 0 | Year 1 | Basic concepts |
| **Sophomore** | 1 | Year 2 | Intermediate |
| **Junior** | 2 | Year 3 | Advanced |
| **Senior** | 3 | Year 4 | Job preparation |
| **Graduate** | 4 | Post-grad | Research/Specialization |

**Example**:
```csharp
// Recommend courses based on level
public async Task<List<Course>> GetRecommendedCourses(StudyLevel level)
{
    return level switch
    {
        StudyLevel.Freshman => await GetBeginnerCourses(),
        StudyLevel.Sophomore => await GetIntermediateCourses(),
        StudyLevel.Junior => await GetAdvancedCourses(),
        StudyLevel.Senior => await GetCapstoneProjects(),
        StudyLevel.Graduate => await GetGraduateCourses(),
        _ => new List<Course>()
    };
}

// Display name
public string GetLevelName(StudyLevel level)
{
    return level switch
    {
        StudyLevel.Freshman => "Freshman",
        StudyLevel.Sophomore => "Sophomore",
        StudyLevel.Junior => "Junior",
        StudyLevel.Senior => "Senior",
        StudyLevel.Graduate => "Graduate",
        _ => "Unknown"
    };
}
```

---

## Skill Enums

### 6. SkillLevel

```csharp
public enum SkillLevel
{
    Beginner,      // 0
    Intermediate,  // 1
    Advanced       // 2
}
```

**Values**:

| Value | Number | Proficiency Range | Description |
|-------|--------|-------------------|-------------|
| **Beginner** | 0 | 0-30 | Just started learning |
| **Intermediate** | 1 | 31-70 | Can work with guidance |
| **Advanced** | 2 | 71-100 | Expert level |

**Example**:
```csharp
// Determine skill level based on proficiency
public SkillLevel DetermineSkillLevel(int proficiency)
{
    return proficiency switch
    {
        <= 30 => SkillLevel.Beginner,
        <= 70 => SkillLevel.Intermediate,
        _ => SkillLevel.Advanced
    };
}

// In UserSkill
public void UpdateProficiency(int newLevel)
{
    ProficiencyLevel = Math.Clamp(newLevel, 0, 100);
    Level = DetermineSkillLevel(ProficiencyLevel);
}

// Filter advanced skills
var advancedSkills = userProfile.Skills
    .Where(s => s.Level == SkillLevel.Advanced)
    .ToList();
```

**Proficiency Scale**:
```
  0——————30——————70——————100
  ?      ?       ?       ?
Beginner ? Intermediate ? Advanced
         ?               ?
     Basic Knowledge  Expert
```

---

## Task Enums

### 7. TaskPriority

```csharp
public enum TaskPriority
{
    Low,     // 0
    Medium,  // 1
    High     // 2
}
```

**Values**:

| Value | Number | Description | Suggested Timeline |
|-------|--------|-------------|--------------------|
| **Low** | 0 | Low priority | Can wait |
| **Medium** | 1 | Normal priority | Standard timeline |
| **High** | 2 | High priority | Urgent |

**Example**:
```csharp
// Sort by priority
var sortedTasks = tasks
    .OrderByDescending(t => t.Priority)
    .ThenBy(t => t.DueDate)
    .ToList();

// Auto-determine priority
public TaskPriority DeterminePriority(DateTime dueDate)
{
    var daysUntilDue = (dueDate - DateTime.UtcNow).Days;
    return daysUntilDue switch
    {
        <= 1 => TaskPriority.High,
        <= 7 => TaskPriority.Medium,
        _ => TaskPriority.Low
    };
}

// Display badge
public string GetPriorityBadge(TaskPriority priority)
{
    return priority switch
    {
        TaskPriority.High => "Urgent",
        TaskPriority.Medium => "Normal",
        TaskPriority.Low => "Low",
        _ => "Unknown"
    };
}
```

---

### 8. TaskStatus

```csharp
public enum TaskStatus
{
    Pending,     // 0
    InProgress,  // 1
    Done         // 2
}
```

**Values**:

| Value | Number | Description | Icon |
|-------|--------|-------------|------|
| **Pending** | 0 | Not started | ? |
| **InProgress** | 1 | Working on it | ?? |
| **Done** | 2 | Completed | ? |

**Example**:
```csharp
// Task statistics
var taskStats = new
{
    Pending = tasks.Count(t => t.Status == TaskStatus.Pending),
    InProgress = tasks.Count(t => t.Status == TaskStatus.InProgress),
    Done = tasks.Count(t => t.Status == TaskStatus.Done),
    TotalProgress = (tasks.Count(t => t.Status == TaskStatus.Done) * 100) / tasks.Count
};

// Start task
public async Task<bool> StartTask(Guid taskId)
{
    var task = await _context.Tasks.FindAsync(taskId);
    if (task?.Status == TaskStatus.Pending)
    {
        task.Status = TaskStatus.InProgress;
        task.StartedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
    return false;
}
```

**State Flow**:
```
Pending ? InProgress ? Done
```

---

## Best Practices

### 1. Use Switch Expressions

```csharp
// Good - Switch Expression
public string GetStatusName(PlanStatus status) => status switch
{
    PlanStatus.Active => "Active",
    PlanStatus.Completed => "Completed",
    PlanStatus.Archived => "Archived",
    _ => "Unknown"
};

// Avoid - If/Else
public string GetStatusName(PlanStatus status)
{
    if (status == PlanStatus.Active) return "Active";
    else if (status == PlanStatus.Completed) return "Completed";
    else if (status == PlanStatus.Archived) return "Archived";
    else return "Unknown";
}
```

### 2. Compare by Enum, Not Int

```csharp
// Good
if (plan.Status == PlanStatus.Active)

// Avoid
if ((int)plan.Status == 0)
```

### 3. Use in LINQ

```csharp
// Good - Direct enum comparison
var activePlans = plans.Where(p => p.Status == PlanStatus.Active);

// Good - Sort by enum
var sortedSteps = steps.OrderBy(s => s.Status);
```

### 4. Validation

```csharp
// Good - Validate enum values
public bool IsValidStatus(PlanStatus status)
{
    return Enum.IsDefined(typeof(PlanStatus), status);
}

// Usage
if (!IsValidStatus(inputStatus))
{
    throw new ArgumentException("Invalid status");
}
```

### 5. String Conversion

```csharp
// Good - ToString()
var statusName = status.ToString(); // "Active"

// For localization - use mapping
var statusNameAr = GetLocalizedStatus(status); // "???"
```

---

## Quick Reference

| Enum | Values | Main Use | Entities |
|------|--------|----------|----------|
| PlanStatus | Active, Completed, Archived | Plan lifecycle | CareerPlan, StudyPlan |
| CareerStepStatus | NotStarted, InProgress, Completed | Step progress | CareerStep |
| StepStatus | Locked, NotStarted, InProgress, Completed, Skipped | Sequential steps | (Future) |
| UserRole | Student, Admin | User permissions | User |
| StudyLevel | Freshman, Sophomore, Junior, Senior, Graduate | Academic level | UserProfile |
| SkillLevel | Beginner, Intermediate, Advanced | Skill mastery | UserSkill |
| TaskPriority | Low, Medium, High | Task importance | (Future) |
| TaskStatus | Pending, InProgress, Done | Task state | (Future) |

---

## Adding New Values

### Extending Enums

```csharp
// Before
public enum PlanStatus
{
    Active,
    Completed,
    Archived
}

// After
public enum PlanStatus
{
    Active,
    Completed,
    Archived,
    Suspended  // New value
}
```

**Considerations**:
- Update all switch statements
- Create migration if enum stored in DB
- Update validation logic

---

## Related Documentation

- [01-DOMAIN-OVERVIEW.md](./01-DOMAIN-OVERVIEW.md) - Domain overview
- [02-ENTITIES-GUIDE.md](./02-ENTITIES-GUIDE.md) - Entity guide
- [06-BUSINESS-RULES.md](./06-BUSINESS-RULES.md) - Business rules

---

**Last Updated**: 2024-12-31  
**Version**: 1.0.0
