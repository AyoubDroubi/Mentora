# ? DDD Architecture Testing Checklist

## ?? Pre-Testing Setup

- [ ] Stop debugging (Shift + F5)
- [ ] Clean solution
  ```powershell
  dotnet clean
  ```
- [ ] Rebuild solution
  ```powershell
  dotnet build
  ```
- [ ] Run migrations
  ```powershell
  dotnet ef database update
  ```
- [ ] Start API
  ```powershell
  dotnet run
  ```
- [ ] Verify Swagger at `https://localhost:7000/swagger`

---

## ?? Architecture Verification

### Layer Separation

- [ ] **Controllers** only handle HTTP requests/responses
- [ ] **Services** contain all business logic
- [ ] **Repositories** only access database
- [ ] **No DbContext** in Controllers
- [ ] **No DbContext** in Services
- [ ] All **Dependencies** use interfaces

### Dependency Injection

- [ ] All repositories registered in `Program.cs`
- [ ] All services registered in `Program.cs`
- [ ] Controllers use constructor injection
- [ ] Services use constructor injection
- [ ] Repositories use constructor injection

---

## ?? Module 1: Authentication

### Endpoints
- [ ] POST `/api/auth/register` - Register new user
- [ ] POST `/api/auth/login` - Login user
- [ ] GET `/api/auth/me` - Get current user
- [ ] POST `/api/auth/refresh-token` - Refresh access token
- [ ] POST `/api/auth/logout` - Logout current device
- [ ] POST `/api/auth/logout-all` - Logout all devices
- [ ] POST `/api/auth/forgot-password` - Request password reset
- [ ] POST `/api/auth/reset-password` - Reset password

### Architecture Validation
- [ ] `AuthController` ? `IAuthService`
- [ ] `AuthService` ? `IRefreshTokenRepository`
- [ ] `AuthService` ? `IPasswordResetTokenRepository`
- [ ] No `DbContext` in `AuthService`
- [ ] Business logic in `AuthService`

### Tests
- [ ] Can register new user
- [ ] Can login with valid credentials
- [ ] Receives JWT access token
- [ ] Receives refresh token
- [ ] Can get current user with token
- [ ] Can refresh expired token
- [ ] Can logout (single device)
- [ ] Can logout all devices
- [ ] Proper error for invalid credentials
- [ ] Proper error for missing fields

---

## ?? Module 2: User Profile

### Endpoints
- [ ] GET `/api/userprofile` - Get user profile
- [ ] PUT `/api/userprofile` - Create/Update profile
- [ ] GET `/api/userprofile/exists` - Check if profile exists
- [ ] GET `/api/userprofile/completion` - Get completion percentage
- [ ] GET `/api/userprofile/timezones` - Get timezone suggestions
- [ ] GET `/api/userprofile/validate-timezone` - Validate timezone

### Architecture Validation
- [ ] `UserProfileController` ? `IUserProfileService`
- [ ] `UserProfileService` ? `IUserProfileRepository`
- [ ] No `DbContext` in `UserProfileService`
- [ ] Business logic in `UserProfileService`

### Tests
- [ ] Can create profile
- [ ] Can update profile
- [ ] Can get profile
- [ ] Arabic text saved correctly
- [ ] Mixed English/Arabic works
- [ ] Timezone validation works
- [ ] Completion percentage calculated
- [ ] Proper error for invalid timezone
- [ ] Proper error for missing required fields

---

## ? Module 3: Todo List

### Endpoints
- [ ] GET `/api/todo` - Get all todos
- [ ] GET `/api/todo?filter=active` - Get active todos
- [ ] GET `/api/todo?filter=completed` - Get completed todos
- [ ] POST `/api/todo` - Create todo
- [ ] PATCH `/api/todo/{id}` - Toggle completion
- [ ] DELETE `/api/todo/{id}` - Delete todo
- [ ] GET `/api/todo/summary` - Get summary

### Architecture Validation
- [ ] `TodoController` ? `ITodoService`
- [ ] `TodoService` ? `ITodoRepository`
- [ ] No `DbContext` in `TodoService`
- [ ] Business logic in `TodoService`

