# ?? Profile Update - Debugging Guide

## ?? Issue Reported

**Error**: 400 Bad Request when updating profile  
**Request**: `PUT https://localhost:7000/api/UserProfile`  
**Problem**: Arabic text showing as `????` in request

---

## ? Fixes Applied

### 1. **UTF-8 Encoding - Frontend**
**File**: `Client/src/services/api.js`

**Fixed**:
```javascript
headers: {
    'Content-Type': 'application/json; charset=utf-8',
    'Accept': 'application/json',
    'Accept-Language': 'en,ar'
}
```

**Why**: Ensures Arabic text is encoded properly in HTTP requests

---

### 2. **UTF-8 Support - Backend**
**File**: `Server/src/Mentora.Api/Program.cs`

**Fixed**:
```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = 
            System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });
```

**Why**: Allows backend to properly handle Unicode characters (Arabic)

---

### 3. **Timezone Validation - DTO**
**File**: `Server/src/Mentora.Application/DTOs/UserProfile/UpdateUserProfileDto.cs`

**Fixed**:
```csharp
[RegularExpression(@"^(UTC|[A-Za-z_]+/[A-Za-z_]+)$", 
    ErrorMessage = "Timezone must be UTC or in IANA format (e.g., Asia/Amman)")]
```

**Why**: Previous regex didn't accept "UTC" - now accepts both UTC and IANA format

---

### 4. **Better Error Messages - Controller**
**File**: `Server/src/Mentora.Api/Controllers/UserProfileController.cs`

**Fixed**:
```csharp
if (!ModelState.IsValid)
{
    var errors = ModelState
        .Where(x => x.Value?.Errors.Count > 0)
        .ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
        );
    
    return BadRequest(new 
    { 
        message = "Validation failed",
        errors = errors
    });
}
```

**Why**: Returns detailed validation errors to help debug issues

---

## ?? Testing Steps

### Step 1: Restart Backend
```bash
# Stop current backend (Ctrl+C)
cd Server/src/Mentora.Api
dotnet run
```

**Expected**: Server starts on port 7000 without errors

---

### Step 2: Clear Frontend Cache
```bash
# In browser DevTools
# Application tab ? Clear storage ? Clear site data

# Or restart frontend
cd Client
npm run dev
```

**Expected**: Frontend starts on port 8000

---

