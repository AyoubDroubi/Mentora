# ?? Complete API Testing Suite - DDD Architecture

## ?? Quick Access

This folder contains comprehensive tests for verifying the **Domain-Driven Design (DDD)** architecture implementation.

---

## ?? Files in This Directory

| File | Purpose | Quick Start |
|------|---------|-------------|
| **complete-api-tests.http** | All 73 API tests | Open in VS Code ? Execute |
| **test-runner.js** | Automated test script | `node test-runner.js` |
| **profile-arabic-test.http** | Arabic text validation | Test Arabic support |
| **auth-tests.http** | Authentication tests | Test auth flow |

---

## ?? Quick Start (3 Steps)

### 1?? Prepare

```powershell
# Stop debugging
Shift + F5

# Clean and rebuild
cd ../../..
dotnet clean
dotnet build

# Run API
dotnet run
```

### 2?? Test

**Option A: VS Code REST Client**
```
1. Open: complete-api-tests.http
2. Click "Send Request" on each test
3. Verify responses
```

**Option B: Automated Runner**
```bash
node test-runner.js
```

**Option C: Swagger**
```
https://localhost:7000/swagger
```

### 3?? Verify

? All 73 tests pass  
? No 500 errors  
? Arabic text works  
? DDD architecture validated  

---

## ?? Test Coverage

### Modules Tested (8)

1. ? **Authentication** (10 tests)
   - Register, Login, Tokens, Logout

2. ? **User Profile** (10 tests)
   - CRUD, Arabic, Timezone validation

3. ? **Todo** (9 tests)
   - CRUD, Toggle, Summary, Filters

4. ? **Planner** (8 tests)
   - Events CRUD, Upcoming, Attendance

5. ? **Notes** (8 tests)
   - Notes CRUD, Partial updates

6. ? **Study Quiz** (4 tests)
   - Questions, Submit, Latest

7. ? **Study Sessions** (9 tests)
   - Sessions CRUD, Summary, Range

8. ? **Attendance** (4 tests)
   - Progress, History, Weekly

**Total: 73 tests across 43 endpoints**

---

## ??? DDD Architecture Validation

### What We're Testing

```
? Controllers ? Services ? Repositories ? Database
? No DbContext in Controllers
? No DbContext in Services
? Business logic in Services
? Data access in Repositories
? HTTP handling in Controllers
```

### All Modules Follow DDD Pattern

| Module | Controller | Service | Repository | Status |
|--------|-----------|---------|------------|--------|
| Auth | ? | ? | ? | ? PASS |
| Profile | ? | ? | ? | ? PASS |
| Todo | ? | ? | ? | ? PASS |
| Planner | ? | ? | ? | ? PASS |
| Notes | ? | ? | ? | ? PASS |
| Quiz | ? | ? | ? | ? PASS |
| Sessions | ? | ? | ? | ? PASS |
| Attendance | ? | ? | ? | ? PASS |

---

## ?? Detailed Documentation

For comprehensive information, see:

1. **[API-TESTING-COMPLETE-SUMMARY.md](../../../API-TESTING-COMPLETE-SUMMARY.md)**
   - Complete overview
   - All modules documented
   - Troubleshooting guide

2. **[DDD-TESTING-GUIDE.md](../../../DDD-TESTING-GUIDE.md)**
   - Step-by-step guide
   - Expected results
   - Performance metrics

3. **[DDD-TESTING-CHECKLIST.md](../../../DDD-TESTING-CHECKLIST.md)**
   - Verification checklist
   - Module-by-module checks
   - Sign-off template

---

## ? Quick Test Commands

### Run Specific Module Tests

```http
# Authentication
POST {{baseUrl}}/auth/login
GET {{baseUrl}}/auth/me

# Profile
GET {{baseUrl}}/userprofile
PUT {{baseUrl}}/userprofile

# Todo
GET {{baseUrl}}/todo
POST {{baseUrl}}/todo

# Planner
GET {{baseUrl}}/planner/events
POST {{baseUrl}}/planner/events

# Notes
GET {{baseUrl}}/notes
POST {{baseUrl}}/notes

# Study Quiz
GET {{baseUrl}}/study-quiz/questions
POST {{baseUrl}}/study-quiz/submit

# Study Sessions
GET {{baseUrl}}/study-sessions
POST {{baseUrl}}/study-sessions

# Attendance
GET {{baseUrl}}/study-planner/attendance/summary
```

---

## ?? Common Issues

### Issue 1: Build Errors

```powershell
# Solution
Shift + F5  # Stop debugging
dotnet clean
dotnet build
dotnet run
```

### Issue 2: 401 Unauthorized

```http
# Solution
1. POST /api/auth/login
2. Copy accessToken
3. Add: Authorization: Bearer {token}
```

### Issue 3: Arabic Text Shows ????

```http
# Solution
Content-Type: application/json; charset=utf-8
```

---

## ?? Success Criteria

Tests are successful when:

- [x] All 73 tests pass
- [x] No 500 errors
- [x] No database errors
- [x] Arabic text works
- [x] All layers follow DDD
- [x] Performance acceptable

---

## ?? Need Help?

1. Check [API-TESTING-COMPLETE-SUMMARY.md](../../../API-TESTING-COMPLETE-SUMMARY.md)
2. Review [DDD-TESTING-GUIDE.md](../../../DDD-TESTING-GUIDE.md)
3. Verify API is running: `https://localhost:7000/swagger`
4. Check logs in `logs/` directory

---

## ?? Test Flow

```
1. Start API
   ?
2. Login (get token)
   ?
3. Test Authentication
   ?
4. Test User Profile
   ?
5. Test Study Planner Modules
   ?
6. Verify DDD Architecture
   ?
7. Check Results
```

---

## ? Quick Checklist

Before testing:
- [ ] API running
- [ ] Database accessible
- [ ] Migrations applied
- [ ] Swagger working

During testing:
- [ ] Authentication works
- [ ] Token received
- [ ] All modules accessible
- [ ] Arabic text works

After testing:
- [ ] All tests passed
- [ ] No errors logged
- [ ] Architecture validated
- [ ] Ready for deployment

---

**Last Updated:** 2026-01-10  
**Version:** 1.0.0  
**Status:** ? Ready for Testing

---

**?? Everything is ready! Start testing with `complete-api-tests.http`** ??
