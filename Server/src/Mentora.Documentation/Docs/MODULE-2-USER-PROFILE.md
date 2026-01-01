# Module 2: User Profile & Personalization

## ?? Overview
Complete implementation of **SRS Module 2** covering academic attributes management, study level logic, and timezone synchronization for personalized user experience.

## ? Implemented Requirements

### 2.1 Academic Attributes Management

#### ? 2.1.1 Structured Data Storage
The system persists structured academic data in the UserProfile entity:

- **University**: Required field, max 200 characters
- **Major**: Required field, max 200 characters  
- **ExpectedGraduationYear**: Required field, range 2024-2050
- Location: `UserProfile.cs` entity with database persistence

**Purpose**: Enable AI engine to calculate timeline and adjust career roadmap complexity based on student's remaining time until graduation.

#### ? 2.1.2 Study Level Logic
The system implements enum-based study level classification:

- **StudyLevel Enum**: `Freshman`, `Sophomore`, `Junior`, `Senior`, `Graduate`
- Location: `Domain/Enums/Enums.cs`
- Stored in: `UserProfile.CurrentLevel` property

**Purpose**: Allow AI engine to adjust the complexity and depth of generated career roadmaps based on student's academic progression.

**Example**:
- Freshman ? Basic concepts, foundational skills
- Senior ? Advanced topics, job preparation
- Graduate ? Specialization, research opportunities

### 2.2 System Configurations

#### ? 2.2.1 Timezone Synchronization
The system detects and stores user's timezone in IANA format:

- **Timezone Field**: String property in UserProfile
- **Format**: IANA timezone identifiers (e.g., "Asia/Amman", "America/New_York")
- **Validation**: Built-in validation against .NET TimeZoneInfo
- **Default**: UTC

**Purpose**: Ensure notifications and task schedules align with user's local time, not server's UTC time.

**Supported Timezones**:
```
Middle East: Asia/Amman, Asia/Riyadh, Asia/Dubai, Asia/Jerusalem
US: America/New_York, America/Chicago, America/Los_Angeles
Europe: Europe/London, Europe/Paris, Europe/Berlin
Asia: Asia/Tokyo, Asia/Shanghai
Others: UTC, Australia/Sydney, Pacific/Auckland
```

---

## ??? Architecture

### Domain Layer (`Mentora.Domain`)
```
Entities/
??? UserProfile.cs                 # Main profile entity
?   ??? University (string)        # SRS 2.1.1
?   ??? Major (string)             # SRS 2.1.1
?   ??? ExpectedGraduationYear (int) # SRS 2.1.1
?   ??? CurrentLevel (StudyLevel)  # SRS 2.1.2
?   ??? Timezone (string)          # SRS 2.2.1
?
Enums/
??? Enums.cs
    ??? StudyLevel                 # SRS 2.1.2
        ??? Freshman
        ??? Sophomore
        ??? Junior
        ??? Senior
        ??? Graduate
```

### Application Layer (`Mentora.Application`)
```
DTOs/UserProfile/
??? UpdateUserProfileDto.cs        # Input with validation
??? UserProfileResponseDto.cs      # Output with computed fields

Interfaces/
??? IUserProfileService.cs         # Service contract
    ??? GetProfileAsync()
    ??? CreateOrUpdateProfileAsync()
    ??? HasProfileAsync()
    ??? GetProfileCompletionAsync()
    ??? IsValidTimezone()
    ??? GetSuggestedTimezonesAsync()
```

### Infrastructure Layer (`Mentora.Infrastructure`)
```
Services/
??? UserProfileService.cs          # Complete implementation
    ??? Timezone validation
    ??? Location-based timezone suggestions
    ??? Profile completion calculation
    ??? CRUD operations
```

### API Layer (`Mentora.Api`)
```
Controllers/
??? UserProfileController.cs       # REST endpoints
    ??? GET /api/userprofile
    ??? PUT /api/userprofile
    ??? GET /api/userprofile/exists
    ??? GET /api/userprofile/completion
    ??? GET /api/userprofile/timezones
    ??? GET /api/userprofile/validate-timezone
```

---

## ?? API Endpoints

### 1. Get User Profile
```http
GET /api/userprofile
Authorization: Bearer {accessToken}
```