### Step 3: Test with English Text First
```json
{
  "bio": "Passionate Computer Science student",
  "location": "Amman, Jordan",
  "phoneNumber": "+962791234567",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

**Expected**: 200 OK response

---

### Step 4: Test with Arabic Text
```json
{
  "bio": "???? ???? ????? ???? ??????? ???????? ???????",
  "location": "?????? ??????",
  "university": "??????? ????????",
  "major": "???? ???????",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

**Expected**: 200 OK response with Arabic text preserved

---

### Step 5: Test with Mixed Text
```json
{
  "bio": "CS Student | ???? ???? ?????",
  "location": "Amman - ?????",
  "university": "University of Jordan - ??????? ????????",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

**Expected**: 200 OK response with both English and Arabic text

---

## ?? Verification Checklist

### Frontend (Browser DevTools)

1. **Network Tab**
   - [x] Request header has: `Content-Type: application/json; charset=utf-8`
   - [x] Request payload shows Arabic text correctly (not ????)
   - [x] Response status: 200 OK
   - [x] Response body has Arabic text correctly

2. **Console Tab**
   - [x] No encoding errors
   - [x] No CORS errors
   - [x] No validation errors

3. **Application Tab**
   - [x] Check localStorage has valid tokens
   - [x] Token not expired

---

### Backend (Swagger/API)

1. **Test via Swagger**
   ```
   https://localhost:7000/swagger
   ```
   
   - [x] PUT /api/UserProfile endpoint visible
   - [x] Can authorize with token
   - [x] Request accepts Arabic text
   - [x] Response shows Arabic correctly

2. **Check Logs**
   ```bash
   # In backend terminal
   ```
   - [x] No encoding exceptions
   - [x] No validation errors
   - [x] Request processed successfully

---

## ?? Common Issues & Solutions

### Issue 1: Still Seeing `????` in Request

**Cause**: Browser cache not cleared  
**Solution**:
1. Hard refresh: `Ctrl + Shift + R`
2. Clear cache: DevTools ? Application ? Clear storage
3. Restart frontend dev server

---

### Issue 2: 400 Bad Request with Timezone Error

**Cause**: Timezone validation failing  
**Check**:
- Timezone value is "UTC" or IANA format (e.g., "Asia/Amman")
- Not empty string
- Not undefined

**Solution**: Make sure timezone dropdown is populated and selected

---

### Issue 3: Still Getting Validation Errors

**Check Response Body**:
```json
{
  "message": "Validation failed",
  "errors": {
    "University": ["The University field is required."],
    "Major": ["The Major field is required."]
  }
}
```

**Solution**: Make sure all required fields are filled:
- University (required)
- Major (required)
- ExpectedGraduationYear (required)
- CurrentLevel (required)
- Timezone (required)

---

### Issue 4: Arabic Text Lost After Save

**Check**:
1. Database column encoding: Should be `NVARCHAR` (not VARCHAR)
2. Backend JSON serialization: Should use UTF-8
3. Frontend display: Should not escape Unicode

**Verify in Database**:
```sql
SELECT Bio, Location, University, Major 
FROM UserProfiles 
WHERE UserId = 'your-user-id'
```

Arabic text should appear correctly in database

---

## ?? Expected Behavior

### Successful Request
```http
PUT https://localhost:7000/api/UserProfile
Authorization: Bearer {token}
Content-Type: application/json; charset=utf-8

{
  "bio": "???? ???? ?????",
  "university": "??????? ????????",
  "major": "???? ???????",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

### Successful Response (200 OK)
```json
{
  "id": "guid",
  "userId": "guid",
  "bio": "???? ???? ?????",
  "university": "??????? ????????",
  "major": "???? ???????",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "currentLevelName": "Junior",
  "timezone": "Asia/Amman",
  "completionPercentage": 85,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-20T10:30:00Z"
}
```

---

## ?? Test Data

### English Profile
```json
{
  "bio": "Passionate developer",
  "location": "Amman, Jordan",
  "phoneNumber": "+962791234567",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

### Arabic Profile
```json
{
  "bio": "???? ???? ????????",
  "location": "?????? ??????",
  "phoneNumber": "+962791234567",
  "university": "??????? ????????",
  "major": "???? ???????",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

### Mixed Profile
```json
{
  "bio": "Developer | ???? ???????",
  "location": "Amman - ?????",
  "phoneNumber": "+962791234567",
  "university": "UJ - ??????? ????????",
  "major": "CS - ???? ???????",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

---

## ?? Quick Fix Commands

### Restart Everything
```bash
# Terminal 1: Backend
cd Server/src/Mentora.Api
dotnet clean
dotnet build
dotnet run

# Terminal 2: Frontend
cd Client
npm run dev
```

### Clear All Caches
```bash
# Frontend
cd Client
rm -rf node_modules/.vite
npm run dev

# Browser
# Hard refresh: Ctrl + Shift + R
# Clear storage: DevTools ? Application ? Clear
```

---

## ?? Validation Rules Reference

| Field | Required | Format | Example |
|-------|----------|--------|---------|
| University | ? | String (max 200) | "University of Jordan" |
| Major | ? | String (max 200) | "Computer Science" |
| ExpectedGraduationYear | ? | Number (2024-2050) | 2025 |
| CurrentLevel | ? | Enum | "Junior" |
| Timezone | ? | UTC or IANA | "Asia/Amman" |
| Bio | ? | String (max 500) | "Passionate developer" |
| Location | ? | String (max 100) | "Amman, Jordan" |
| PhoneNumber | ? | Phone format | "+962791234567" |
| LinkedInUrl | ? | Valid URL | "https://linkedin.com/in/user" |
| GitHubUrl | ? | Valid URL | "https://github.com/user" |

---

## ? Success Indicators

After fixes applied, you should see:

1. ? **Frontend Request**
   - Arabic text visible in Network tab payload
   - No `????` characters
   - Content-Type includes `charset=utf-8`

2. ? **Backend Response**
   - Status 200 OK
   - Arabic text in response body
   - No validation errors

3. ? **Database**
   - Arabic text stored correctly
   - Viewable in SQL Server Management Studio

4. ? **UI Display**
   - Arabic text displays correctly
   - No placeholder characters
   - RTL text direction (if needed)

---

**Last Updated**: 2024-12-31  
**Fix Status**: ? Applied  
**Test Status**: ?? Ready for Testing
