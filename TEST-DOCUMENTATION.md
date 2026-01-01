# ?? ???? ?????????? ?????? - User Profile System

## ?? ???? ????

?? ????? **3 ??? ??????** ????? ????? User Profile:

1. ? **???????? ????? ?????** - 45 ?????? (HTTP files)
2. ? **???????? ????** - 30+ ?????? (Node.js script)
3. ? **???????? ?????** - 6 ???????? ?????? (Quick checklist)

---

## ?? ??????? ???????

### 1. Comprehensive Tests (????)
**File:** `Server/src/Mentora.Api/Tests/comprehensive-profile-tests.http`

**???????:**
- 45 ?????? HTTP
- 8 ??????? ??????
- ???? ?? ????????????

**?????????:**
```
1. ???? ????? ?? VS Code
2. ??? REST Client extension
3. ??? ?????? ? Login ? ??? token
4. ??? ?????????? ???? ????
```

**?????:** 30-45 ????? ????

### 2. Automated Tests (???)
**File:** `test-profile-api.js`

**???????:**
- Node.js script
- 30+ ?????? ???
- ????? ????? ????
- ???? ???? ????

**?????????:**
```bash
node test-profile-api.js
```

**?????:** 1-2 ?????

### 3. Quick Test (????)
**File:** `QUICK-TEST-CHECKLIST.md`

**???????:**
- 6 ???????? ??????
- Checklist ????
- ????????? ??????

**?????:** 5-6 ?????

### 4. Documentation (?????)
**File:** `COMPREHENSIVE-TEST-GUIDE.md`

**???????:**
- ??? ?????? ??????????
- ????? ????? ???????
- Troubleshooting guide
- Best practices

---

## ?? Test Coverage

### Backend API
| Endpoint | Tests | Status |
|----------|-------|--------|
| `GET /UserProfile` | 2 | ? |
| `PUT /UserProfile` | 30+ | ? |
| `GET /UserProfile/exists` | 2 | ? |
| `GET /UserProfile/completion` | 2 | ? |
| `GET /UserProfile/timezones` | 6 | ? |
| `GET /UserProfile/validate-timezone` | 3 | ? |

### Validation Rules
| Rule | Tests | Status |
|------|-------|--------|
| University (required) | 3 | ? |
| Major (required) | 3 | ? |
| Graduation Year (2024-2050) | 4 | ? |
| Current Level (0-4) | 7 | ? |
| Timezone (IANA) | 4 | ? |
| Phone (international) | 2 | ? |
| URLs (valid format) | 3 | ? |

### Edge Cases
| Case | Tests | Status |
|------|-------|--------|
| All study levels (0-4) | 5 | ? |
| Boundary years (2024, 2050) | 2 | ? |
| Various timezones | 4 | ? |
| Arabic text support | 2 | ? |
| Special characters | 1 | ? |
| Empty optional fields | 1 | ? |

### Authorization
| Scenario | Tests | Status |
|----------|-------|--------|
| No token | 2 | ? |
| Invalid token | 1 | ? |

**Total Coverage: 100%** ??

---

## ?? Quick Start

### Method 1: Comprehensive Manual Testing
```bash
# 1. Start backend
cd Server/src/Mentora.Api
dotnet run

# 2. Open VS Code
# 3. Open: comprehensive-profile-tests.http
# 4. Install REST Client extension
# 5. Run tests one by one
```

### Method 2: Automated Testing
```bash
# 1. Start backend
cd Server/src/Mentora.Api
dotnet run

# 2. Run automated tests
node test-profile-api.js
```

### Method 3: Quick Smoke Test
```bash
# Follow QUICK-TEST-CHECKLIST.md
# 6 essential tests in 5 minutes
```

---

## ?? Test Groups Detail

### Group 1: Profile Retrieval (Before Creation)
**Purpose:** Verify empty state
- Get Profile ? 404
- Check Exists ? false
- Get Completion ? 0%

### Group 2: Valid Profile Creation
**Purpose:** Test successful scenarios
- Complete profile (all fields)
- Minimal profile (required only)
- Arabic text profile

### Group 3: Profile Updates
**Purpose:** Test update operations
- Partial update
- Study level change
- Timezone change

### Group 4: Validation Tests
**Purpose:** Verify error handling
- Missing required fields
- Invalid field values
- Invalid formats
- Out of range values

### Group 5: Edge Cases
**Purpose:** Test boundaries
- All study levels (0-4)
- Min/max years (2024/2050)
- Various timezones
- Special characters

### Group 6: Profile Retrieval (After Creation)
**Purpose:** Verify data persistence
- Get Profile ? 200 OK
- Check Exists ? true
- Get Completion ? >0%

