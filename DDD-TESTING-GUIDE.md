# ?? Mentora API - Complete Testing Guide

## ?? Overview

This guide covers comprehensive testing for all Mentora APIs after refactoring to **Domain-Driven Design (DDD)** architecture.

---

## ?? Testing Objectives

1. ? Verify all controllers work with new DDD architecture
2. ? Validate Repository pattern implementation
3. ? Ensure Service layer business logic
4. ? Test error handling and validation
5. ? Verify Arabic/Unicode support
6. ? Check authentication and authorization

---

## ??? Architecture Validation

### Layer Separation Checklist

```
? Controller ? Service ? Repository ? Database
? No DbContext in Controllers
? No DbContext in Services  
? Business Logic in Services
? Data Access in Repositories
? HTTP Handling in Controllers
```

---

## ?? Test Files

| File | Description | Endpoints Tested |
|------|-------------|------------------|
| `complete-api-tests.http` | Full test suite | All 8 modules |
| `profile-arabic-test.http` | Arabic text validation | UserProfile |
| `auth-tests.http` | Authentication flow | Auth module |

---

## ?? Quick Start

### Prerequisites

1. **Stop any running debug session**
   ```
   Shift + F5
   ```

2. **Rebuild the project**
   ```powershell
   cd Server\src\Mentora.Api
   dotnet clean
   dotnet build
   ```

3. **Run the API**
   ```powershell
   dotnet run
   ```

4. **Verify Swagger**
   ```
   https://localhost:7000/swagger
   ```

---

## ?? Test Execution Order

### Phase 1: Authentication (Required First)

```http
1. Register ? POST /api/auth/register
2. Login ? POST /api/auth/login (save access token)
3. Get Me ? GET /api/auth/me (verify token)
```

**Expected Results:**
- ? Registration successful
- ? Access token received
- ? User info returned

---

### Phase 2: User Profile

```http
1. Check Exists ? GET /api/userprofile/exists
2. Create Profile ? PUT /api/userprofile
3. Get Profile ? GET /api/userprofile
4. Get Completion ? GET /api/userprofile/completion
```

**Expected Results:**
- ? Profile created successfully
- ? Arabic text saved correctly
- ? Completion percentage calculated

---

### Phase 3: Study Planner - Todo

```http
1. Create Todo ? POST /api/todo
2. Get All Todos ? GET /api/todo
3. Toggle Todo ? PATCH /api/todo/{id}
4. Get Summary ? GET /api/todo/summary
5. Delete Todo ? DELETE /api/todo/{id}
```

**DDD Validation:**
```
? TodoController ? ITodoService
? TodoService ? ITodoRepository
? TodoRepository ? DbContext
? Business logic in TodoService
```

---

### Phase 4: Study Planner - Events

```http
1. Create Event ? POST /api/planner/events
2. Get All Events ? GET /api/planner/events
3. Get Upcoming ? GET /api/planner/events/upcoming
4. Mark Attended ? PATCH /api/planner/events/{id}/attend
5. Delete Event ? DELETE /api/planner/events/{id}
```

**DDD Validation:**
```
? PlannerController ? IPlannerService
? PlannerService ? IPlannerRepository
? PlannerRepository ? DbContext
```

---

### Phase 5: Study Planner - Notes

```http
1. Create Note ? POST /api/notes
2. Get All Notes ? GET /api/notes
3. Get Note ? GET /api/notes/{id}
4. Update Note ? PUT /api/notes/{id}
5. Delete Note ? DELETE /api/notes/{id}
```

**DDD Validation:**
```
? NotesController ? INotesService
? NotesService ? INotesRepository
? NotesRepository ? DbContext
```

---

### Phase 6: Study Quiz

```http
1. Get Questions ? GET /api/study-quiz/questions
2. Submit Quiz ? POST /api/study-quiz/submit
3. Get Latest ? GET /api/study-quiz/latest
```

