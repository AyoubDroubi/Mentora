# Module 2: User Profile & Personalization

## Overview
Complete implementation of user profile management including academic attributes, study level tracking, and timezone synchronization for personalized user experience.

---

## Features Implemented

### Academic Attributes
- **University**: Required, max 200 characters
- **Major**: Required, max 200 characters
- **Expected Graduation Year**: Required, range 2024-2050
- **Study Level**: Enum-based (Freshman ? Graduate)

**Purpose**: Enable AI to calculate timeline and adjust career roadmap complexity based on student's remaining time and academic level.

### Study Level Classification
```
Freshman   ? Year 1 ? Basic concepts
Sophomore  ? Year 2 ? Core skills
Junior     ? Year 3 ? Specialization
Senior     ? Year 4 ? Job preparation
Graduate   ? Post-grad ? Advanced topics
```

**Purpose**: AI adjusts content complexity based on student level.

### Timezone Synchronization
- **Format**: IANA timezone identifiers (e.g., Asia/Amman, America/New_York)
- **Validation**: Built-in .NET TimeZoneInfo
- **Default**: UTC

**Purpose**: Notifications and task schedules align with user's local time.

**Supported Regions**:
- Middle East: Asia/Amman, Asia/Riyadh, Asia/Dubai
- US: America/New_York, America/Los_Angeles
- Europe: Europe/London, Europe/Paris
- Asia: Asia/Tokyo, Asia/Shanghai
- Other: UTC, Australia/Sydney

---

## Architecture

### Domain Layer
```
Entities/
??? UserProfile.cs
    ??? University (string)
    ??? Major (string)
    ??? ExpectedGraduationYear (int)
    ??? CurrentLevel (StudyLevel enum)
    ??? Timezone (string)

Enums/
??? StudyLevel
    ??? Freshman
    ??? Sophomore
    ??? Junior
    ??? Senior
    ??? Graduate
```

### Application Layer
```
DTOs/UserProfile/
??? UpdateUserProfileDto.cs        # Input with validation
??? UserProfileResponseDto.cs      # Output with computed fields

Interfaces/
??? IUserProfileService.cs         # Service contract
```

### Infrastructure Layer
```
Services/
??? UserProfileService.cs
    ??? Timezone validation
    ??? Location-based timezone suggestions
    ??? Profile completion calculation
    ??? CRUD operations
```

### API Layer
```
Controllers/
??? UserProfileController.cs
    ??? GET /api/userprofile
    ??? PUT /api/userprofile
    ??? GET /api/userprofile/exists
    ??? GET /api/userprofile/completion
    ??? GET /api/userprofile/timezones
    ??? GET /api/userprofile/validate-timezone
```

---

## API Endpoints

### 1. Get Profile
```http
GET /api/userprofile
Authorization: Bearer {token}
```

**Response:**
```json
{
  "id": "guid",
  "bio": "Student bio",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2024,
  "currentLevel": "Senior",
  "timezone": "Asia/Amman",
  "yearsUntilGraduation": 0,
  "age": 24
}
```

### 2. Update Profile
```http
PUT /api/userprofile
Authorization: Bearer {token}

{
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2024,
  "currentLevel": "Senior",
  "timezone": "Asia/Amman",
  "bio": "Passionate CS student",
  "location": "Amman, Jordan",
  "phoneNumber": "+962791234567",
  "linkedInUrl": "https://linkedin.com/in/user",
  "gitHubUrl": "https://github.com/user"
}
```

**Validation**:
- University: Required, max 200 chars
- Major: Required, max 200 chars
- GraduationYear: Required, 2024-2050
- CurrentLevel: Required, valid enum
- Timezone: Required, valid IANA format
- URLs: Valid URL format if provided

### 3. Check Profile Exists
```http
GET /api/userprofile/exists
Authorization: Bearer {token}

Response: { "exists": true }
```

### 4. Get Completion Percentage
```http
GET /api/userprofile/completion
Authorization: Bearer {token}

Response: { "completionPercentage": 85 }
```

