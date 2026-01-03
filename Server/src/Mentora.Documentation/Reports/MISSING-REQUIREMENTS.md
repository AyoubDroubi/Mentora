# ? MISSING REQUIREMENTS - Mentora Platform

> **Document Version:** 1.0  
> **Last Updated:** 2025-01-02  
> **Priority:** High-Priority Missing Features

---

## Executive Summary

This document lists all **MISSING features** from the Software Requirements Specification (SRS) v1.0 that are **NOT YET IMPLEMENTED** in the Mentora platform.

**Critical Missing Features:**
- ? **Skills Portfolio Management** (SRS 2.3) - 70% Missing
- ? **SRS-Defined Study Planner** (SRS 4) - 100% Missing (Alternative exists)
- ? **Gamification System** (SRS 6) - 60% Missing
- ? **Admin Analytics** (SRS 7.2) - 100% Missing

---

## Module 2: User Profile & Personalization - Missing Features

### ? 2.3.1 UserProfileSkill Entity - Extended Properties

**Priority:** HIGH  
**SRS Reference:** 2.3.1.1

#### Missing Fields:

1. **AcquisitionMethod**
   - Type: `string` (max 200 chars)
   - Purpose: How the skill was acquired
   - Example: "Online course", "University", "Self-taught"

2. **StartedDate**
   - Type: `DateTime?` (nullable)
   - Purpose: When learning began
   - Used for: Timeline visualization

3. **YearsOfExperience**
   - Type: `int?` (nullable, 0-50 range)
   - Purpose: Total years of experience with skill
   - Validation: 0 ? value ? 50

4. **IsFeatured**
   - Type: `bool`
   - Purpose: Display on public profile
   - Default: `false`
   - Constraint: Maximum 10 featured skills per profile

5. **Notes**
   - Type: `string` (max 1000 chars)
   - Purpose: Personal notes about skill
   - Example: "Used in 3 major projects"

6. **DisplayOrder**
   - Type: `int`
   - Purpose: Ordering for featured skills showcase
   - Used for: Custom sorting of featured skills

**Recommended Entity Structure:**
```csharp
public class UserProfileSkill : BaseEntity
{
    // Existing
    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; }
    public SkillLevel ProficiencyLevel { get; set; }
    
    // MISSING - Add these fields:
    public string? AcquisitionMethod { get; set; } // max 200
    public DateTime? StartedDate { get; set; }
    public int? YearsOfExperience { get; set; } // 0-50
    public bool IsFeatured { get; set; } = false;
    public string? Notes { get; set; } // max 1000
    public int DisplayOrder { get; set; } = 0;
}
```

---

### ? 2.3.1.2 Unique Constraints

**Priority:** HIGH  
**SRS Reference:** 2.3.1.2

**Missing Database Constraint:**
```csharp
// In DbContext OnModelCreating:
modelBuilder.Entity<UserProfileSkill>()
    .HasIndex(ups => new { ups.UserProfileId, ups.SkillId })
    .IsUnique();
```

**Purpose:** Prevent duplicate skills per profile

---

### ? 2.3.1.3 Business Rules

**Priority:** MEDIUM  
**SRS Reference:** 2.3.1.3

**Missing Validations:**
1. Maximum 100 skills per profile
2. Maximum 10 featured skills per profile

**Recommended Service Logic:**
```csharp
public async Task<Result> AddSkillAsync(Guid userProfileId, AddSkillDto dto)
{
    // Check total skills count
    var skillsCount = await _repository.GetSkillsCountAsync(userProfileId);
    if (skillsCount >= 100)
        return Result.Fail("Maximum 100 skills per profile");
    
    // Check featured skills count if adding featured skill
    if (dto.IsFeatured)
    {
        var featuredCount = await _repository.GetFeaturedCountAsync(userProfileId);
        if (featuredCount >= 10)
            return Result.Fail("Maximum 10 featured skills");
    }
    
    // Add skill...
}
```

---

### ? 2.3.2 Skills CRUD Operations

**Priority:** HIGH  
**SRS Reference:** 2.3.2

#### Missing Endpoints:

##### 2.3.2.1 Add Skills
```http
POST /api/userprofile/skills
Content-Type: application/json
Authorization: Bearer {token}

{
  "skillId": "guid",
  "proficiencyLevel": 2,
  "acquisitionMethod": "Online Course",
  "startedDate": "2023-01-15",
  "yearsOfExperience": 2,
  "isFeatured": true,
  "notes": "Completed 5 projects with this skill"
}

Response 201:
{
  "success": true,
  "message": "Skill added successfully",
  "data": { ... }
}
```

