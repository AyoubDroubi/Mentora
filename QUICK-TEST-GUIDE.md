# ?? Quick Test Guide - Update Profile Feature

## ? Verified Configuration

### Backend API Routes
? Controller: `UserProfileController`  
? Base Route: `/api/UserProfile` (capital U and P)  
? Port: `https://localhost:7000`

### Frontend API Service
? Service: `userProfileService.js`  
? Base URL: `https://localhost:7000/api`  
? Endpoints: All using `/UserProfile`

---

## ?? Quick Test Steps

### 1. Start Backend
```bash
cd Server/src/Mentora.Api
dotnet run
```

**Expected**:
```
Now listening on: https://localhost:7000
info: Microsoft.Hosting.Lifetime[0]
      Application started.
```

**Verify**: Open `https://localhost:7000/swagger`

---

### 2. Start Frontend
```bash
cd Client
npm run dev
```

**Expected**:
```
VITE v4.4.9  ready in XXX ms
?  Local:   http://localhost:8000/
```

**Verify**: Browser opens at `http://localhost:8000`

---

### 3. Test Update Profile (Manual)

#### Step 1: Login
1. Navigate to: `http://localhost:8000/login`
2. Use test account:
   - Email: `saad@mentora.com`
   - Password: `Saad@123`
3. Should redirect to dashboard

#### Step 2: Go to Profile
1. Navigate to: `http://localhost:8000/profile`
2. Should see profile data (if exists)
3. Profile completion bar should show percentage

#### Step 3: Edit Profile
1. Click **"Edit Profile"** button
2. Form fields become editable
3. Modify any field (e.g., Bio, University, etc.)

#### Step 4: Save Changes
1. Click **"Save"** button
2. Should see loading spinner
3. Should see success message
4. Form should return to view mode
5. Changes should be visible

#### Step 5: Verify Persistence
1. Refresh page (F5)
2. Changes should still be there
3. Navigate away and back
4. Data should persist

---

### 4. Test with DevTools

#### Open Browser Console
1. Right-click ? Inspect ? Console
2. Check for errors (should be none)

#### Check Network Tab
1. Go to Network tab
2. Filter by "UserProfile"
3. Edit and save profile
4. Should see:
   - `PUT https://localhost:7000/api/UserProfile`
   - Status: `200 OK`
   - Response: Profile data

---

### 5. Test API Directly (Swagger)

#### Open Swagger UI
`https://localhost:7000/swagger`

#### Test PUT /api/UserProfile
1. Click on **PUT /api/UserProfile**
2. Click **"Try it out"**
3. Click **"Authorize"** button
4. Login to get token
5. Paste token in Authorization field
6. Modify request body:
```json
{
  "university": "Test University",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman",
  "bio": "Test bio from Swagger",
  "location": "Amman, Jordan"
}
```
7. Click **"Execute"**
8. Should see **Response Code: 200**

---

### 6. Test with HTTP File

#### Using VS Code REST Client
1. Open: `Server/src/Mentora.Api/Tests/userprofile-tests.http`
2. Find test #6 (Login)
3. Click "Send Request"
4. Copy `accessToken` from response
5. Paste at top: `@accessToken = your-token-here`
6. Find test #10 (Update Profile)
7. Click "Send Request"
8. Should see 200 OK with updated profile

---

## ?? Verification Checklist

### Backend Health
- [ ] API running on port 7000
- [ ] Swagger accessible
- [ ] Database migrations applied
- [ ] Test users seeded

### Frontend Health
- [ ] App running on port 8000
- [ ] Can access login page
- [ ] Can navigate to profile
- [ ] No console errors

### API Integration
- [ ] Login works
- [ ] Profile loads
- [ ] Edit mode works
- [ ] Save button works
- [ ] Data persists
- [ ] Completion updates

### Network Requests
- [ ] `GET /api/UserProfile` returns data
- [ ] `PUT /api/UserProfile` returns 200
- [ ] No CORS errors
- [ ] Authorization header present

---

## ?? Common Issues & Fixes

### Issue 1: 401 Unauthorized
**Cause**: Invalid or expired token  
**Fix**:
1. Logout and login again
2. Check localStorage has `accessToken`
3. Check token in request headers

### Issue 2: 404 Not Found
**Cause**: Wrong endpoint path  
**Fix**:
- Frontend should use `/UserProfile` (capital U and P)
- Backend controller uses `[Route("api/[controller]")]`

### Issue 3: 400 Bad Request
**Cause**: Validation error  
**Check**:
- University is required
- Major is required
- ExpectedGraduationYear is required (2024-2050)
- CurrentLevel is valid enum
- Timezone is valid IANA format

### Issue 4: CORS Error
**Cause**: Port not in CORS policy  
**Fix**:
1. Check `Program.cs` has port 8000
2. Restart backend
3. Clear browser cache

### Issue 5: Changes Don't Save
**Cause**: Network error or validation  
**Check**:
1. Browser console for errors
2. Network tab for failed request
3. Response body for error message

---

## ?? Expected API Responses

### Successful Update (200 OK)
```json
{
  "id": "guid",
  "userId": "guid",
  "bio": "Updated bio",
  "location": "Amman, Jordan",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "currentLevelName": "Junior",
  "timezone": "Asia/Amman",
  "completionPercentage": 85,
  "yearsUntilGraduation": 1,
  "age": 24,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-20T10:30:00Z"
}
```

### Validation Error (400 Bad Request)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "University": ["The University field is required."]
  }
}
```

### Unauthorized (401)
```json
{
  "message": "Unauthorized"
}
```

---

## ?? Success Criteria

? Backend runs without errors  
? Frontend runs without errors  
? Login works correctly  
? Profile page loads  
? Edit mode activates  
? All fields are editable  
? Save button works  
? Success message appears  
? Data persists after refresh  
? Completion percentage updates  
? No console errors  
? No CORS errors  
? API returns 200 OK  

---

## ?? Test Data

### Test User 1 (Saad)
```
Email: saad@mentora.com
Password: Saad@123
Profile: Senior, Computer Science, University of Jordan
```

### Test User 2 (Maria)
```
Email: maria@mentora.com
Password: Maria@123
Profile: Graduate, Data Science, American University of Dubai
```

### Sample Update Data
```json
{
  "bio": "Passionate developer",
  "location": "Amman, Jordan",
  "phoneNumber": "+962791234567",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman",
  "linkedInUrl": "https://linkedin.com/in/test",
  "gitHubUrl": "https://github.com/test"
}
```

---

## ?? Related Files

### Backend
- `Server/src/Mentora.Api/Controllers/UserProfileController.cs`
- `Server/src/Mentora.Infrastructure/Services/UserProfileService.cs`
- `Server/src/Mentora.Application/DTOs/UserProfile/UpdateUserProfileDto.cs`

### Frontend
- `Client/src/services/userProfileService.js`
- `Client/src/contexts/ProfileContext.jsx`
- `Client/src/pages/Profile.jsx`

### Tests
- `Server/src/Mentora.Api/Tests/userprofile-tests.http`
- `Client/FRONTEND-TESTING-GUIDE.md`

---

**Last Updated**: 2024-12-31  
**Test Status**: ? Ready for Testing  
**Configuration**: ? Verified