**Response:**
```json
{
  "id": "guid",
  "userId": "guid",
  "bio": "Student bio...",
  "location": "Amman, Jordan",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2024,
  "currentLevel": "Senior",
  "currentLevelName": "Senior",
  "timezone": "Asia/Amman",
  "yearsUntilGraduation": 0,
  "age": 24,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

### 2. Create/Update Profile
```http
PUT /api/userprofile
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2024,
  "currentLevel": "Senior",
  "timezone": "Asia/Amman",
  "bio": "Passionate CS student...",
  "location": "Amman, Jordan",
  "phoneNumber": "+962791234567",
  "linkedInUrl": "https://linkedin.com/in/username",
  "gitHubUrl": "https://github.com/username"
}
```

**Validation Rules**:
- ? University: Required, max 200 chars
- ? Major: Required, max 200 chars
- ? ExpectedGraduationYear: Required, 2024-2050
- ? CurrentLevel: Required, valid StudyLevel enum
- ? Timezone: Required, valid IANA format
- ? URLs: Valid URL format if provided
- ? PhoneNumber: Valid phone format if provided

### 3. Check Profile Existence
```http
GET /api/userprofile/exists
Authorization: Bearer {accessToken}
```

**Response:**
```json
{
  "exists": true
}
```

### 4. Get Profile Completion
```http
GET /api/userprofile/completion
Authorization: Bearer {accessToken}
```

**Response:**
```json
{
  "completionPercentage": 85
}
```

**Completion Factors** (10 fields total):
- Required: University, Major, ExpectedGraduationYear, Timezone (40%)
- Valuable: Bio, Location, Phone, DateOfBirth, LinkedIn, GitHub (60%)

### 5. Get Suggested Timezones
```http
GET /api/userprofile/timezones?location=Jordan
```

**Response:**
```json
[
  "Asia/Amman",
  "Asia/Jerusalem",
  "Asia/Dubai"
]
```

### 6. Validate Timezone
```http
GET /api/userprofile/validate-timezone?timezone=Asia/Amman
```

**Response:**
```json
{
  "isValid": true,
  "timezone": "Asia/Amman"
}
```

---

## ?? Business Logic

### Study Level Impact (SRS 2.1.2)

| Level | Years | AI Complexity | Focus |
|-------|-------|---------------|-------|
| Freshman | Year 1 | Basic | Fundamentals, exploration |
| Sophomore | Year 2 | Intermediate | Core skills, projects |
| Junior | Year 3 | Advanced | Specialization, internships |
| Senior | Year 4 | Professional | Job prep, portfolio |
| Graduate | Post-grad | Expert | Research, advanced roles |

**Use Case**: When generating career plans, AI considers study level:
```
Freshman ? "Learn Python basics (3 months)"
Senior ? "Build full-stack applications with Python & React (2 months)"
```

### Timeline Calculations (SRS 2.1.1)

The system calculates timeline based on `ExpectedGraduationYear`:

```csharp
int yearsUntilGraduation = ExpectedGraduationYear - CurrentYear;

// Used in AI prompts:
// "You have 2 years until graduation"
// "Create a plan fitting 18 months timeline"
```

### Timezone Synchronization (SRS 2.2.1)

**Purpose**: All time-based features respect user's local time

**Implementation**:
```csharp
// User in Jordan (Asia/Amman = UTC+3)
UserTimezone = "Asia/Amman"

// When scheduling task at 9:00 AM local
TaskSchedule = ConvertToUtc(9:00, "Asia/Amman") // ? 6:00 UTC

// When sending notification
NotificationTime = ConvertFromUtc(utcTime, "Asia/Amman")
```

**Affected Features**:
- ? Study task scheduling
- ?? Notification delivery
- ?? Daily reflection timing
- ?? Deadline calculations

---

## ?? Data Validation

### UpdateUserProfileDto Validation

```csharp
// Academic Attributes (SRS 2.1.1)
[Required]
[MaxLength(200)]
public string University { get; set; }

[Required]
[MaxLength(200)]
public string Major { get; set; }

[Required]
[Range(2024, 2050)]
public int ExpectedGraduationYear { get; set; }

// Study Level (SRS 2.1.2)
[Required]
public StudyLevel CurrentLevel { get; set; }

// Timezone (SRS 2.2.1)
[Required]
[RegularExpression(@"^[A-Za-z_]+/[A-Za-z_]+$")]
public string Timezone { get; set; }

// Optional Fields
[MaxLength(500)]
public string? Bio { get; set; }

