# Complete Database Setup Summary

## Current Status: Complete

The database for the Mentora project has been successfully set up!

---

## Issues Resolved During Setup

### 1. Entity Framework Tools Version Mismatch
**Problem**: Had `dotnet-ef` 10.0.1 which is incompatible with .NET 9
```
System.Runtime, Version=10.0.0.0 not found
```

**Solution**: 
```bash
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 9.0.0
```

---

### 2. Entity Relationship Mismatches
**Problem**: Foreign key type mismatches in relationships

#### `UserAchievement.cs`
- **Before**: `public int AchievementId`
- **After**: `public Guid AchievementId` ?

#### `UserSkill.cs`
- **Before**: 
  ```csharp
  public Guid Id { get; set; } = Guid.NewGuid(); // Duplicate!
  public int SkillId { get; set; }
  ```
- **After**:
  ```csharp
  public Guid SkillId { get; set; } ?
  ```

---

### 3. Cascade Delete Conflicts
**Problem**: SQL Server couldn't handle multiple cascade paths
```
FK_StudyTasks_CareerSteps may cause cycles or multiple cascade paths
```

**Solution**: In `ApplicationDbContext.cs`
```csharp
// Before
.OnDelete(DeleteBehavior.SetNull);

// After
.OnDelete(DeleteBehavior.NoAction); ?
```

---

## Final Database Structure

### Complete Tables (25 Tables)

#### Authentication Module
- `AspNetUsers` - Users (from Identity)
- `AspNetRoles` - Roles
- `AspNetUserRoles` - User-Role mapping
- `RefreshTokens` - Refresh tokens for auth
- `PasswordResetTokens` - Password reset tokens

#### User Profile & Stats
- `UserProfiles` - Extended user information
- `UserStats` - User statistics (XP, Level, Streak)
- `UserSkills` - User skills
- `Skills` - Available skills

#### Career Builder Module
- `CareerPlans` - Career plans
- `CareerSteps` - Career steps
- `CareerPlanSkills` - Required skills
- `StepCheckpoints` - Checkpoint tracking
- `LearningResources` - Learning resources

#### Study Scheduler Module
- `StudyPlans` - Study plans
- `AvailabilitySlots` - User availability
- `StudyTasks` - Study tasks
- `StudySessions` - Study sessions
- `TaskFeedbackLogs` - Task feedback logs

#### Engagement & Tracking
- `Achievements` - Available achievements
- `UserAchievements` - User achievements
- `DailyReflections` - Daily reflections
- `Notifications` - Notifications
- `AiRequestLogs` - AI request logs
- `UserDiagnosticResponses` - Diagnostic responses

---

## ApplicationDbContext Configuration

### Key Features Implemented:

1. **GUID Primary Keys** ?
   - All entities use `Guid` as primary key

2. **Soft Delete** ?
   - Deleted records are marked `IsDeleted = true` instead of physical deletion
   - Query Filter automatically excludes soft-deleted records

3. **Auto Timestamps** ?
   ```csharp
   CreatedAt - Set automatically on create
   UpdatedAt - Set automatically on update
   DeletedAt - Set automatically on soft delete
   ```

4. **Enum to String Conversion** ?
   ```csharp
   - StudyTask.Status ? string
   - StudyTask.Priority ? string
   - CareerStep.Status ? string
   - UserSkill.CurrentLevel ? string
   - CareerPlanSkill.TargetLevel ? string
   ```

---

## Background Services

### TokenCleanupService
**Purpose**: Automatically clean expired tokens

**Schedule**: Every 24 hours

**What it Cleans**:
1. `PasswordResetToken` - Expired for more than 7 days
2. `RefreshToken` - Revoked for more than 7 days

**Registration**: 
```csharp
builder.Services.AddHostedService<TokenCleanupService>();
```

---

## Setup Steps

### 1. Verify Connection String
```json
// appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=False"
}
```

### 2. Verify Migration Exists
```bash
cd Server/src/Mentora.Infrastructure
dotnet ef migrations list --startup-project ../Mentora.Api
```

**Expected Output**:
```
? 20251231070541_InitialMigration
```

### 3. Apply Migration
```bash
dotnet ef database update --startup-project ../Mentora.Api
```

**Expected Output**: `Done.`

### 4. Run Application
```bash
cd ../Mentora.Api
dotnet run
```

**Expected Output**:
```
? Token Cleanup Service started
? Now listening on: https://localhost:7xxx
? Swagger UI: https://localhost:7xxx/swagger
```

---

## Verification Checklist

### Database Structure
- [?] Migration created successfully
- [?] Database created (MentoraDb)
- [?] All tables created (25 tables)
- [?] Foreign keys configured
- [?] Indexes created

### ApplicationDbContext
- [?] DbSet properties for all entities
- [?] Relationships configured via Fluent API
- [?] Soft Delete Query Filter active
- [?] Auto Timestamps configured
- [?] Enum Conversions configured

### Background Services
- [?] TokenCleanupService registered
- [?] IServiceProvider Scoping correct
- [?] Logging configured

### Build & Compilation
- [?] Project builds successfully
- [?] No build errors
- [?] No warnings

---

## Important Notes

### 1. Soft Delete Behavior
Do NOT use:
```csharp
// Physical delete
context.Users.Remove(user);

// Use instead
entity.IsDeleted = true;
entity.DeletedAt = DateTime.UtcNow;
```

### 2. Token Cleanup
- Runs every 24 hours
- To change interval, edit `TokenCleanupService.cs`:
  ```csharp
  private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(24);
  ```

### 3. Cascade Delete Strategy
- User ? CareerPlans: **Cascade** (deleting user removes plans)
- User ? RefreshTokens: **Cascade** (deleting user removes tokens)
- User ? UserProfile: **Cascade** (deleting user removes profile)
- StudyTask ? CareerStep: **NoAction** (avoids cascade conflicts)

---

## Useful SQL Queries

### 1. List All Tables
```sql
USE MentoraDb;
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
```

### 2. View Row Counts
```sql
SELECT 
    t.NAME AS TableName,
    p.rows AS RowCounts
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE p.index_id < 2
ORDER BY t.NAME;
```

### 3. List All Foreign Keys
```sql
SELECT 
    fk.name AS ForeignKey,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fc 
    ON fk.object_id = fc.constraint_object_id
ORDER BY TableName;
```

---

## Summary

? **Database Setup is Complete and Verified**
- Database created successfully
- ApplicationDbContext configured correctly
- Background Services running properly
- Project builds successfully

**Last Updated**: December 31, 2024  
**Status**: ? Fully Operational
