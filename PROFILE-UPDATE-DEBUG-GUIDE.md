# Profile Update Debugging Guide

## Changes Made

### 1. Enhanced Error Handling in ProfileContext.jsx
- Extracts detailed validation errors from server response
- Formats error messages with field names and specific validation issues
- Displays both general errors and field-specific validation errors

### 2. Improved Logging in userProfileService.js
- Logs the exact data being sent to the API
- Logs study level conversion (string ? enum)
- Captures and logs detailed error responses including:
  - HTTP status code
  - Status text
  - Response data (validation errors)
  - Data that was sent

### 3. Frontend Validation in Profile.jsx
- Validates required fields before sending to API:
  - University (required)
  - Major (required)
  - Expected Graduation Year (2024-2050)
  - Current Level (required)
  - Timezone (required)
- Validates URL formats for social links
- Shows friendly error messages before API call

## Debugging Steps

### Step 1: Check Browser Console
When you click "Save", you should now see:

```
Saving profile with data: { ... }
Required fields check: { university: "...", major: "...", ... }
Sending profile data: { ... }
Study level conversion: { original: "Junior", converted: 2 }
```

**Look for:**
- Are all required fields present?
- Is `currentLevel` being converted to a number (0-4)?
- Are there any frontend validation errors?

### Step 2: Check Network Tab
Open DevTools ? Network tab ? Filter by "UserProfile"

**Click on the failed request and check:**

1. **Request Headers:**
   - Content-Type: `application/json; charset=utf-8`
   - Authorization: `Bearer <token>`

2. **Request Payload:**
   ```json
   {
     "bio": "...",
     "location": "...",
     "phoneNumber": "+962791234567",
     "dateOfBirth": "2000-05-15",
     "university": "...",
     "major": "...",
     "expectedGraduationYear": 2026,
     "currentLevel": 2,
     "timezone": "Asia/Amman",
     "linkedInUrl": "https://linkedin.com/...",
     "gitHubUrl": "https://github.com/...",
     "avatarUrl": ""
   }
   ```

3. **Response (400 Bad Request):**
   ```json
   {
     "message": "Validation failed",
     "errors": {
       "FieldName": ["Error message 1", "Error message 2"]
     }
   }
   ```

### Step 3: Common Validation Errors

#### Error: "University is required"
**Cause:** Empty or null university field
**Fix:** Make sure the university field is filled

#### Error: "Invalid phone number format"
**Cause:** Phone number doesn't match expected format
**Fix:** Use format like `+962791234567` (with country code)

#### Error: "Graduation year must be between 2024 and 2050"
**Cause:** Year is outside the valid range
**Fix:** Enter a year between 2024-2050

#### Error: "Invalid timezone format"
**Cause:** Timezone is not in IANA format
**Fix:** Use format like `Asia/Amman`, `UTC`, `America/New_York`

#### Error: "Invalid LinkedIn URL format"
**Cause:** URL doesn't start with http:// or https://
**Fix:** Use full URL: `https://linkedin.com/in/username`

### Step 4: Arabic Text Issues

The console showing `????` is just a **display issue**, not an encoding problem. The actual data is sent correctly.

**To verify:**
1. Check Network tab ? Request Payload
2. You should see proper UTF-8 encoded text
3. If still showing `????`, it means the browser console can't display it, but the server receives it correctly

**Test with English text first:**
- If English works but Arabic fails ? encoding issue
- If both fail ? validation issue (not encoding)

### Step 5: Test with Minimal Data

Try saving with only required fields:

```json
{
  "university": "Test University",
  "major": "Test Major",
  "expectedGraduationYear": 2026,
  "currentLevel": "Junior",
  "timezone": "UTC"
}
```

If this works, gradually add optional fields to find which one causes the error.

## Expected Console Output (Success)

```
Saving profile with data: { university: "...", major: "...", ... }
Required fields check: { university: "...", major: "...", ... }
Sending profile data: { university: "...", major: "...", currentLevel: 2, ... }
Study level conversion: { original: "Junior", converted: 2 }
Profile update successful: { id: "...", university: "...", ... }
```

## Expected Console Output (Failure)

```
Saving profile with data: { university: "", major: "Test", ... }
Required fields check: { university: "", major: "Test", ... }
Alert: Please fix the following errors:
University is required
```

OR (if backend validation fails):

```
Sending profile data: { ... }
Profile update failed: {
  status: 400,
  statusText: "Bad Request",
  data: {
    message: "Validation failed",
    errors: {
      "University": ["University is required"],
      "Timezone": ["Invalid timezone format: XYZ"]
    }
  },
  sentData: { ... }
}
Error updating profile: AxiosError { ... }
Alert: Failed to update profile:
Validation failed:
University: University is required
Timezone: Invalid timezone format: XYZ
```

## Quick Fixes

### If you see "University is required" but field is filled:
- Check if there are trailing spaces
- Try clearing the field and re-typing

### If timezone validation fails:
- Use the dropdown (loads valid IANA timezones)
- Or manually enter: `UTC`, `Asia/Amman`, `America/New_York`

### If phone validation fails:
- Include country code: `+962791234567`
- No spaces or special characters except `+`

### If URL validation fails:
- Must start with `http://` or `https://`
- Leave empty if you don't have a profile

## Testing Checklist

- [ ] Required fields have values
- [ ] Expected graduation year is 2024-2050
- [ ] Study level is selected from dropdown
- [ ] Timezone is selected from dropdown
- [ ] Phone starts with `+` if provided
- [ ] URLs start with `http://` or `https://` if provided
- [ ] Check browser console for detailed errors
- [ ] Check Network tab for request/response details

## Still Having Issues?

1. **Copy the full console output** (including all logs)
2. **Copy the Network tab Request Payload**
3. **Copy the Network tab Response**
4. **Share these three things** for further debugging

The enhanced error handling now provides exact validation errors, making it easy to identify and fix issues.
