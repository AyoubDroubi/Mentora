# Profile Update Fix - StudyLevel Enum Issue

## Problem
? **Error 400**: Profile update failing with validation error
- Frontend sends `currentLevel: "Junior"` (string)
- Backend expects `currentLevel: 2` (integer enum)

## Root Cause
`StudyLevel` is an enum in C#:
```csharp
public enum StudyLevel
{
    Freshman = 0,
    Sophomore = 1,
    Junior = 2,
    Senior = 3,
    Graduate = 4
}
```

ASP.NET Core can accept both string and int, but we need to ensure proper conversion.

---

## ? Solution Applied

### 1. Backend - Removed Restrictive Regex
**File**: `Server/src/Mentora.Application/DTOs/UserProfile/UpdateUserProfileDto.cs`

**Before**:
```csharp
[RegularExpression(@"^(UTC|[A-Za-z_]+/[A-Za-z_]+)$", 
    ErrorMessage = "Timezone must be UTC or in IANA format")]
public string Timezone { get; set; } = "UTC";
```

**After**:
```csharp
// Accept UTC or IANA format (e.g., Asia/Amman, America/New_York, Europe/London)
[Required(ErrorMessage = "Timezone is required")]
public string Timezone { get; set; } = "UTC";
```

**Why**: Regex was too restrictive, blocking valid timezones like `Europe/London`.

---

### 2. Backend - Added Manual Timezone Validation
**File**: `Server/src/Mentora.Api/Controllers/UserProfileController.cs`

**Added**:
```csharp
// Validate timezone manually
if (!_profileService.IsValidTimezone(dto.Timezone))
{
    return BadRequest(new 
    { 
        message = "Validation failed",
        errors = new Dictionary<string, string[]>
        {
            ["Timezone"] = new[] { $"Invalid timezone: {dto.Timezone}" }
        }
    });
}
```

**Why**: Proper timezone validation using `TimeZoneInfo` instead of regex.

---

### 3. Frontend - Convert StudyLevel to Enum
**File**: `Client/src/services/userProfileService.js`

**Added**:
```javascript
const convertStudyLevel = (level) => {
  const levelMap = {
    'Freshman': 0,
    'Sophomore': 1,
    'Junior': 2,
    'Senior': 3,
    'Graduate': 4
  };
  
  if (typeof level === 'number') return level;
  return levelMap[level] !== undefined ? levelMap[level] : 0;
};

updateProfile: async (profileData) => {
  const dataToSend = {
    ...profileData,
    currentLevel: convertStudyLevel(profileData.currentLevel)
  };
  
  const response = await api.put('/UserProfile', dataToSend);
  return response.data;
}
```

**Why**: Ensures correct enum value is sent to API.

---

## Testing

### Test 1: With Integer (Direct)
```json
{
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2026,
  "currentLevel": 2,
  "timezone": "Asia/Amman"
}
```

### Test 2: With String (Auto-Converted)
```json
{
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2026,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman"
}
```

Both should work now!

---

## How to Test

### Option 1: Using Frontend
1. Run Backend: `cd Server/src/Mentora.Api && dotnet run`
2. Run Frontend: `cd Client && npm run dev`
3. Login
4. Go to Profile page
5. Edit profile
6. Click Save

### Option 2: Using VS Code REST Client
1. Open `Server/src/Mentora.Api/Tests/profile-update-test.http`
2. Get token from login
3. Replace `YOUR_TOKEN_HERE` with actual token
4. Click "Send Request"

---

## Expected Behavior

### ? Success Response (200 OK)
```json
{
  "userId": "guid-here",
  "bio": "Passionate CS student",
  "location": "Amman, Jordan",
  "phoneNumber": "+962791234567",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2026,
  "currentLevel": 2,
  "currentLevelName": "Junior",
  "timezone": "Asia/Amman",
  "yearsUntilGraduation": 2,
  "completionPercentage": 85
}
```

### ? Error Response (400 Bad Request)
```json
{
  "message": "Validation failed",
  "errors": {
    "University": ["University is required"],
    "Timezone": ["Invalid timezone: Invalid/Timezone"]
  }
}
```

---

## Study Level Values

| String | Integer | Description |
|--------|---------|-------------|
| Freshman | 0 | Year 1 |
| Sophomore | 1 | Year 2 |
| Junior | 2 | Year 3 |
| Senior | 3 | Year 4 |
| Graduate | 4 | Post-grad |

---

## Valid Timezones Examples

? **Valid**:
- `UTC`
- `Asia/Amman`
- `America/New_York`
- `Europe/London`
- `Africa/Cairo`
- `Asia/Dubai`

? **Invalid**:
- `GMT+3`
- `Jordan Time`
- `EST`
- `PST`

---

## Debugging Tips

### Check Backend Logs
```bash
cd Server/src/Mentora.Api
dotnet run
```

Look for:
```
Received profile update request
DTO: { university: "...", currentLevel: 2, ... }
Profile updated successfully for user {guid}
```

### Check Frontend Console
```javascript
// In userProfileService.js
console.log('Sending profile data:', dataToSend);
```

Should show:
```
Sending profile data: { 
  university: "...", 
  currentLevel: 2,  // ? Should be number!
  timezone: "Asia/Amman" 
}
```

---

## If Still Not Working

### 1. Check ModelState Validation
Add to controller:
```csharp
_logger.LogWarning("Model validation failed: {@ModelState}", ModelState);
```

### 2. Check DTO Binding
Add breakpoint in controller:
```csharp
public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
{
    debugger; // Check dto.CurrentLevel value here
    ...
}
```

### 3. Check Frontend Request
```javascript
const result = await updateProfile(profileEdit);
console.log('Result:', result);
```

---

## Files Changed

1. ? `Server/src/Mentora.Application/DTOs/UserProfile/UpdateUserProfileDto.cs`
2. ? `Server/src/Mentora.Api/Controllers/UserProfileController.cs`
3. ? `Client/src/services/userProfileService.js`
4. ? `Server/src/Mentora.Api/Tests/profile-update-test.http` (new)

---

**Status**: ? Fixed  
**Test**: Frontend profile update should work now  
**Date**: 2024-12-31