```http
POST /api/userprofile/skills/bulk
Content-Type: application/json

{
  "skills": [
    { "skillId": "...", "proficiencyLevel": 2 },
    { "skillId": "...", "proficiencyLevel": 1 }
  ]
}

Response 200:
{
  "success": true,
  "added": 2,
  "failed": 0,
  "results": [ ... ]
}
```

##### 2.3.2.2 Retrieve Skills
```http
GET /api/userprofile/skills?proficiencyLevel=2&category=Technical&isFeatured=true&sort=proficiency

Response 200:
{
  "success": true,
  "data": [
    {
      "id": "...",
      "skillName": "Python",
      "category": "Technical",
      "proficiencyLevel": 2,
      "yearsOfExperience": 3,
      "isFeatured": true,
      "displayOrder": 1
    }
  ]
}
```

**Query Parameters:**
- `proficiencyLevel` (0-3): Filter by level
- `category`: Filter by category
- `isFeatured` (bool): Show only featured
- `sort`: Sort by (name, proficiency, experience, date)

##### 2.3.2.3 Update Skills
```http
PUT /api/userprofile/skills/{id}
Content-Type: application/json

{
  "proficiencyLevel": 3,
  "yearsOfExperience": 5,
  "notes": "Updated experience"
}
```

```http
PATCH /api/userprofile/skills/{id}
Content-Type: application/json

{
  "proficiencyLevel": 3
}
```

```http
PATCH /api/userprofile/skills/reorder
Content-Type: application/json

{
  "skillOrders": [
    { "skillId": "...", "displayOrder": 1 },
    { "skillId": "...", "displayOrder": 2 }
  ]
}
```

##### 2.3.2.4 Delete Skills
```http
DELETE /api/userprofile/skills/{id}

Response 200:
{
  "success": true,
  "message": "Skill removed successfully"
}
```

```http
DELETE /api/userprofile/skills/bulk
Content-Type: application/json

{
  "skillIds": ["guid1", "guid2"]
}

Response 200:
{
  "success": true,
  "removed": 2,
  "failed": 0
}
```

---

### ? 2.3.3 Skills Analytics & Intelligence

**Priority:** MEDIUM  
**SRS Reference:** 2.3.3

#### Missing Endpoints:

##### 2.3.3.1 Skills Summary
```http
GET /api/userprofile/skills/summary

Response 200:
{
  "totalSkills": 25,
  "proficiencyDistribution": {
    "beginner": 10,
    "intermediate": 10,
    "advanced": 4,
    "expert": 1
  },
  "categoryBreakdown": {
    "technical": 15,
    "soft": 8,
    "business": 2
  },
  "totalExperienceYears": 45,
  "featuredSkillsCount": 5
}
```

##### 2.3.3.2 Skills Distribution
```http
GET /api/userprofile/skills/distribution

Response 200:
{
  "byProficiency": {
    "beginner": 40,      // percentage
    "intermediate": 40,
    "advanced": 16,
    "expert": 4
  },
  "byCategory": {
    "technical": 60,
    "soft": 32,
    "business": 8
  }
}
```

##### 2.3.3.3 Skills Timeline
```http
GET /api/userprofile/skills/timeline

Response 200:
{
  "timeline": [
    {
      "year": 2023,
      "months": [
        {
          "month": 1,
          "skills": [
            { "name": "Python", "proficiency": "Beginner" }
          ]
        }
      ]
    }
  ]
}
```

##### 2.3.3.4 Skills Coverage Analysis
```http
GET /api/userprofile/skills/coverage

Response 200:
{
  "coverageScore": 65,
  "strongCategories": ["Technical", "Soft"],
  "weakCategories": ["Business", "Creative"],
  "recommendations": [
    "Add more business skills for well-rounded profile",
    "Consider learning project management"
  ],
  "gapAnalysis": {
    "technical": { "coverage": 80, "missing": ["DevOps", "Cloud"] },
    "soft": { "coverage": 70, "missing": ["Public Speaking"] }
  }
}
```

---

### ? 2.3.4 Profile Completion Integration

**Priority:** MEDIUM  
**SRS Reference:** 2.3.4

#### Missing Implementation:

##### 2.3.4.1 Skills Weight in Completion
Current implementation has basic completion percentage but doesn't include:

**Skills Section Weight (40%):**
- At least 3 skills: 20%
- At least 1 featured skill: 10%
- Skills with experience details: 10%

**Recommended Implementation:**
```csharp
public async Task<int> CalculateProfileCompletionAsync(Guid userId)
{
    int completion = 0;
    var profile = await GetProfileAsync(userId);
    
    // Basic fields (60%)
    if (!string.IsNullOrEmpty(profile.Bio)) completion += 10;
    if (!string.IsNullOrEmpty(profile.University)) completion += 15;
    if (!string.IsNullOrEmpty(profile.Major)) completion += 15;
    if (profile.ExpectedGraduationYear > 0) completion += 10;
    if (!string.IsNullOrEmpty(profile.Timezone)) completion += 10;
    
    // Skills section (40%)
    var skills = await GetUserSkillsAsync(userId);
    if (skills.Count >= 3) completion += 20;
    if (skills.Any(s => s.IsFeatured)) completion += 10;
    if (skills.Any(s => s.YearsOfExperience.HasValue)) completion += 10;
    
    return completion;
}
```

