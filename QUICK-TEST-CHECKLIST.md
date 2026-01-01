# ? Quick Test Checklist - User Profile

## ?? Pre-Test Setup

- [ ] Backend running on `https://localhost:7000`
- [ ] Database connected and clean
- [ ] REST Client installed (VS Code) OR Postman ready

---

## ?? Manual Testing (5 minutes)

### 1. Authentication (2 min)
- [ ] Register: `POST /Auth/register`
  ```json
  {
    "firstName": "Test",
    "lastName": "User",
    "email": "quick.test@mentora.com",
    "password": "Test@123456",
    "confirmPassword": "Test@123456"
  }
  ```
- [ ] Login: `POST /Auth/login`
- [ ] Copy `accessToken`

### 2. Profile CRUD (3 min)
- [ ] **Create Profile** ? Expected: 200 OK
  ```json
  {
    "university": "Test University",
    "major": "Computer Science",
    "expectedGraduationYear": 2026,
    "currentLevel": 2,
    "timezone": "UTC"
  }
  ```

- [ ] **Get Profile** ? Expected: 200 OK with data
  ```http
  GET /UserProfile
  ```

- [ ] **Update Profile** ? Expected: 200 OK
  ```json
  {
    "bio": "Updated bio",
    "university": "Updated University",
    "major": "Updated Major",
    "expectedGraduationYear": 2027,
    "currentLevel": 3,
    "timezone": "Asia/Amman"
  }
  ```

### 3. Validation (Quick Check)
- [ ] **Missing University** ? Expected: 400
- [ ] **Invalid Year** ? Expected: 400
- [ ] **Invalid Level** ? Expected: 400

---

## ?? Automated Testing (1 minute)

### Run Automated Script
```bash
node test-profile-api.js
```

**Expected Output:**
```
? Register new user
? Login user
? Get profile (should be 404)
? Check exists (should be false)
? Create complete profile
...
? Testing completed!
Total: 30+ tests
Passed: 30+
Failed: 0
Success Rate: 100%
```

---

## ?? Quick Status Check

| Component | Status | Notes |
|-----------|--------|-------|
| **Backend** | ? | Running on 7000? |
| **Auth** | ? | Register/Login OK? |
| **Create** | ? | Profile created? |
| **Read** | ? | Profile retrieved? |
| **Update** | ? | Profile updated? |
| **Validation** | ? | Errors caught? |

---

## ?? Quick Troubleshooting

| Problem | Quick Fix |
|---------|-----------|
| 401 Unauthorized | Check token copied correctly |
| 400 Validation | Check required fields |
| 500 Server Error | Check backend logs |
| Connection Refused | Backend not running? |

---

## ? Success Criteria

All must pass:
- ? Can create profile with minimal data
- ? Can retrieve profile
- ? Can update profile
- ? Validation catches missing fields
- ? Validation catches invalid values
- ? Arabic text works correctly

**Total Time: ~6 minutes**

---

## ?? Report Results

**Date:** _____________  
**Tester:** _____________

**Results:**
- Manual Tests: ___/6 passed
- Automated Tests: ___/30 passed
- Issues Found: _____________

**Status:** ? PASS  ? FAIL

**Notes:**
_________________________________
_________________________________

---

**Quick, Simple, Effective! ??**
