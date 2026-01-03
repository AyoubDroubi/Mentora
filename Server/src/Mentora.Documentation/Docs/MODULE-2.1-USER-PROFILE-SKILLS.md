# Module 2.1: User Profile Skills - Implementation Guide

## Overview
Extension of Module 2 (User Profile) to add comprehensive skills management functionality. Users can showcase their technical and soft skills with proficiency levels, experience tracking, and featured display.

---

## Features Implemented

### Skills on Profile
- **Add Skills**: Link skills from Skills library to user profile
- **Proficiency Levels**: Track skill level (Beginner ? Intermediate ? Advanced ? Expert)
- **Acquisition Methods**: Record how skill was acquired (Courses, Work, Self-taught)
- **Experience Tracking**: Years of experience and start date
- **Featured Skills**: Showcase top skills on profile
- **Display Ordering**: Custom order for featured skills
- **Bulk Operations**: Add/Remove multiple skills at once

### Skills Analytics
- **Skills Summary**: Total counts, distribution by proficiency/category
- **Timeline View**: When skills were acquired
- **Experience Calculation**: Total years across all skills
- **Coverage Analysis**: Skills gaps and recommendations
- **Profile Strength**: Score based on skills quality and diversity

### Public Profile
- **Featured Skills Display**: Show selected skills on public profile
- **Share Links**: Generate time-limited profile share URLs
- **Privacy Controls**: Control what information is public
- **Professional Showcase**: Profile suitable for sharing with employers

---

## Architecture

### Domain Layer
```
Entities/
??? UserProfileSkill.cs
    ??? UserProfileId (FK to UserProfile)
    ??? SkillId (FK to Skill)
    ??? ProficiencyLevel (0-3: Beginner to Expert)
    ??? AcquisitionMethod (string)
    ??? StartedDate (DateTime?)
    ??? YearsOfExperience (int?)
    ??? IsFeatured (bool)
    ??? Notes (string)
    ??? DisplayOrder (int)

UserProfile.cs
??? Skills (ICollection<UserProfileSkill>)
```

### Application Layer
```
DTOs/UserProfile/
??? AddProfileSkillDto.cs           # Add single skill
??? AddMultipleProfileSkillsDto.cs  # Bulk add
??? UpdateProfileSkillDto.cs        # Update skill
??? UserProfileSkillResponseDto.cs  # Skill details
??? SkillsSummaryDto.cs             # Statistics

Services/
??? UserProfileSkillService.cs      # Business logic
```

### Infrastructure Layer
```
Repositories/
??? UserProfileSkillRepository.cs
    ??? CRUD operations
    ??? Filtering (proficiency, category, featured)
    ??? Statistics queries
    ??? Timeline queries

Database Configuration/
??? ApplicationDbContext.cs
    ??? UserProfileSkill entity configuration
```

### API Layer
```
Controllers/
??? UserProfileController.cs
    ??? POST   /api/userprofile/skills
    ??? POST   /api/userprofile/skills/bulk
    ??? GET    /api/userprofile/skills
    ??? GET    /api/userprofile/skills/featured
    ??? GET    /api/userprofile/skills/{id}
    ??? PUT    /api/userprofile/skills/{id}
    ??? PATCH  /api/userprofile/skills/{id}/proficiency
    ??? PATCH  /api/userprofile/skills/{id}/featured
    ??? PATCH  /api/userprofile/skills/reorder
    ??? DELETE /api/userprofile/skills/{id}
    ??? DELETE /api/userprofile/skills/bulk
    ??? GET    /api/userprofile/skills/summary
    ??? GET    /api/userprofile/skills/distribution
    ??? GET    /api/userprofile/skills/timeline
    ??? GET    /api/userprofile?includeSkills=true
    ??? GET    /api/userprofile/public/{userId}
```

---

## Database Schema

### UserProfileSkills Table
```sql
CREATE TABLE UserProfileSkills (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserProfileId UNIQUEIDENTIFIER NOT NULL,
    SkillId UNIQUEIDENTIFIER NOT NULL,
    ProficiencyLevel INT NOT NULL DEFAULT 0,
    AcquisitionMethod NVARCHAR(200),
    StartedDate DATETIME2,
    YearsOfExperience INT,
    IsFeatured BIT NOT NULL DEFAULT 0,
    Notes NVARCHAR(1000),
    DisplayOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    
    CONSTRAINT FK_UserProfileSkills_UserProfile 
        FOREIGN KEY (UserProfileId) REFERENCES UserProfiles(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserProfileSkills_Skill 
        FOREIGN KEY (SkillId) REFERENCES Skills(Id) ON DELETE RESTRICT,
    CONSTRAINT UQ_UserProfileSkills_UserProfileId_SkillId 
        UNIQUE (UserProfileId, SkillId)
);

-- Indexes
CREATE INDEX IX_UserProfileSkills_UserProfileId ON UserProfileSkills(UserProfileId);
CREATE INDEX IX_UserProfileSkills_IsFeatured ON UserProfileSkills(IsFeatured) WHERE IsFeatured = 1;
```

