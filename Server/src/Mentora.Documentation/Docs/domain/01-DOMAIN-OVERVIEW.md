# Domain Layer Overview - Mentora Platform

## Table of Contents
- [Overview](#overview)
- [Core Entities](#core-entities)
- [Enumerations](#enumerations)
- [Entity Relationships](#entity-relationships)
- [Business Rules](#business-rules)
- [BaseEntity](#baseentity)

---

## Overview

The Domain Layer is the **heart of the system** containing:
- Entities
- Business Rules
- Enumerations
- Base Entity
- **NO** dependencies on other layers
- **NO** external dependencies

### Domain Layer Principles

```
???????????????????????????????????????
?      Domain Layer Principles        ?
???????????????????????????????????????
?                                     ?
?  ? Pure .NET (No Dependencies)     ?
?  ? POCO (Plain Old CLR Objects)    ?
?  ? Business Logic Only              ?
?  ? Framework Independent            ?
?  ? Testable                         ?
?                                     ?
???????????????????????????????????????
```

---

## Core Entities

### 1. User

**Location**: `Entities/Auth/User.cs`

```csharp
public class User : BaseEntity
{
    // Personal Info
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    // Account Status
    public bool IsEmailConfirmed { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation Properties
    public UserProfile? UserProfile { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<CareerPlan> CareerPlans { get; set; } = new List<CareerPlan>();
    public ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();
    
    // Computed Property
    public string FullName => $"{FirstName} {LastName}";
}
```

**Key Points**:
- Inherits from BaseEntity (GUID primary key)
- `PasswordHash` not `Password` (security)
- One-to-One and One-to-Many relationships
- Computed property for FullName

---

### 2. UserProfile

**Location**: `Entities/UserProfile.cs`

```csharp
public class UserProfile : BaseEntity
{
    // User Reference
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    // Personal Info
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    
    // Career Info
    public string? CurrentRole { get; set; }
    public string? TargetRole { get; set; }
    public int? YearsOfExperience { get; set; }
    
    // Collections
    public ICollection<UserSkill> Skills { get; set; } = new List<UserSkill>();
    public ICollection<UserAchievement> Achievements { get; set; } = new List<UserAchievement>();
}
```

**Key Points**:
- One-to-One with User
- Optional fields (nullable)
- Related collections (Skills, Achievements)

---

### 3. CareerPlan

**Location**: `Entities/CareerPlan.cs`

```csharp
public class CareerPlan : BaseEntity
{
    // User Reference
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public string TargetRole { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    
    // Timeline
    public int TimelineMonths { get; set; } = 12;
    public int CurrentStepIndex { get; set; } = 0;
    
    // Status
    public bool IsActive { get; set; } = true;
    public PlanStatus Status { get; set; } = PlanStatus.Active;
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Collections
    public ICollection<CareerStep> Steps { get; set; } = new List<CareerStep>();
}
```

**Key Points**:
- Many-to-One with User
- One-to-Many with CareerStep
- Progress tracking (CurrentStepIndex)
- Status management (PlanStatus)

---

### 4. CareerStep

**Location**: `Entities/CareerStep.cs`

```csharp
public class CareerStep : BaseEntity
{
    // Plan Reference
    public Guid CareerPlanId { get; set; }
    public CareerPlan CareerPlan { get; set; } = null!;
    
    public int StepNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Timeline
    public int EstimatedDurationDays { get; set; }
    public CareerStepStatus Status { get; set; } = CareerStepStatus.NotStarted;
    
    // Timestamps
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
}
```

**Key Points**:
- Many-to-One with CareerPlan
- Sequential ordering (StepNumber)
- Status tracking (Started, Completed, Due)

---

### 5. StudyPlan

**Location**: `Entities/StudyPlan.cs`

```csharp
public class StudyPlan : BaseEntity
{
    // User Reference
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Timeline
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Status
    public bool IsActive { get; set; } = true;
    public PlanStatus Status { get; set; } = PlanStatus.Active;
}
```

**Key Points**:
- Many-to-One with User
- Date range tracking (StartDate, EndDate)

---

### 6. Skill

**Location**: `Entities/Skill.cs`

```csharp
public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Collections
    public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
```

**Key Points**:
- Master Data (reference list)
- Many-to-Many with UserProfile

---

### 7. UserSkill

**Location**: `Entities/UserSkill.cs`

```csharp
public class UserSkill : BaseEntity
{
    // References
    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
    
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    
    // Proficiency
    public int ProficiencyLevel { get; set; } // 0-100
    public SkillLevel Level { get; set; } = SkillLevel.Beginner;
    
    // Timestamp
    public DateTime AcquiredAt { get; set; } = DateTime.UtcNow;
}
```

**Key Points**:
- Many-to-Many Relationship (Join Table)
- Proficiency tracking

---

### 8. UserAchievement

**Location**: `Entities/UserAchievement.cs`

```csharp
public class UserAchievement : BaseEntity
{
    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime AchievedAt { get; set; } = DateTime.UtcNow;
}
```

---

### 9. RefreshToken

**Location**: `Entities/RefreshToken.cs`

```csharp
public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    // Device Info
    public string? DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    
    // Status
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }
}
```

**Key Points**:
- Device tracking
- Supports multi-device logout

---

### 10. PasswordResetToken

**Location**: `Entities/Auth/PasswordResetToken.cs`

```csharp
public class PasswordResetToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
    public DateTime? UsedAt { get; set; }
}
```

**Key Points**:
- One-time use token
- Expiration tracking

---

## Enumerations

### 1. PlanStatus

```csharp
public enum PlanStatus
{
    Active,      // Active
    Completed,   // Completed
    Archived     // Archived
}
```

**Used By**:
- CareerPlan.Status
- StudyPlan.Status

---

### 2. CareerStepStatus

```csharp
public enum CareerStepStatus
{
    NotStarted,  // Not Started
    InProgress,  // In Progress
    Completed    // Completed
}
```

**Used By**:
- CareerStep.Status

---

### 3. StepStatus

```csharp
public enum StepStatus
{
    Locked,      // Locked (prerequisites not met)
    NotStarted,  // Not Started
    InProgress,  // In Progress
    Completed,   // Completed
    Skipped      // Skipped
}
```

---

### 4. SkillLevel

```csharp
public enum SkillLevel
{
    Beginner,      // Beginner (0-30)
    Intermediate,  // Intermediate (31-70)
    Advanced       // Advanced (71-100)
}
```

**Used By**:
- UserSkill.Level

---

### 5. UserRole

```csharp
public enum UserRole
{
    Student,  // Student
    Admin     // Admin
}
```

---

### 6. StudyLevel

```csharp
public enum StudyLevel
{
    Freshman,   // Year 1
    Sophomore,  // Year 2
    Junior,     // Year 3
    Senior,     // Year 4
    Graduate    // Post-grad
}
```

---

### 7. TaskPriority

```csharp
public enum TaskPriority
{
    Low,     // Low
    Medium,  // Medium
    High     // High
}
```

---

### 8. TaskStatus

```csharp
public enum TaskStatus
{
    Pending,     // Pending
    InProgress,  // In Progress
    Done         // Done
}
```

---

## Entity Relationships

### ER Diagram

```
???????????????????????????????????????????????
?           Entity Relationships              ?
???????????????????????????????????????????????

                ????????????
                ?   User   ?
                ????????????
                      ?
      ?????????????????????????????????
      ?               ?               ?
      ?               ?               ?
????????????  ????????????????  ????????????
? Profile  ?  ? CareerPlan   ?  ?RefreshTk ?
????????????  ????????????????  ????????????
     ?               ?
     ?????????       ?
     ?       ?       ?
     ?       ?       ?
??????????? ??????????????
?UserSkill? ? CareerStep ?
??????????? ??????????????
    ?
    ?
??????????
? Skill  ?
??????????

Relationships:
?????????????
User 1:1 UserProfile
User 1:N CareerPlan
User 1:N StudyPlan
User 1:N RefreshToken
UserProfile 1:N UserSkill
UserProfile 1:N UserAchievement
Skill 1:N UserSkill
CareerPlan 1:N CareerStep
```

### Relationship Details

#### 1. User ? UserProfile (One-to-One)

```csharp
// In User
public UserProfile? UserProfile { get; set; }

// In UserProfile
public Guid UserId { get; set; }
public User User { get; set; } = null!;
```

#### 2. User ? CareerPlan (One-to-Many)

```csharp
// In User
public ICollection<CareerPlan> CareerPlans { get; set; } = new List<CareerPlan>();

// In CareerPlan
public Guid UserId { get; set; }
public User User { get; set; } = null!;
```

#### 3. CareerPlan ? CareerStep (One-to-Many)

```csharp
// In CareerPlan
public ICollection<CareerStep> Steps { get; set; } = new List<CareerStep>();

// In CareerStep
public Guid CareerPlanId { get; set; }
public CareerPlan CareerPlan { get; set; } = null!;
```

#### 4. UserProfile ? Skill (Many-to-Many via UserSkill)

```csharp
// In UserProfile
public ICollection<UserSkill> Skills { get; set; } = new List<UserSkill();

// In Skill
public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

// In UserSkill (Join Table)
public Guid UserProfileId { get; set; }
public UserProfile UserProfile { get; set; } = null!;
public Guid SkillId { get; set; }
public Skill Skill { get; set; } = null!;
```

---

## Business Rules

### 1. User

```
? Email must be unique
? Password must be at least 8 characters
? FirstName and LastName required
? Cannot be deleted (Soft Delete)
```

### 2. CareerPlan

```
? TimelineMonths: 1-60
? CurrentStepIndex: 0 to Steps.Count
? Only one active plan per user at a time
? When status = Completed, all steps must be completed
```

### 3. CareerStep

```
? Cannot start until previous step completed
? When InProgress, must have StartedAt
? DueDate cannot be in the past
```

### 4. UserSkill

```
? ProficiencyLevel: 0-100
? Level determined by ProficiencyLevel:
    - Beginner: 0-30
    - Intermediate: 31-70
    - Advanced: 71-100
```

---

## BaseEntity

### Definition

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
```

### Properties

- `Id` - GUID primary key
- `CreatedAt` - Creation timestamp
- `UpdatedAt` - Last update timestamp
- `IsDeleted` - Soft delete flag

### Benefits

1. **Consistency** - All entities have same base properties
2. **Soft Delete** - Never physically delete records
3. **Audit Trail** - Track when records created/updated
4. **GUID** - Globally unique identifiers

### Usage

```csharp
// All entities inherit from BaseEntity
public class User : BaseEntity
{
    // User-specific properties
    public string Email { get; set; }
    // Id, CreatedAt, UpdatedAt, IsDeleted inherited
}
```

---

## Design Patterns

### 1. Repository Pattern (Future)

```csharp
// Defined in Infrastructure
public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
}
```

### 2. Specification Pattern (Future)

```csharp
// Business rule encapsulation
public class ActiveCareerPlansSpecification
{
    public Expression<Func<CareerPlan, bool>> Criteria =>
        plan => plan.IsActive && !plan.IsDeleted;
}
```

### 3. Domain Events (Future)

```csharp
// Event-driven architecture
public class CareerPlanCompletedEvent
{
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime CompletedAt { get; set; }
}
```

---

## Related Documentation

- [02-ENTITIES-GUIDE.md](./02-ENTITIES-GUIDE.md) - Detailed entity guide
- [03-ENUMS-REFERENCE.md](./03-ENUMS-REFERENCE.md) - Enumerations reference
- [06-BUSINESS-RULES.md](./06-BUSINESS-RULES.md) - Business rules details
- [entities/User.md](./entities/User.md) - User entity details
- [entities/CareerPlan.md](./entities/CareerPlan.md) - CareerPlan entity details

---

**Last Updated**: 2024-12-31  
**Version**: 1.0.0