##### 2.3.4.2 Missing Fields Detection
```http
GET /api/userprofile/completion/missing

Response 200:
{
  "completionPercentage": 65,
  "missingFields": [
    {
      "field": "skills",
      "message": "Add at least 3 skills to your profile",
      "weight": 20,
      "priority": "high"
    },
    {
      "field": "featuredSkills",
      "message": "Mark at least 1 skill as featured",
      "weight": 10,
      "priority": "medium"
    }
  ],
  "recommendations": [
    "Complete your skills section to improve profile visibility",
    "Add years of experience to showcase expertise"
  ]
}
```

##### 2.3.4.3 Profile Strength Score
```http
GET /api/userprofile/strength

Response 200:
{
  "strengthScore": 72,
  "level": "Strong",
  "factors": {
    "completeness": 80,
    "skillsDiversity": 65,
    "experienceDepth": 70,
    "featuredQuality": 75
  },
  "insights": [
    "Your profile is stronger than 68% of users",
    "Add more diverse skills to reach 'Excellent' level"
  ]
}
```

---

### ? 2.3.5 Public Profile & Sharing

**Priority:** LOW  
**SRS Reference:** 2.3.5

#### Missing Endpoints:

##### 2.3.5.1 Public Profile View
```http
GET /api/userprofile/public/{userId}
# No authentication required

Response 200:
{
  "userId": "...",
  "firstName": "Ahmed",
  "lastName": "M.",
  "university": "...",
  "major": "...",
  "bio": "...",
  "featuredSkills": [
    {
      "name": "Python",
      "proficiency": "Advanced",
      "yearsOfExperience": 3
    }
  ],
  "socialLinks": {
    "linkedin": "...",
    "github": "..."
  }
}
```

**Security:** Only show non-sensitive info + featured skills

##### 2.3.5.2 Share Link Generation
```http
POST /api/userprofile/share
Content-Type: application/json

{
  "expirationDays": 7,
  "includeContact": false,
  "includeSkills": true
}

Response 200:
{
  "shareLink": "https://mentora.com/p/abc123xyz",
  "expiresAt": "2025-01-09T00:00:00Z",
  "accessToken": "abc123xyz"
}
```

##### 2.3.5.3 Profile Export
```http
GET /api/userprofile/export?includeSkills=true&format=json

Response 200:
{
  "profile": { ... },
  "skills": [ ... ],
  "exportedAt": "2025-01-02T14:30:00Z"
}
```

**Formats:** JSON, CSV, PDF (future)

---

### ? 2.3.6 Featured Skills Showcase

**Priority:** MEDIUM  
**SRS Reference:** 2.3.6

#### Missing Endpoints:

##### 2.3.6.1 Display Order Management
```http
PATCH /api/userprofile/skills/reorder
Content-Type: application/json

{
  "skillOrders": [
    { "skillId": "...", "displayOrder": 1 },
    { "skillId": "...", "displayOrder": 2 }
  ]
}
```

##### 2.3.6.2 Featured Toggle
```http
PATCH /api/userprofile/skills/{id}/featured
Content-Type: application/json

{
  "isFeatured": true
}

Response 200:
{
  "success": true,
  "message": "Skill featured status updated"
}
```

**Business Rule:** Max 10 featured skills

##### 2.3.6.3 Featured Retrieval
```http
GET /api/userprofile/skills/featured

Response 200:
{
  "featuredSkills": [
    {
      "id": "...",
      "name": "Python",
      "proficiency": "Advanced",
      "displayOrder": 1
    }
  ]
}
```

**Sorting:** By displayOrder ASC

---

### ? 2.3.7 Integration with Profile System

**Priority:** MEDIUM  
**SRS Reference:** 2.3.7

#### Missing Features:

##### 2.3.7.1 Profile with Skills
```http
GET /api/userprofile?includeSkills=true&featuredOnly=false

Response 200:
{
  "profile": { ... },
  "skills": [ ... ]  // ? Currently not included
}
```

**Current:** Profile endpoint doesn't include skills  
**Required:** Add query parameters to include skills

##### 2.3.7.2 Cascade Operations
**Current Status:** Partially configured