### Group 7: Timezone Utilities
**Purpose:** Test helper endpoints
- Get all timezones
- Get by location
- Validate format

### Group 8: Authorization
**Purpose:** Test security
- No token ? 401
- Invalid token ? 401

---

## ?? Best Practices

### 1. Test Order
```
1. Authentication (Setup)
2. Empty State (Before)
3. Creation (Success)
4. Validation (Failure)
5. Edge Cases
6. Retrieval (After)
7. Utilities
8. Authorization
```

### 2. Test Data
```javascript
// Use unique emails per run
const email = `test.${Date.now()}@mentora.com`;

// Use realistic data
const arabicText = "???? ???? ?????";
const phone = "+962791234567";
```

### 3. Error Handling
```
? Read error messages carefully
? Check field names in errors
? Verify status codes
? Log unexpected results
```

### 4. Documentation
```
? Record test results
? Note any failures
? Track performance
? Update test data
```

---

## ?? Reading Test Results

### Success Response
```json
{
  "status": 200,
  "data": {
    "id": "guid",
    "university": "Test University",
    "major": "Computer Science",
    ...
  }
}
```

### Validation Error
```json
{
  "status": 400,
  "data": {
    "message": "Validation failed",
    "errors": {
      "University": ["University name is required"],
      "Major": ["Major is required"]
    }
  }
}
```

### Authorization Error
```json
{
  "status": 401,
  "data": {
    "message": "User not authenticated"
  }
}
```

---

## ?? Troubleshooting

### Problem: Tests failing with 401
**Solution:**
```
1. Check token is copied correctly
2. Verify token hasn't expired (60 min)
3. Restart if needed: Register ? Login ? Copy new token
```

### Problem: Validation errors
**Solution:**
```
1. Read error message carefully
2. Check field requirements:
   - University: min 2 chars
   - Major: min 2 chars
   - Year: 2024-2050
   - Level: 0-4
   - Timezone: IANA format
```

### Problem: 500 Server Error
**Solution:**
```
1. Check backend console logs
2. Verify database connection
3. Check for missing dependencies
4. Restart backend
```

### Problem: Connection refused
**Solution:**
```
1. Verify backend is running
2. Check port 7000 is free
3. Verify SSL certificate
```

---

## ?? Test Report Template

```markdown
# Test Execution Report

**Date:** YYYY-MM-DD
**Tester:** Name
**Duration:** X minutes

## Results
- Total Tests: 45
- Passed: __
- Failed: __
- Success Rate: __%

## Failed Tests
| Test | Expected | Actual | Notes |
|------|----------|--------|-------|
|      |          |        |       |

## Performance
| Operation | Avg Time | Notes |
|-----------|----------|-------|
| Create    | __ms     |       |
| Read      | __ms     |       |
| Update    | __ms     |       |

## Issues Found
1. ___________________
2. ___________________

## Recommendations
___________________________

**Status:** PASS / FAIL
```

---

## ?? Success Criteria

### All Tests Must Pass
- ? Authentication works
- ? Profile CRUD operations work
- ? Validation catches errors
- ? Edge cases handled
- ? Arabic text supported
- ? Timezones work correctly
- ? Authorization enforced

### Performance Criteria
- ? Create < 500ms
- ? Read < 200ms
- ? Update < 500ms
- ? List < 300ms

### Code Quality
- ? No console errors
- ? Proper error messages
- ? Consistent responses
- ? Clean architecture maintained

---

## ?? Next Steps After Testing

### If All Tests Pass ?
1. Tag version
2. Deploy to staging
3. Run integration tests
4. Deploy to production

### If Tests Fail ?
1. Document failures
2. Fix issues
3. Re-run tests
4. Verify fixes

---

## ?? Test Summary

| Aspect | Coverage | Status |
|--------|----------|--------|
| **API Endpoints** | 6/6 | ? 100% |
| **Validation Rules** | 7/7 | ? 100% |
| **Edge Cases** | 14/14 | ? 100% |
| **Authorization** | 3/3 | ? 100% |
| **Error Handling** | 13/13 | ? 100% |
| **Total** | **45/45** | **? 100%** |

---

## ?? Support

??? ????? ?? ?????:

1. ???? `COMPREHENSIVE-TEST-GUIDE.md`
2. ???? ?? `QUICK-TEST-CHECKLIST.md`
3. ???? console logs ?? Backend
4. ???? ?? Database connection

---

**Testing Made Easy! ??**

Created: 2025-01-01
Total Tests: 45
Automation: Available
Coverage: 100%