### Migration
```bash
cd Server/src/Mentora.Infrastructure
dotnet ef migrations add AddUserProfileSkills --startup-project ../Mentora.Api
dotnet ef database update --startup-project ../Mentora.Api
```

---

## API Examples

### 1. Add Skill to Profile
```http
POST /api/userprofile/skills
Authorization: Bearer {token}
Content-Type: application/json

{
  "skillId": "550e8400-e29b-41d4-a716-446655440000",
  "proficiencyLevel": 2,
  "acquisitionMethod": "Online Courses & Self-Learning",
  "startedDate": "2023-01-15T00:00:00Z",
  "yearsOfExperience": 2,
  "isFeatured": true,
  "notes": "Built multiple React applications",
  "displayOrder": 1
}

Response: 201 Created
{
  "id": "guid",
  "skill": {
    "id": "guid",
    "name": "React",
    "category": "Frontend"
  },
  "proficiencyLevel": 2,
  "proficiencyLevelName": "Advanced",
  "acquisitionMethod": "Online Courses & Self-Learning",
  "yearsOfExperience": 2,
  "isFeatured": true,
  "createdAt": "2025-01-20T10:00:00Z"
}
```

### 2. Get Profile with Skills
```http
GET /api/userprofile?includeSkills=true&featuredOnly=false
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "guid",
  "bio": "CS Student passionate about web development",
  "university": "University of Jordan",
  "major": "Computer Science",
  ...
  "skills": [
    {
      "id": "guid",
      "skill": { "name": "React", "category": "Frontend" },
      "proficiencyLevel": 2,
      "proficiencyLevelName": "Advanced",
      "yearsOfExperience": 2,
      "isFeatured": true,
      "displayOrder": 1
    }
  ],
  "featuredSkills": [
    // Only featured skills, sorted by displayOrder
  ]
}
```

### 3. Get Skills Summary
```http
GET /api/userprofile/skills/summary
Authorization: Bearer {token}

Response: 200 OK
{
  "totalSkills": 10,
  "featuredSkills": 5,
  "byProficiency": {
    "Beginner": 3,
    "Intermediate": 4,
    "Advanced": 2,
    "Expert": 1
  },
  "byCategory": {
    "Frontend": 3,
    "Backend": 2,
    "Database": 2,
    "DevOps": 1
  },
  "totalYearsOfExperience": 15
}
```

### 4. Update Skill Proficiency
```http
PATCH /api/userprofile/skills/{id}/proficiency
Authorization: Bearer {token}
Content-Type: application/json

{
  "proficiencyLevel": 3
}

Response: 200 OK
```

### 5. Reorder Featured Skills
```http
PATCH /api/userprofile/skills/reorder
Authorization: Bearer {token}
Content-Type: application/json

{
  "skillOrders": [
    { "profileSkillId": "guid1", "displayOrder": 1 },
    { "profileSkillId": "guid2", "displayOrder": 2 },
    { "profileSkillId": "guid3", "displayOrder": 3 }
  ]
}

Response: 200 OK
```

### 6. Get Public Profile
```http
GET /api/userprofile/public/{userId}

Response: 200 OK
{
  "firstName": "John",
  "lastName": "Doe",
  "bio": "CS Student",
  "university": "University of Jordan",
  "featuredSkills": [
    {
      "name": "React",
      "proficiencyLevel": "Advanced",
      "yearsOfExperience": 2
    }
  ]
}
```

---

## Business Logic

### Proficiency Level Mapping
```csharp
public enum SkillLevel
{
    Beginner = 0,      // 0-1 years or just starting
    Intermediate = 1,   // 1-2 years with projects
    Advanced = 2,       // 2-5 years with extensive use
    Expert = 3          // 5+ years, can teach others
}
```

### Profile Completion Calculation
Skills contribute 40% to profile completion:
- Base profile fields: 60%
- Skills section: 40%
  - At least 3 skills: 20%
  - At least 1 featured skill: 10%
  - Skills with experience details: 10%

### Featured Skills Rules
- Maximum 10 featured skills per profile
- Featured skills shown on public profile
- Sorted by displayOrder (ascending)
- If no order, use createdAt

### Validation Rules
- **SkillId**: Must exist in Skills table
- **ProficiencyLevel**: 0-3 range
- **YearsOfExperience**: 0-50 range if provided
- **AcquisitionMethod**: Max 200 characters
- **Notes**: Max 1000 characters
- **Unique Constraint**: One skill per profile (UserProfileId + SkillId)