**Completion Factors** (10 fields):
- Required (40%): University, Major, Year, Timezone
- Optional (60%): Bio, Location, Phone, DOB, LinkedIn, GitHub

### 5. Get Timezone Suggestions
```http
GET /api/userprofile/timezones?location=Jordan

Response: ["Asia/Amman", "Asia/Jerusalem", "Asia/Dubai"]
```

### 6. Validate Timezone
```http
GET /api/userprofile/validate-timezone?timezone=Asia/Amman

Response: { "isValid": true, "timezone": "Asia/Amman" }
```

---

## Business Logic

### Study Level Impact

AI adjusts content based on level:

**Freshman**:
- Focus: Fundamentals, exploration
- Example: "Learn Python basics (3 months)"

**Senior**:
- Focus: Job prep, portfolio
- Example: "Build full-stack app with Python & React (2 months)"

**Graduate**:
- Focus: Research, specialization
- Example: "Explore Machine Learning research papers"

### Timeline Calculations

```
Years Until Graduation = ExpectedGraduationYear - CurrentYear

Used in AI prompts:
- "You have 2 years until graduation"
- "Create a plan fitting 18 months timeline"
```

### Timezone Use Cases

**Study Task Scheduling**:
- User in Jordan (UTC+3) schedules task at 9:00 AM local
- System stores as 6:00 AM UTC
- Displays to user as 9:00 AM in their timezone

**Notification Delivery**:
- Send notification at user's 8:00 AM
- System converts based on timezone
- Works across all regions

---

## Database Schema

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
    
    -- Academic
    University NVARCHAR(MAX) NOT NULL,
    Major NVARCHAR(MAX) NOT NULL,
    ExpectedGraduationYear INT NOT NULL,
    CurrentLevel INT NOT NULL,
    
    -- System
    Timezone NVARCHAR(MAX) NOT NULL DEFAULT 'UTC',
    
    -- Social
    LinkedInUrl NVARCHAR(MAX),
    GitHubUrl NVARCHAR(MAX),
    AvatarUrl NVARCHAR(MAX),
    
    -- Audit
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);
```

---

## Integration with Other Modules

### Module 1: Authentication
- Profile created after registration
- Linked via UserId

### Module 3: AI Career Builder
- **University** ? Academic context
- **Major** ? Career path tailoring
- **GraduationYear** ? Timeline constraints
- **StudyLevel** ? Content complexity

**AI Prompt Example**:
```
User: Computer Science Senior, 1 year to graduation, Jordan timezone
Generate: Advanced complexity, 12-month timeline, local job market focus
```

### Module 4: Study Planner
- **Timezone** ? Schedule tasks in local time
- **StudyLevel** ? Task difficulty suggestions

### Module 6: Gamification
- Profile completion ? Award XP
- Academic milestones ? Achievements

---

## Test Users

### Saad (Senior CS Student)
```
Email: saad@mentora.com
Password: Saad@123
University: University of Jordan
Major: Computer Science
Level: Senior
Timezone: Asia/Amman
```

### Maria (Graduate Data Science)
```
Email: maria@mentora.com
Password: Maria@123
University: American University of Dubai
Major: Data Science
Level: Graduate
Timezone: Asia/Dubai
```

---

## Testing

**Test File**: `Server/src/Mentora.Api/Tests/userprofile-tests.http`

**Test Coverage** (28 cases):
- Timezone operations (5 tests)
- Profile CRUD (4 tests)
- Validation (5 tests)
- Study levels (5 tests)
- Timezone regions (4 tests)
- Integration (5 tests)

**Run Tests**:
1. Start application
2. Open `userprofile-tests.http` in VS Code
3. Install REST Client extension
4. Run tests sequentially

---

## Quick Start

### 1. Run Backend
```bash
cd Server/src/Mentora.Api
dotnet run
```

### 2. Access Swagger
`https://localhost:7000/swagger`

### 3. Test Profile
- Login via `/api/auth/login`
- Get token
- Call `/api/userprofile` endpoints

---

## Status

**Module 2**: Complete

**Next Module**: Module 3 - AI Career Builder

---

**Last Updated**: 2024-12-31