**Required Configurations:**
```csharp
// In DbContext:
modelBuilder.Entity<UserProfile>()
    .HasMany(up => up.Skills)
    .WithOne(ups => ups.UserProfile)
    .OnDelete(DeleteBehavior.Cascade); // ? Delete profile ? delete skills

modelBuilder.Entity<Skill>()
    .HasMany(s => s.UserProfileSkills)
    .WithOne(ups => ups.Skill)
    .OnDelete(DeleteBehavior.Restrict); // ? MISSING: Prevent skill deletion if referenced
```

---

### ? 2.3.8 Data Validation & Security

**Priority:** HIGH  
**SRS Reference:** 2.3.8

#### Missing Validations:

##### 2.3.8.1 Input Validation
**Required Validations:**
```csharp
public class AddSkillDto
{
    [Required]
    public Guid SkillId { get; set; }
    
    [Range(0, 3, ErrorMessage = "Proficiency level must be 0-3")]
    public int ProficiencyLevel { get; set; }
    
    [MaxLength(200)]
    public string? AcquisitionMethod { get; set; }
    
    [Range(0, 50, ErrorMessage = "Years of experience must be 0-50")]
    public int? YearsOfExperience { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}
```

##### 2.3.8.2 Authorization
**Required:** Ownership validation on all endpoints
```csharp
// Before any operation:
var profile = await _profileRepository.GetByUserIdAsync(userId);
if (profile == null || profile.UserId != currentUserId)
    return Forbid(); // 403 Forbidden
```

##### 2.3.8.3 Skill Existence Validation
```csharp
var skill = await _skillRepository.GetByIdAsync(dto.SkillId);
if (skill == null)
    return NotFound(new { message = "Skill not found" }); // 404
```

##### 2.3.8.4 Duplicate Prevention
```csharp
var existing = await _repository.GetUserSkillAsync(userProfileId, dto.SkillId);
if (existing != null)
    return Conflict(new { message = "Skill already added to profile" }); // 409
```

---

### ? 2.3.9 Performance Requirements

**Priority:** MEDIUM  
**SRS Reference:** 2.3.9

#### Missing Implementation:

##### 2.3.9.1 Response Times
**SRS Requirements:**
- GET requests: <200ms
- POST/PUT: <500ms
- Bulk operations (up to 50 skills): <1 second

**Current Status:** Not measured

**Recommended:**
- Add performance monitoring
- Add logging for slow queries
- Add response time middleware

##### 2.3.9.2 Database Optimization
**Missing Indexes:**
```csharp
// In DbContext OnModelCreating:
modelBuilder.Entity<UserProfileSkill>()
    .HasIndex(ups => ups.UserProfileId);

modelBuilder.Entity<UserProfileSkill>()
    .HasIndex(ups => ups.SkillId);

modelBuilder.Entity<UserProfileSkill>()
    .HasIndex(ups => ups.IsFeatured);

modelBuilder.Entity<UserProfileSkill>()
    .HasIndex(ups => ups.DisplayOrder);

modelBuilder.Entity<UserProfileSkill>()
    .HasIndex(ups => ups.ProficiencyLevel);
```

##### 2.3.9.3 Caching Strategy
**Missing Implementation:**
```csharp
// Example caching service:
public async Task<List<UserSkillDto>> GetUserSkillsCachedAsync(Guid userId)
{
    var cacheKey = $"user_skills_{userId}";
    
    var cached = await _cache.GetAsync<List<UserSkillDto>>(cacheKey);
    if (cached != null)
        return cached;
    
    var skills = await _repository.GetUserSkillsAsync(userId);
    await _cache.SetAsync(cacheKey, skills, TimeSpan.FromMinutes(10));
    
    return skills;
}

// Invalidate on update:
public async Task UpdateSkillAsync(UpdateSkillDto dto)
{
    await _repository.UpdateAsync(skill);
    await _cache.RemoveAsync($"user_skills_{skill.UserProfileId}");
}
```

---

## Module 3: AI Career Builder - Missing Features

### ? 3.3.3 Profile Skills Integration

**Priority:** MEDIUM  
**SRS Reference:** 3.3.3

#### Missing Features:

**Cross-reference CareerPlan skills with UserProfile skills:**
```csharp
public async Task<SkillGapAnalysisDto> AnalyzeSkillGapsAsync(Guid userId, Guid planId)
{
    // Get required skills from career plan
    var requiredSkills = await _careerPlanSkillRepo.GetByPlanIdAsync(planId);
    
    // Get user profile skills
    var userSkills = await _userSkillRepo.GetByUserIdAsync(userId);
    
    var gaps = new List<SkillGapDto>();
    
    foreach (var required in requiredSkills)
    {
        var userSkill = userSkills.FirstOrDefault(us => us.SkillId == required.SkillId);
        
        if (userSkill == null)
        {
            gaps.Add(new SkillGapDto
            {
                SkillName = required.Skill.Name,
                Status = "Missing",
                RequiredLevel = required.TargetLevel,
                CurrentLevel = null,
                Recommendation = "Start learning this skill"
            });
        }
        else if (userSkill.CurrentLevel < required.TargetLevel)
        {
            gaps.Add(new SkillGapDto
            {
                SkillName = required.Skill.Name,
                Status = "Insufficient",
                RequiredLevel = required.TargetLevel,
                CurrentLevel = userSkill.CurrentLevel,
                Recommendation = "Improve proficiency level"
            });
        }
    }
    
    return new SkillGapAnalysisDto { Gaps = gaps };
}
```