---

## Integration with Other Modules

### Module 1: Authentication
- All endpoints require JWT authentication
- UserId extracted from token claims

### Module 3: Career Builder
- Career Plans can link skills (CareerPlanSkill)
- Profile skills are independent baseline
- AI can suggest skills based on profile skills

### Module 6: Gamification
- Award XP for adding skills
- Badges for skill milestones (e.g., "10 Skills", "5 Expert Skills")
- Achievements for skill diversity

---

## Frontend Integration

### Profile Page Updates
```jsx
// Client/src/pages/Profile.jsx

// Display skills section
<div className="skills-section">
  <h3>My Skills</h3>
  <div className="skills-grid">
    {profile.featuredSkills.map(skill => (
      <SkillCard 
        key={skill.id}
        skill={skill}
        featured={true}
      />
    ))}
  </div>
  
  <button onClick={handleAddSkill}>Add Skill</button>
</div>

// Add Skill Modal
<AddSkillModal
  onAdd={async (skillData) => {
    await userProfileService.addProfileSkill(skillData);
    await refreshProfile();
  }}
/>
```

### Skills Management Page
```jsx
// Client/src/pages/ProfileSkills.jsx

export default function ProfileSkills() {
  const [skills, setSkills] = useState([]);
  const [summary, setSummary] = useState(null);

  // Fetch skills
  useEffect(() => {
    loadSkills();
    loadSummary();
  }, []);

  // Render skills list with filters
  return (
    <div>
      <SkillsSummary data={summary} />
      <SkillsFilters onChange={applyFilters} />
      <SkillsList 
        skills={skills}
        onUpdate={handleUpdate}
        onRemove={handleRemove}
        onReorder={handleReorder}
      />
    </div>
  );
}
```

### Public Profile View
```jsx
// Client/src/pages/PublicProfile.jsx

export default function PublicProfile({ userId }) {
  const [profile, setProfile] = useState(null);

  useEffect(() => {
    loadPublicProfile(userId);
  }, [userId]);

  return (
    <div className="public-profile">
      <div className="profile-header">
        <img src={profile.avatarUrl} alt="avatar" />
        <h1>{profile.firstName} {profile.lastName}</h1>
        <p>{profile.university} - {profile.major}</p>
      </div>

      <div className="featured-skills">
        <h2>Skills</h2>
        {profile.featuredSkills.map(skill => (
          <SkillBadge key={skill.name} skill={skill} />
        ))}
      </div>
    </div>
  );
}
```

---

## Testing

### HTTP Test File
Location: `Tests/UserProfile/userprofile-with-skills-tests.http`

**Test Coverage** (80+ requests):
- ? Authentication (3 tests)
- ? Profile Management (6 tests)
- ? Timezone Management (6 tests)
- ? Create Skills (5 tests)
- ? Profile Skills CRUD (9 tests)
- ? Update Profile Skills (5 tests)
- ? Skills Statistics (4 tests)
- ? Profile with Skills (3 tests)
- ? Remove Skills (3 tests)
- ? Validation Tests (7 tests)
- ? Public Profile (3 tests)
- ? Completion & Analytics (4 tests)

### Running Tests
1. Start application: `dotnet run` (API)
2. Open `userprofile-with-skills-tests.http` in VS Code
3. Install REST Client extension
4. Run tests sequentially (PHASE 1 ? PHASE 12)

### Key Test Scenarios
```http
# Scenario 1: Complete Profile Setup with Skills
Register ? Login ? Create Profile ? Add Skills ? View Profile

# Scenario 2: Skills Management
Add Skills ? Update Proficiency ? Mark Featured ? Reorder ? View Stats

# Scenario 3: Featured Skills Showcase
Mark as Featured ? Reorder Display ? Get Featured Only ? Public View

# Scenario 4: Validation Tests
Invalid Skill ID ? Duplicate Skill ? Invalid Proficiency ? Max Skills
```

---

## Deployment Checklist

### Database Migration
- [ ] Run migration: `dotnet ef database update`
- [ ] Verify UserProfileSkills table created
- [ ] Verify indexes created
- [ ] Verify constraints working

### Backend Deployment
- [ ] UserProfileSkill entity configured
- [ ] UserProfileSkillService implemented
- [ ] UserProfileSkillRepository implemented
- [ ] Controller endpoints added
- [ ] DTOs created
- [ ] Validation implemented
- [ ] Error handling complete