**DDD Validation:**
```
? StudyQuizController ? IStudyQuizService
? StudyQuizService ? IStudyQuizRepository
? StudyQuizRepository ? DbContext
```

---

### Phase 7: Study Sessions

```http
1. Save Session ? POST /api/study-sessions
2. Get Summary ? GET /api/study-sessions/summary
3. Get All ? GET /api/study-sessions
4. Get by Range ? GET /api/study-sessions/range
5. Delete Session ? DELETE /api/study-sessions/{id}
```

**DDD Validation:**
```
? StudySessionsController ? IStudySessionsService
? StudySessionsService ? IStudySessionsRepository
? StudySessionsRepository ? DbContext
```

---

### Phase 8: Attendance & Progress

```http
1. Get Summary ? GET /api/study-planner/attendance/summary
2. Get History ? GET /api/study-planner/attendance/history
3. Get Weekly ? GET /api/study-planner/attendance/weekly
```

**DDD Validation:**
```
? AttendanceController ? IAttendanceService
? AttendanceService ? (uses ITodoRepository + IPlannerRepository)
? Business logic aggregation in Service
```

---

## ? Expected Test Results

### Success Criteria

#### Authentication Module
```json
{
  "isSuccess": true,
  "message": "Login successful",
  "accessToken": "eyJhbGc...",
  "refreshToken": "...",
  "tokenExpiration": "2026-01-10T15:00:00Z"
}
```

#### User Profile Module
```json
{
  "id": "guid",
  "userId": "guid",
  "university": "University of Jordan",
  "major": "Computer Science",
  "timezone": "Asia/Amman",
  "completionPercentage": 80
}
```

#### Todo Module
```json
{
  "success": true,
  "data": {
    "id": "guid",
    "title": "Complete Assignment",
    "isCompleted": false,
    "createdAt": "2026-01-10T12:00:00Z"
  }
}
```

#### Planner Module
```json
{
  "success": true,
  "data": {
    "id": "guid",
    "title": "Exam",
    "eventDateTime": "2026-01-15T10:00:00Z",
    "isAttended": false
  }
}
```

---

## ?? Validation Checklist

### Repository Pattern
- [ ] All repositories implement interfaces
- [ ] Repositories use DbContext
- [ ] No business logic in repositories
- [ ] Proper async/await usage

### Service Pattern
- [ ] All services implement interfaces
- [ ] Services use repositories (not DbContext)
- [ ] Business logic in services
- [ ] Proper error handling

### Controller Pattern
- [ ] Controllers use services (not repositories)
- [ ] HTTP-only logic in controllers
- [ ] Proper status codes returned
- [ ] Authorization attributes applied

---

## ?? Common Issues & Solutions

### Issue 1: Hot Reload Errors
**Symptom:**
```
ENC0014: Updating the base class requires restarting
```

**Solution:**
```powershell
# Stop debugging
Shift + F5

# Rebuild
dotnet clean
dotnet build
dotnet run
```

---

### Issue 2: RefreshToken Type Not Found
**Symptom:**
```
CS0246: The type 'RefreshToken' could not be found
```

**Solution:**
```csharp
// Add using statement
using Mentora.Domain.Entities;
```

---

### Issue 3: 401 Unauthorized
**Symptom:**
```
Status: 401 Unauthorized
```

**Solution:**
1. Login first
2. Copy access token
3. Add to Authorization header:
   ```
   Authorization: Bearer {your-token}
   ```

---

### Issue 4: Arabic Text Shows as ????
**Symptom:**
```json
{
  "bio": "???? ???? ?????"
}
```

**Solution:**
```http
Content-Type: application/json; charset=utf-8
```

---

## ?? Test Coverage Matrix