### Tests
- [ ] Can create todo
- [ ] Can get all todos
- [ ] Can filter active todos
- [ ] Can filter completed todos
- [ ] Can toggle completion
- [ ] Can delete todo
- [ ] Summary calculated correctly
- [ ] Arabic text in todos works
- [ ] Proper error for empty title

---

## ?? Module 4: Planner (Events)

### Endpoints
- [ ] GET `/api/planner/events` - Get all events
- [ ] GET `/api/planner/events?date=YYYY-MM-DD` - Get events by date
- [ ] GET `/api/planner/events/upcoming` - Get upcoming events
- [ ] POST `/api/planner/events` - Create event
- [ ] PATCH `/api/planner/events/{id}/attend` - Mark attended
- [ ] DELETE `/api/planner/events/{id}` - Delete event

### Architecture Validation
- [ ] `PlannerController` ? `IPlannerService`
- [ ] `PlannerService` ? `IPlannerRepository`
- [ ] No `DbContext` in `PlannerService`
- [ ] Business logic in `PlannerService`

### Tests
- [ ] Can create event
- [ ] Can get all events
- [ ] Can get events by date
- [ ] Can get upcoming events
- [ ] Can mark as attended
- [ ] Can delete event
- [ ] Arabic text in events works
- [ ] Proper error for invalid date

---

## ?? Module 5: Notes

### Endpoints
- [ ] GET `/api/notes` - Get all notes
- [ ] GET `/api/notes/{id}` - Get specific note
- [ ] POST `/api/notes` - Create note
- [ ] PUT `/api/notes/{id}` - Update note
- [ ] DELETE `/api/notes/{id}` - Delete note

### Architecture Validation
- [ ] `NotesController` ? `INotesService`
- [ ] `NotesService` ? `INotesRepository`
- [ ] No `DbContext` in `NotesService`
- [ ] Business logic in `NotesService`

### Tests
- [ ] Can create note
- [ ] Can get all notes
- [ ] Can get specific note
- [ ] Can update note (full)
- [ ] Can update note (partial)
- [ ] Can delete note
- [ ] Arabic text in notes works
- [ ] Proper error for empty title/content

---

## ?? Module 6: Study Quiz

### Endpoints
- [ ] GET `/api/study-quiz/questions` - Get quiz questions
- [ ] POST `/api/study-quiz/submit` - Submit answers
- [ ] GET `/api/study-quiz/latest` - Get latest attempt

### Architecture Validation
- [ ] `StudyQuizController` ? `IStudyQuizService`
- [ ] `StudyQuizService` ? `IStudyQuizRepository`
- [ ] No `DbContext` in `StudyQuizService`
- [ ] Business logic in `StudyQuizService`

### Tests
- [ ] Can get questions
- [ ] Can submit answers
- [ ] Study plan generated
- [ ] Can get latest attempt
- [ ] Proper error for empty answers

---

## ?? Module 7: Study Sessions

### Endpoints
- [ ] GET `/api/study-sessions` - Get all sessions
- [ ] GET `/api/study-sessions/summary` - Get summary
- [ ] GET `/api/study-sessions/range` - Get by date range
- [ ] POST `/api/study-sessions` - Save session
- [ ] DELETE `/api/study-sessions/{id}` - Delete session

### Architecture Validation
- [ ] `StudySessionsController` ? `IStudySessionsService`
- [ ] `StudySessionsService` ? `IStudySessionsRepository`
- [ ] No `DbContext` in `StudySessionsService`
- [ ] Business logic in `StudySessionsService`

### Tests
- [ ] Can save session
- [ ] Can get all sessions
- [ ] Can get summary
- [ ] Can get by date range
- [ ] Can delete session
- [ ] Proper error for invalid duration
- [ ] Proper error for negative duration

---

## ?? Module 8: Attendance & Progress

### Endpoints
- [ ] GET `/api/study-planner/attendance/summary` - Get summary
- [ ] GET `/api/study-planner/attendance/history` - Get history
- [ ] GET `/api/study-planner/attendance/weekly` - Get weekly progress