### Frontend Deployment
- [ ] Profile page updated with skills section
- [ ] Add Skill functionality
- [ ] Update/Remove skill functionality
- [ ] Featured skills toggle
- [ ] Reorder functionality
- [ ] Skills statistics page
- [ ] Public profile view
- [ ] Skills in profile export

### Testing
- [ ] All HTTP tests passing
- [ ] Unit tests for business logic
- [ ] Integration tests for API
- [ ] E2E tests for user flows

### Documentation
- [ ] API documentation updated
- [ ] Requirements document created
- [ ] Implementation guide complete
- [ ] Frontend integration guide
- [ ] Test documentation

---

## Performance Optimization

### Database Queries
```csharp
// Eager loading for skills with skill details
var profile = await _context.UserProfiles
    .Include(p => p.Skills)
        .ThenInclude(ps => ps.Skill)
    .Where(p => p.UserId == userId)
    .FirstOrDefaultAsync();

// Filtered query for featured skills only
var featuredSkills = await _context.UserProfileSkills
    .Include(ps => ps.Skill)
    .Where(ps => ps.UserProfileId == profileId && ps.IsFeatured)
    .OrderBy(ps => ps.DisplayOrder)
    .ToListAsync();
```

### Caching Strategy
```csharp
// Cache featured skills for public profiles
var cacheKey = $"profile:skills:featured:{userId}";
var cachedSkills = await _cache.GetAsync<List<UserProfileSkillDto>>(cacheKey);

if (cachedSkills == null)
{
    cachedSkills = await LoadFeaturedSkills(userId);
    await _cache.SetAsync(cacheKey, cachedSkills, TimeSpan.FromMinutes(30));
}
```

### Pagination
```csharp
// For profiles with many skills
public async Task<PagedResult<UserProfileSkillDto>> GetSkillsAsync(
    Guid userId, 
    int pageNumber = 1, 
    int pageSize = 20)
{
    var query = _context.UserProfileSkills
        .Include(ps => ps.Skill)
        .Where(ps => ps.UserProfile.UserId == userId);

    var total = await query.CountAsync();
    var skills = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new PagedResult<UserProfileSkillDto>
    {
        Items = skills.Select(MapToDto),
        TotalCount = total,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
}
```

---

## Security Considerations

### Authorization
```csharp
// Verify skill belongs to current user before modification
private async Task<bool> CanModifySkill(Guid profileSkillId, Guid userId)
{
    return await _context.UserProfileSkills
        .AnyAsync(ps => ps.Id == profileSkillId && ps.UserProfile.UserId == userId);
}

// Controller usage
[HttpPut("{id}")]
public async Task<IActionResult> UpdateSkill(Guid id, UpdateProfileSkillDto dto)
{
    var userId = GetCurrentUserId();
    if (!await _service.CanModifySkill(id, userId))
        return Forbid();
    
    // ... update logic
}
```

### Input Validation
```csharp
// DTO validation attributes
public class AddProfileSkillDto
{
    [Required]
    public Guid SkillId { get; set; }
    
    [Range(0, 3)]
    public int ProficiencyLevel { get; set; }
    
    [MaxLength(200)]
    public string? AcquisitionMethod { get; set; }
    
    [Range(0, 50)]
    public int? YearsOfExperience { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}
```

### SQL Injection Prevention
```csharp
// Always use parameterized queries
var skills = await _context.UserProfileSkills
    .Where(ps => ps.UserProfile.UserId == userId)  // Parameter
    .Where(ps => ps.ProficiencyLevel == level)     // Parameter
    .ToListAsync();

// Never concatenate SQL strings
// ? BAD: $"SELECT * FROM Skills WHERE Name = '{name}'"
// ? GOOD: .Where(s => s.Name == name)
```

---

## Monitoring & Analytics

### Metrics to Track
- Average skills per profile
- Most common proficiency levels
- Most featured skills
- Profile completion rate with skills
- Skills update frequency
- Public profile views

### Logging
```csharp
_logger.LogInformation(
    "User {UserId} added skill {SkillId} with proficiency {Level}",
    userId, skillId, proficiencyLevel);

_logger.LogWarning(
    "User {UserId} attempted to add duplicate skill {SkillId}",
    userId, skillId);

_logger.LogError(ex,
    "Error updating skill {SkillId} for user {UserId}",
    skillId, userId);
```

---

## Status

**Module 2.1**: ? Complete

**Documentation**:
- ? Requirements (MODULE-2-USER-PROFILE-SKILLS-REQUIREMENTS.md)
- ? Implementation Guide (this file)
- ? API Tests (userprofile-with-skills-tests.http)

**Next Steps**:
- Implement frontend UI for skills management
- Add skills to Career Builder AI prompts
- Create skills recommendation engine
- Build skill endorsement system

---

**Last Updated**: 2025-01-20