**Missing Endpoint:**
```http
GET /api/career-plans/{planId}/skill-gaps

Response 200:
{
  "totalRequired": 15,
  "acquired": 8,
  "missing": 5,
  "insufficient": 2,
  "gaps": [
    {
      "skillName": "Docker",
      "status": "Missing",
      "requiredLevel": "Intermediate",
      "currentLevel": null,
      "recommendation": "Start learning this skill"
    }
  ]
}
```

---

## Module 4: Intelligent Study Planner - Missing Features

### ?? Important Note:
The SRS defines a **complex availability-based scheduling system**, but the current implementation provides a **simpler Study Planner** with different features (ToDo, Events, Notes, Sessions).

**Decision Required:**
1. Implement SRS-defined features alongside current system
2. Replace current system with SRS features
3. Document current system as alternative approach

---

### ? 4.1 Constraints & Availability Configuration

**Priority:** HIGH (if SRS approach required)  
**SRS Reference:** 4.1

#### Missing Implementation:

##### 4.1.1 Granular Slots Definition
**Entity exists but not used:**
```csharp
public class AvailabilitySlot : BaseEntity
{
    public Guid StudyPlanId { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public EnergyLevel EnergyLevel { get; set; }
}

public enum EnergyLevel
{
    Low,
    High
}
```

**Missing Endpoints:**
```http
POST /api/study-plans/{id}/availability
Content-Type: application/json

{
  "slots": [
    {
      "day": 1,  // Monday
      "startTime": "08:00:00",
      "endTime": "10:00:00",
      "energyLevel": "High"
    }
  ]
}
```

```http
GET /api/study-plans/{id}/availability

Response 200:
{
  "slots": [ ... ]
}
```

##### 4.1.2 Energy Level Tagging
**Feature defined but not implemented:**
- High-cognitive tasks (Math, Coding) ? High-Energy slots
- Low-cognitive tasks (Reading) ? Low-Energy slots

---

### ? 4.2 Dynamic Scheduling Algorithm

**Priority:** HIGH (if SRS approach required)  
**SRS Reference:** 4.2

#### Missing Features:

##### 4.2.1 Constraint Satisfaction Logic
**Missing Service:**
```csharp
public class TaskSchedulingService
{
    public async Task<ScheduleResult> AllocateTasksAsync(Guid studyPlanId)
    {
        var plan = await _planRepo.GetWithSlotsAsync(studyPlanId);
        var tasks = await _taskRepo.GetUnscheduledAsync(studyPlanId);
        
        foreach (var task in tasks.OrderByDescending(t => t.Priority))
        {
            var suitableSlot = FindSuitableSlot(plan.AvailabilitySlots, task);
            
            if (suitableSlot != null)
            {
                task.ScheduledDate = suitableSlot.Day;
                task.StartTime = suitableSlot.StartTime;
                await _taskRepo.UpdateAsync(task);
            }
        }
    }
    
    private AvailabilitySlot? FindSuitableSlot(
        IEnumerable<AvailabilitySlot> slots, 
        StudyTask task)
    {
        return slots.FirstOrDefault(s =>
            s.GetCapacityMinutes() >= task.DurationMinutes &&
            (task.Priority != TaskPriority.High || s.EnergyLevel == EnergyLevel.High)
        );
    }
}
```

##### 4.2.2 Conflict Detection
**Missing Validation:**
```csharp
public async Task<bool> HasConflictAsync(DateTime date, TimeSpan start, TimeSpan end)
{
    var existingTasks = await _taskRepo.GetByDateAsync(date);
    
    return existingTasks.Any(t =>
        t.StartTime < end && start < (t.StartTime + TimeSpan.FromMinutes(t.DurationMinutes))
    );
}
```

---

### ?? 4.3 Task Execution & Session Tracking

**Priority:** MEDIUM  
**SRS Reference:** 4.3

#### Partially Implemented:

##### ? 4.3.1 Micro-Interaction Tracking
**Status:** Implemented

##### ? 4.3.2 Focus Score Calculation
**Status:** Implemented in StudySession

##### ? 4.3.3 Feedback Loop Integration
**Entity exists but not used:**
```csharp
public class TaskFeedbackLog : BaseEntity
{
    public Guid StudyTaskId { get; set; }
    public string FailureReason { get; set; }
    public string Mood { get; set; }
    public string UserComment { get; set; }
}
```