| Module | Endpoints | Tests | Status |
|--------|-----------|-------|--------|
| **Authentication** | 8 | 10 | ? Ready |
| **User Profile** | 8 | 10 | ? Ready |
| **Todo** | 6 | 9 | ? Ready |
| **Planner** | 5 | 8 | ? Ready |
| **Notes** | 5 | 8 | ? Ready |
| **Study Quiz** | 3 | 4 | ? Ready |
| **Study Sessions** | 5 | 9 | ? Ready |
| **Attendance** | 3 | 4 | ? Ready |
| **Integration** | - | 2 | ? Ready |
| **Error Handling** | - | 5 | ? Ready |
| **Unicode** | - | 4 | ? Ready |
| **Total** | **43** | **73** | ? **Ready** |

---

## ?? Test Scenarios

### Scenario 1: Complete Study Session Flow

```
1. Login
2. Create Profile
3. Take Study Quiz
4. Create Study Plan:
   - Add 3 Todos
   - Add 2 Events  
   - Add 2 Notes
5. Start Study Session (45 min)
6. Complete Todos
7. Attend Events
8. Check Progress Summary
```

**Expected:**
- All items created successfully
- Progress percentage calculated
- Study time tracked
- Attendance rate updated

---

### Scenario 2: Arabic Content Flow

```
1. Login
2. Create Profile (Arabic)
3. Add Todo (Arabic)
4. Add Event (Arabic)
5. Add Note (Arabic)
6. Retrieve all data
```

**Expected:**
- All Arabic text preserved
- No encoding issues
- Data retrieved correctly

---

### Scenario 3: Error Handling Flow

```
1. Try unauthorized access
2. Try invalid token
3. Try missing required fields
4. Try invalid data types
5. Try non-existent resources
```

**Expected:**
- 401 for unauthorized
- 400 for validation errors
- 404 for not found
- Proper error messages

---

## ?? Performance Benchmarks

### Expected Response Times

| Endpoint Type | Target | Acceptable |
|--------------|--------|------------|
| GET (single) | < 100ms | < 200ms |
| GET (list) | < 200ms | < 500ms |
| POST | < 150ms | < 300ms |
| PUT | < 150ms | < 300ms |
| DELETE | < 100ms | < 200ms |

---

## ?? Security Tests

### Authentication Tests
- [ ] Can't access protected endpoints without token
- [ ] Expired tokens rejected
- [ ] Invalid tokens rejected
- [ ] Refresh token rotation works
- [ ] Logout revokes tokens

### Authorization Tests
- [ ] Users can only access their own data
- [ ] Can't modify other users' resources
- [ ] Proper 403 for forbidden access

---

## ?? Cleanup After Tests

```http
# Delete all test data
DELETE {{baseUrl}}/todo/{id}
DELETE {{baseUrl}}/planner/events/{id}
DELETE {{baseUrl}}/notes/{id}
DELETE {{baseUrl}}/study-sessions/{id}

# Logout
POST {{baseUrl}}/auth/logout-all
```

---

## ?? Additional Resources

- [DDD Architecture Guide](../../../DOCUMENTATION-CLEANUP-SUMMARY.md)
- [API Quick Reference](../../Mentora.Documentation/Docs/API-QUICK-REFERENCE.md)
- [Swagger Documentation](https://localhost:7000/swagger)

---

## ? Final Verification

After running all tests, verify:

1. ? All 73 tests pass
2. ? No database errors in logs
3. ? No 500 errors
4. ? Arabic text works
5. ? All DDD layers working
6. ? Repository pattern implemented
7. ? Service pattern implemented
8. ? Controllers thin and clean

---

## ?? Success Criteria

**All tests pass when:**

```
? Authentication: 10/10 tests pass
? User Profile: 10/10 tests pass
? Todo: 9/9 tests pass
? Planner: 8/8 tests pass
? Notes: 8/8 tests pass
? Study Quiz: 4/4 tests pass
? Study Sessions: 9/9 tests pass
? Attendance: 4/4 tests pass
? Integration: 2/2 tests pass
? Error Handling: 5/5 tests pass
? Unicode: 4/4 tests pass

Total: 73/73 tests ?
```

---

**Last Updated:** 2026-01-10  
**Version:** 1.0.0  
**Status:** ? Ready for Testing