### Architecture Validation
- [ ] `AttendanceController` ? `IAttendanceService`
- [ ] `AttendanceService` ? `ITodoRepository + IPlannerRepository`
- [ ] No `DbContext` in `AttendanceService`
- [ ] Business logic aggregation in `AttendanceService`

### Tests
- [ ] Can get summary
- [ ] Progress percentage calculated (50% tasks + 50% events)
- [ ] Can get history
- [ ] Can get weekly progress
- [ ] Statistics accurate

---

## ?? Integration Tests

- [ ] Can create complete study plan (todo + event + note + session)
- [ ] Progress reflects all activities
- [ ] Attendance rate calculated correctly
- [ ] Arabic content works across all modules
- [ ] Deleting items updates progress

---

## ?? Error Handling Tests

- [ ] 401 for missing token
- [ ] 401 for invalid token
- [ ] 401 for expired token
- [ ] 400 for validation errors
- [ ] 400 for missing required fields
- [ ] 404 for non-existent resources
- [ ] Proper error messages returned

---

## ?? Unicode & Special Characters

- [ ] Arabic text saved correctly
- [ ] Arabic text retrieved correctly
- [ ] Mixed English/Arabic works
- [ ] Emojis work
- [ ] Special math symbols work
- [ ] No encoding issues in responses

---

## ?? Performance Tests

- [ ] Single GET < 200ms
- [ ] List GET < 500ms
- [ ] POST < 300ms
- [ ] PUT < 300ms
- [ ] DELETE < 200ms
- [ ] No memory leaks
- [ ] No N+1 query problems

---

## ?? Security Tests

- [ ] Can't access without authentication
- [ ] Can't access other users' data
- [ ] Token rotation works
- [ ] Logout revokes tokens
- [ ] Password reset tokens work
- [ ] SQL injection prevented
- [ ] XSS prevented

---

## ?? Final Verification

### Code Quality
- [ ] No compiler warnings
- [ ] No code smells
- [ ] Proper error handling everywhere
- [ ] Consistent naming conventions
- [ ] XML documentation on public methods

### DDD Principles
- [ ] Domain layer independent
- [ ] Application layer has interfaces only
- [ ] Infrastructure has implementations
- [ ] API layer thin (HTTP only)
- [ ] No circular dependencies

### Database
- [ ] Migrations applied successfully
- [ ] Seed data created
- [ ] Relationships configured correctly
- [ ] Indexes on foreign keys
- [ ] No orphaned records

---

## ?? Test Results Summary

| Module | Total Tests | Passed | Failed | Status |
|--------|-------------|---------|---------|--------|
| Authentication | 10 | __ | __ | ? |
| User Profile | 10 | __ | __ | ? |
| Todo | 9 | __ | __ | ? |
| Planner | 8 | __ | __ | ? |
| Notes | 8 | __ | __ | ? |
| Study Quiz | 4 | __ | __ | ? |
| Study Sessions | 9 | __ | __ | ? |
| Attendance | 4 | __ | __ | ? |
| Integration | 2 | __ | __ | ? |
| Error Handling | 5 | __ | __ | ? |
| Unicode | 4 | __ | __ | ? |
| **TOTAL** | **73** | **__** | **__** | **?** |

---

## ? Sign-Off

- [ ] All tests passed
- [ ] No critical bugs
- [ ] Documentation updated
- [ ] Code reviewed
- [ ] Ready for deployment

**Tested By:** _______________  
**Date:** _______________  
**Status:** ? PASS  ? FAIL  

---

## ?? Quick Commands

### Run All Tests
```powershell
# Using HTTP files in VS Code
# Open: complete-api-tests.http
# Execute all requests sequentially
```

### Run Automated Tests
```bash
cd Server/src/Mentora.Api/Tests
node test-runner.js
```

### Check Logs
```powershell
# View API logs
Get-Content "logs/mentora-$(Get-Date -Format 'yyyy-MM-dd').log" -Tail 100
```

### Reset Database
```powershell
dotnet ef database drop --force
dotnet ef database update
```

---

**Last Updated:** 2026-01-10  
**Version:** 1.0.0  
**Status:** ? Ready for Testing