**Missing Feature:**
- Force creation when task marked as Skipped/Failed
- Use for future schedule optimization

---

## Module 5: Deep Integration - Missing Features

### ? 5.1.2 Checkpoint Synchronization

**Priority:** MEDIUM  
**SRS Reference:** 5.1.2

**Missing Logic:**
```csharp
public async Task CompleteTaskAsync(Guid taskId)
{
    var task = await _taskRepo.GetByIdAsync(taskId);
    task.Status = TaskStatus.Done;
    await _taskRepo.UpdateAsync(task);
    
    // MISSING: Auto-toggle linked checkpoint
    if (task.CareerStepId.HasValue)
    {
        var checkpoint = await _checkpointRepo.GetLinkedAsync(task.Id);
        if (checkpoint != null)
        {
            checkpoint.IsCompleted = true;
            await _checkpointRepo.UpdateAsync(checkpoint);
        }
    }
}
```

---

### ? 5.2.1 Upward Bubbling - Complete Implementation

**Priority:** HIGH  
**SRS Reference:** 5.2.1

**Currently Partially Implemented (Skills ? Steps ? Plan)**

**Missing:**
- StudyTask completion ? CareerStep progress
- Event-based architecture

**Recommended Full Implementation:**
```csharp
public async Task OnTaskCompletedAsync(Guid taskId)
{
    var task = await _taskRepo.GetByIdWithStepAsync(taskId);
    
    if (task.CareerStepId.HasValue)
    {
        // Update step progress
        var step = task.CareerStep;
        var stepTasks = await _taskRepo.GetByStepIdAsync(step.Id);
        var completedCount = stepTasks.Count(t => t.Status == TaskStatus.Done);
        
        step.ProgressPercentage = (completedCount * 100) / stepTasks.Count;
        await _stepRepo.UpdateAsync(step);
        
        // Update plan progress
        var plan = await _planRepo.GetByIdWithStepsAsync(step.CareerPlanId);
        plan.ProgressPercentage = (int)plan.Steps.Average(s => s.ProgressPercentage);
        await _planRepo.UpdateAsync(plan);
    }
}
```

---

### ? 5.2.2 Impact Visualization

**Priority:** LOW  
**SRS Reference:** 5.2.2

**Missing Endpoint:**
```http
GET /api/study-tasks/{taskId}/impact

Response 200:
{
  "taskTitle": "Complete Python Course",
  "linkedTo": {
    "step": "Learn Backend Development",
    "plan": "Full Stack Developer Path"
  },
  "contribution": {
    "toStep": "15%",
    "toPlan": "2%"
  },
  "message": "Completing this task contributes 2% to your Full Stack Developer Goal"
}
```

---

### ? 5.3 Skills-Career Alignment

**Priority:** MEDIUM  
**SRS Reference:** 5.3

#### Missing Features:

##### 5.3.1 Skill Progress Tracking
**Missing Logic:**
```csharp
public async Task OnStepCompletedAsync(Guid stepId)
{
    var step = await _stepRepo.GetByIdWithSkillsAsync(stepId);
    
    foreach (var skill in step.Skills)
    {
        // Update UserProfile skill proficiency
        var userSkill = await _userSkillRepo.GetBySkillIdAsync(userId, skill.SkillId);
        
        if (userSkill != null)
        {
            // Increase proficiency level
            if (userSkill.CurrentLevel < SkillLevel.Expert)
            {
                userSkill.CurrentLevel++;
                await _userSkillRepo.UpdateAsync(userSkill);
            }
        }
        else
        {
            // Add skill to user profile
            await _userSkillRepo.CreateAsync(new UserSkill
            {
                UserId = userId,
                SkillId = skill.SkillId,
                CurrentLevel = SkillLevel.Beginner
            });
        }
    }
}
```

##### 5.3.2 Achievement Triggers
**Missing Implementation:**
- Trigger achievements on skill milestones
- Example: "Mastered 5 skills", "Expert in Python"

---

## Module 6: Gamification & Engagement - Missing Features

### ? 6.1.1 Event-Based Experience

**Priority:** MEDIUM  
**SRS Reference:** 6.1.1

**Entity exists but XP logic not implemented:**

**Missing Event Bus:**
```csharp
public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
}

public class TaskCompletedEvent : IEvent
{
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }
    public int XpReward { get; set; }
}

// Event Handler:
public class XpAwardHandler : IEventHandler<TaskCompletedEvent>
{
    public async Task HandleAsync(TaskCompletedEvent @event)
    {
        var stats = await _statsRepo.GetByUserIdAsync(@event.UserId);
        stats.TotalXP += @event.XpReward;
        stats.Level = CalculateLevel(stats.TotalXP);
        await _statsRepo.UpdateAsync(stats);
    }
}
```