[Url]
public string? LinkedInUrl { get; set; }
```

---

## ?? Testing

### Test File Location
`Server/src/Mentora.Api/Tests/userprofile-tests.http`

### Test Coverage (28 Test Cases)

1. **Timezone Operations** (5 tests)
   - Get suggested timezones
   - Location-based suggestions
   - Timezone validation

2. **Profile CRUD** (4 tests)
   - Get profile
   - Create/update profile
   - Check existence
   - Get completion percentage

3. **Validation Tests** (5 tests)
   - Invalid timezone format
   - Missing required fields
   - Invalid graduation year
   - Invalid URLs
   - Invalid study level

4. **Study Level Tests** (5 tests)
   - Test all enum values: Freshman ? Graduate

5. **Timezone Tests** (4 tests)
   - Middle East, US, Europe, Asia timezones

6. **Integration Tests** (5 tests)
   - Login and profile operations
   - Multiple users
   - Partial updates

### Running Tests

1. Start the application
2. Open `userprofile-tests.http` in VS Code
3. Install REST Client extension
4. Run tests sequentially
5. Update `@accessToken` variable after login

---

## ?? Database Schema

### UserProfile Table

```sql
CREATE TABLE UserProfiles (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    
    -- Personal
    Bio NVARCHAR(MAX),
    Location NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    DateOfBirth DATETIME2,
    
    -- Academic Attributes (SRS 2.1.1)
    University NVARCHAR(MAX) NOT NULL,
    Major NVARCHAR(MAX) NOT NULL,
    ExpectedGraduationYear INT NOT NULL,
    
    -- Study Level (SRS 2.1.2)
    CurrentLevel INT NOT NULL, -- Enum stored as int
    
    -- Timezone (SRS 2.2.1)
    Timezone NVARCHAR(MAX) NOT NULL DEFAULT 'UTC',
    
    -- Social
    LinkedInUrl NVARCHAR(MAX),
    GitHubUrl NVARCHAR(MAX),
    AvatarUrl NVARCHAR(MAX),
    
    -- Audit
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
```

---

## ?? Integration with Other Modules

### Module 1: Authentication
- Profile created after user registration
- Profile linked via UserId foreign key

### Module 3: AI Career Builder
- **University** ? Understands academic context
- **Major** ? Tailors career paths
- **ExpectedGraduationYear** ? Calculates timeline constraints
- **CurrentLevel** ? Adjusts complexity

**AI Prompt Injection**:
```
User Context:
- Major: Computer Science
- Study Level: Senior
- Time until graduation: 1 year
- Location timezone: Asia/Amman

Generate career plan considering:
- 12 months timeline
- Advanced complexity (Senior level)
- Local job market in Jordan
```

### Module 4: Study Planner
- **Timezone** ? Schedule tasks in local time
- **CurrentLevel** ? Suggest appropriate task difficulty

### Module 6: Gamification
- Profile completion ? Award XP
- Academic milestones ? Trigger achievements

---

## ?? Sample Data

### Test User: Saad
```json
{
  "email": "saad@mentora.com",
  "password": "Saad@123",
  "profile": {
    "university": "University of Jordan",
    "major": "Computer Science",
    "expectedGraduationYear": 2024,
    "currentLevel": "Senior",
    "timezone": "Asia/Amman",
    "location": "Amman, Jordan"
  }
}
```

### Test User: Maria
```json
{
  "email": "maria@mentora.com",
  "password": "Maria@123",
  "profile": {
    "university": "American University of Dubai",
    "major": "Data Science",
    "expectedGraduationYear": 2023,
    "currentLevel": "Graduate",
    "timezone": "Asia/Dubai",
    "location": "Dubai, UAE"
  }
}
```

---

## ?? Running the Application

### Prerequisites
- .NET 9 SDK
- SQL Server
- Existing database from Module 1

### Steps
1. Update database with migration:
```bash
dotnet ef database update --project Server/src/Mentora.Infrastructure --startup-project Server/src/Mentora.Api
```

2. Run the application:
```bash
cd Server/src/Mentora.Api
dotnet run
```

3. Access Swagger UI: `https://localhost:7001/swagger`

4. Test endpoints using `userprofile-tests.http`

---

## ?? Configuration

### appsettings.json
No additional configuration needed. Uses existing database connection.

### Timezone Support
The system uses .NET's built-in `TimeZoneInfo` class:
- Automatically detects system timezones
- Supports all IANA timezone identifiers
- Cross-platform compatible

---

## ? Clean Architecture Compliance

? **Domain Layer**: Pure entities, no dependencies  
? **Application Layer**: DTOs, interfaces, business rules  
? **Infrastructure Layer**: EF Core implementation  
? **API Layer**: Controllers, authentication, validation  

? **SOLID Principles**: Applied throughout  
? **Dependency Injection**: All services registered  
? **Separation of Concerns**: Clear layer boundaries  

---

## ?? SRS Compliance Matrix

| Requirement | Status | Location |
|------------|--------|----------|
| 2.1.1 Structured Data Storage | ? | `UserProfile` entity |
| 2.1.2 Study Level Logic | ? | `StudyLevel` enum |
| 2.2.1 Timezone Synchronization | ? | `Timezone` field + validation |
| GUID Primary Keys (8.1) | ? | All entities |
| Soft Deletes (8.2) | ? | `BaseEntity` |

---

## ?? Next Steps

Module 2 is **COMPLETE** and ready for integration with other modules.

**Next Module**: Module 3 - AI Career Builder (The Strategist)

---

## ?? Related Documentation

- [Module 1: Authentication](./MODULE-1-AUTHENTICATION.md)
- [Domain Overview](./domain/01-DOMAIN-OVERVIEW.md)
- [Architecture Overview](./architecture/01-ARCHITECTURE-OVERVIEW.md)

---

**Last Updated**: 2024-12-31  
**Version**: 1.0.0  
**Module Status**: ? COMPLETE