**Missing XP Awards:**
- Task completed: 10 XP
- Career step completed: 50 XP
- Career plan completed: 200 XP
- Skill added: 5 XP
- Skill proficiency increased: 10 XP
- Featured skill: 15 XP

---

### ? 6.1.2 Streak Maintenance

**Priority:** MEDIUM  
**SRS Reference:** 6.1.2

**Missing Logic:**
```csharp
public async Task UpdateStreakAsync(Guid userId)
{
    var stats = await _statsRepo.GetByUserIdAsync(userId);
    var user = await _userRepo.GetByIdAsync(userId);
    
    var hoursSinceLastLogin = (DateTime.UtcNow - user.LastLoginAt).TotalHours;
    
    if (hoursSinceLastLogin > 48) // 24 hours + offset
    {
        stats.CurrentStreak = 0; // Reset streak
    }
    else if (hoursSinceLastLogin > 24)
    {
        stats.CurrentStreak++; // Increment streak
    }
    
    await _statsRepo.UpdateAsync(stats);
}
```

---

### ? 6.2 Achievement System

**Priority:** MEDIUM  
**SRS Reference:** 6.2

#### Missing Implementation:

##### 6.2.1 Trigger Conditions
**Entities exist but no evaluation logic:**

**Missing Service:**
```csharp
public class AchievementService
{
    public async Task EvaluateAchievementsAsync(Guid userId)
    {
        var stats = await _statsRepo.GetByUserIdAsync(userId);
        var achievements = await _achievementRepo.GetAllAsync();
        
        foreach (var achievement in achievements)
        {
            var hasAchievement = await _userAchievementRepo.HasAsync(userId, achievement.Id);
            if (hasAchievement) continue;
            
            var earned = await EvaluateTriggerAsync(userId, achievement.TriggerKey);
            if (earned)
            {
                await AwardAchievementAsync(userId, achievement);
            }
        }
    }
    
    private async Task<bool> EvaluateTriggerAsync(Guid userId, string triggerKey)
    {
        return triggerKey switch
        {
            "7_day_streak" => await Has7DayStreakAsync(userId),
            "first_plan" => await HasCompletedFirstPlanAsync(userId),
            "master_5_skills" => await HasMastered5SkillsAsync(userId),
            _ => false
        };
    }
}
```

##### 6.2.2 Skills Achievements
**Missing Achievements:**
- "Master 5 Skills" - Reach Expert level in 5 skills
- "Featured Showcase" - Have 5 featured skills
- "Full Stack Expert" - Have both frontend + backend skills at Advanced
- "Skill Collector" - Acquire skills across 5 categories
- "Experience Master" - Total 50+ years of experience across all skills

---

### ? 6.3 Emotional Reflection

**Priority:** LOW  
**SRS Reference:** 6.3

**Entity exists but no endpoints:**
```csharp
public class DailyReflection : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public int SatisfactionScore { get; set; } // 1-10
    public string Summary { get; set; }
}
```

**Missing Endpoints:**
```http
POST /api/reflections
Content-Type: application/json

{
  "date": "2025-01-02",
  "satisfactionScore": 8,
  "summary": "Productive day, completed 3 tasks"
}
```

```http
GET /api/reflections?startDate=2025-01-01&endDate=2025-01-31

Response 200:
{
  "reflections": [ ... ],
  "averageSatisfaction": 7.5,
  "sentiment": "Positive"
}
```

---

## Module 7: System Monitoring - Missing Features

### ? 7.2 Skills Analytics Dashboard

**Priority:** LOW  
**SRS Reference:** 7.2

#### Missing Features:

##### 7.2.1 System-Wide Analytics
**Missing Admin Endpoints:**
```http
GET /api/admin/analytics/skills/trending

Response 200:
{
  "trendingSkills": [
    { "name": "Python", "usersCount": 1250, "trend": "up" },
    { "name": "React", "usersCount": 980, "trend": "up" }
  ]
}
```

```http
GET /api/admin/analytics/skills/most-acquired

Response 200:
{
  "topSkills": [
    { "name": "Python", "count": 1500 },
    { "name": "JavaScript", "count": 1200 }
  ]
}
```

```http
GET /api/admin/analytics/skills/proficiency

Response 200:
{
  "averageProficiency": {
    "Python": 2.3,  // Average level (0-3)
    "JavaScript": 1.8
  }
}
```

##### 7.2.2 Profile Completion Metrics
**Missing Endpoints:**
```http
GET /api/admin/analytics/profiles/completion

Response 200:
{
  "averageCompletion": 65,
  "distribution": {
    "0-25%": 150,
    "26-50%": 300,
    "51-75%": 450,
    "76-100%": 600
  },
  "skillsAdoptionRate": 72
}
```

---

## Module 8: Data Integrity - Missing Features

### ? 8.3 Delete Cascades - Complete Configuration

**Priority:** MEDIUM  
**SRS Reference:** 8.3

**Missing Configurations:**
```csharp
// In DbContext OnModelCreating:

// SetNull: For history preservation
modelBuilder.Entity<CareerStep>()
    .HasMany(cs => cs.StudyTasks)
    .WithOne(st => st.CareerStep)
    .OnDelete(DeleteBehavior.SetNull); // ? MISSING

// Restrict: For shared resources
modelBuilder.Entity<Skill>()
    .HasMany(s => s.UserProfileSkills)
    .WithOne(ups => ups.Skill)
    .OnDelete(DeleteBehavior.Restrict); // ? MISSING

modelBuilder.Entity<Skill>()
    .HasMany(s => s.CareerPlanSkills)
    .WithOne(cps => cps.Skill)
    .OnDelete(DeleteBehavior.Restrict); // ? MISSING
```

---

## Module 9: API Design - Missing Features

### ? 9.3 Bulk Operations

**Priority:** MEDIUM  
**SRS Reference:** 9.3

**Missing Endpoints:**
```http
POST /api/userprofile/skills/bulk
DELETE /api/userprofile/skills/bulk
POST /api/study-tasks/bulk
DELETE /api/study-tasks/bulk
```

**Response Format:**
```json
{
  "success": true,
  "successCount": 8,
  "failedCount": 2,
  "results": [
    { "item": "...", "status": "success" },
    { "item": "...", "status": "failed", "reason": "Duplicate" }
  ]
}
```

---

### ? 9.4 Pagination Support

**Priority:** HIGH  
**SRS Reference:** 9.4

**Missing Implementation:**
```csharp
public class PaginatedResult<T>
{
    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}
```

**Required for Endpoints:**
```http
GET /api/career-plans?page=1&pageSize=10
GET /api/userprofile/skills?page=1&pageSize=20
GET /api/todo?page=1&pageSize=50
```

---

## Priority Summary

### ?? HIGH Priority (Must Implement)

1. **Skills Portfolio Management (SRS 2.3)**
   - UserProfileSkill extended fields
   - Skills CRUD endpoints
   - Unique constraints
   - Input validation

2. **Profile Skills Integration (SRS 3.3.3)**
   - Cross-reference with career plan skills
   - Skill gap analysis

3. **Progress Propagation (SRS 5.2)**
   - Complete upward bubbling logic
   - StudyTask ? Step ? Plan

4. **Pagination (SRS 9.4)**
   - All list endpoints

### ?? MEDIUM Priority (Should Implement)

1. **Skills Analytics (SRS 2.3.3)**
   - Summary, distribution, timeline

2. **Profile Completion (SRS 2.3.4)**
   - Skills weight in calculation
   - Missing fields detection

3. **Featured Skills (SRS 2.3.6)**
   - Display order management
   - Featured toggle

4. **Gamification (SRS 6)**
   - XP system
   - Achievements
   - Streak maintenance

5. **Database Optimization (SRS 2.3.9)**
   - Indexes
   - Caching

### ?? LOW Priority (Nice to Have)

1. **Public Profile (SRS 2.3.5)**
   - Share links
   - Export

2. **Study Planner SRS Features (SRS 4)**
   - Availability slots
   - Scheduling algorithm

3. **Reflections (SRS 6.3)**
   - Daily reflections API

4. **Admin Analytics (SRS 7.2)**
   - System-wide metrics

---

## Recommended Implementation Order

### Phase 1: Core Missing Features (2-3 weeks)
1. UserProfileSkill extended entity
2. Skills CRUD endpoints
3. Input validation and security
4. Profile-Career skills integration
5. Pagination support

### Phase 2: Analytics & Intelligence (1-2 weeks)
1. Skills summary and distribution
2. Profile completion with skills weight
3. Featured skills management
4. Timeline visualization

### Phase 3: Gamification (2-3 weeks)
1. Event bus for XP
2. Achievement evaluation logic
3. Streak maintenance
4. Skills-based achievements

### Phase 4: Advanced Features (1-2 weeks)
1. Public profile and sharing
2. Profile export
3. Admin analytics dashboard
4. Database optimization

### Phase 5: Study Planner (Optional, 3-4 weeks)
1. Decision: Keep current or implement SRS
2. If SRS: Availability slots + scheduling algorithm
3. Integration with existing features

---

## Total Estimated Effort

- **High Priority:** 4-5 weeks
- **Medium Priority:** 4-5 weeks
- **Low Priority:** 2-3 weeks

**Total:** 10-13 weeks for complete SRS implementation

---

**End of Missing Requirements Report**
