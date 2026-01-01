# Frontend Integration Testing Guide - Module 2: User Profile

## Prerequisites
1. Backend API running on `https://localhost:7001`
2. Frontend running on `http://localhost:5173`
3. User registered and logged in

## Test Scenario 1: View Profile (Empty State)

### Steps:
1. Login with new user account
2. Navigate to `/profile`
3. Verify that profile shows:
   - ? User name and email from auth
   - ? "Please complete your academic profile" messages
   - ? 0% completion bar
   - ? Edit button visible

**Expected Result**: Profile page loads with empty fields ready for editing

---

## Test Scenario 2: Complete Profile (First Time)

### Steps:
1. Click "Edit" button
2. Fill in all required fields:
   - University: "University of Jordan"
   - Major: "Computer Science"
   - Expected Graduation Year: 2025
   - Study Level: "Junior"
   - Timezone: "Asia/Amman"
3. Fill in optional fields:
   - Bio: "Test bio"
   - Location: "Amman, Jordan"
   - Phone: "+962791234567"
   - LinkedIn: "https://linkedin.com/in/test"
4. Click "Save"

**Expected Result**:
- ? Success message appears
- ? Profile data saved to database
- ? Completion percentage updates
- ? Form switches back to view mode

---

## Test Scenario 3: Profile Completion Indicator

### Steps:
1. After saving, check completion percentage
2. Should be around 85-90% if all fields filled
3. Navigate away and back to profile
4. Completion should persist

**Expected Result**: Completion percentage accurately reflects filled fields

---

## Test Scenario 4: Edit Existing Profile

### Steps:
1. Click "Edit" button again
2. Modify some fields:
   - Change Major to "Software Engineering"
   - Change Study Level to "Senior"
3. Click "Save"

**Expected Result**:
- ? Changes saved successfully
- ? Updated data appears in view mode

---

## Test Scenario 5: Timezone Selection

### Steps:
1. Click "Edit"
2. Check timezone dropdown
3. Enter "Jordan" in location field
4. Wait for timezone suggestions to update
5. Select "Asia/Amman"
6. Save

**Expected Result**:
- ? Location-based timezone suggestions work
- ? Selected timezone persists

---

## Test Scenario 6: Validation Tests

### Test 6.1: Missing Required Fields
1. Click "Edit"
2. Clear "University" field
3. Try to save

**Expected**: Browser validation error (HTML5 required)

### Test 6.2: Invalid Graduation Year
1. Enter year "2060"
2. Try to save

**Expected**: Backend returns 400 error with validation message

### Test 6.3: Invalid URL Format
1. Enter "not-a-url" in LinkedIn field
2. Try to save

**Expected**: Browser validation error (HTML5 URL type)

---

## Test Scenario 7: Cancel Edit

### Steps:
1. Click "Edit"
2. Make changes to fields
3. Click "Cancel"

**Expected Result**:
- ? All changes reverted
- ? Original data restored
- ? Form switches to view mode

---

## Test Scenario 8: Profile Persistence

### Steps:
1. Complete profile with all data
2. Logout
3. Login again
4. Navigate to profile

**Expected Result**:
- ? All profile data persists
- ? Completion percentage same as before
- ? Academic attributes displayed correctly

---

## Test Scenario 9: Study Level Display

### Steps:
1. Set Study Level to each value:
   - Freshman
   - Sophomore
   - Junior
   - Senior
   - Graduate
2. Save and verify display

**Expected Result**:
- ? Each level displays correctly in view mode
- ? Appropriate label shown (e.g., "Senior" not just enum value)

---

## Test Scenario 10: Academic Information Display

### Steps:
1. Complete profile with:
   - University: "Harvard University"
   - Major: "Computer Science"
   - Graduation Year: 2025
   - Study Level: "Junior"
2. View profile

**Expected Result**:
- ? "Academic Information" section shows all data
- ? "2 years to graduation" badge appears (if 2025)
- ? Icons display correctly

---

## Test Scenario 11: Social Links

### Steps:
1. Add LinkedIn and GitHub URLs
2. Save
3. Click on the links in view mode

**Expected Result**:
- ? Links open in new tab
- ? Correct URLs loaded
- ? Links display with proper icons

---

## Test Scenario 12: Profile Loading States

### Steps:
1. Clear localStorage
2. Login
3. Navigate to profile
4. Observe loading states

**Expected Result**:
- ? Loading spinner shows while fetching
- ? Profile data loads correctly
- ? No flicker or layout shift

---

## Test Scenario 13: Mobile Responsiveness

### Steps:
1. Open profile on mobile device/DevTools mobile view
2. Test all interactions:
   - View mode
   - Edit mode
   - Save/Cancel buttons
   - Form fields

**Expected Result**:
- ? Layout adjusts for mobile
- ? All buttons accessible
- ? Form fields usable

---

## Test Scenario 14: ProfileContext Integration

### Steps:
1. Open DevTools Console
2. Type: `window.localStorage.getItem('user')`
3. Navigate to Profile
4. Check Network tab for API calls

**Expected Result**:
- ? ProfileContext loads on app start
- ? Profile data fetched from API
- ? Completion percentage calculated
- ? State updates correctly

---

## Test Scenario 15: Profile Onboarding (Dashboard)

### Steps:
1. Create new user and login
2. Navigate to Dashboard
3. Observe ProfileOnboarding banner

**Expected Result**:
- ? Banner appears at top of dashboard
- ? Shows completion percentage
- ? "Complete Profile Now" button works
- ? Banner disappears when profile is 100% complete

---

## API Integration Verification

### Check these API endpoints:

1. **GET /api/userprofile**
   - Returns current user's profile or 404

2. **PUT /api/userprofile**
   - Creates/updates profile
   - Returns updated profile data

3. **GET /api/userprofile/completion**
   - Returns completion percentage (0-100)

4. **GET /api/userprofile/exists**
   - Returns `{ exists: true/false }`

5. **GET /api/userprofile/timezones?location=Jordan**
   - Returns list of timezones

6. **GET /api/userprofile/validate-timezone?timezone=Asia/Amman**
   - Returns `{ isValid: true/false }`

---

## Browser Console Tests

### Test ProfileContext:
```javascript
// Should show profile data
console.log(JSON.parse(localStorage.getItem('user')));

// Check if profile exists
fetch('https://localhost:7001/api/userprofile/exists', {
  headers: {
    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
  }
}).then(r => r.json()).then(console.log);

// Get completion
fetch('https://localhost:7001/api/userprofile/completion', {
  headers: {
    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
  }
}).then(r => r.json()).then(console.log);
```

---

## Common Issues & Solutions

### Issue 1: Profile not loading
**Solution**: Check if user is authenticated and access token is valid

### Issue 2: Timezone dropdown empty
**Solution**: Check API endpoint `/api/userprofile/timezones` is accessible

### Issue 3: Completion percentage not updating
**Solution**: Verify ProfileContext is wrapped around routes in App.jsx

### Issue 4: Save button doesn't work
**Solution**: Check browser console for validation errors or network errors

### Issue 5: Changes don't persist
**Solution**: Verify backend API is saving to database correctly

---

## Success Criteria

? All 15 test scenarios pass  
? No console errors  
? Profile data persists across sessions  
? Validation works correctly  
? Mobile responsive  
? Loading states work  
? API integration complete  

---

## Next Steps After Testing

1. Add ProfileOnboarding to Dashboard
2. Add profile link to navigation menu
3. Implement profile photo upload (optional)
4. Add profile analytics/insights
5. Connect to Career Builder module (Module 3)

---

**Test Date**: ___________  
**Tested By**: ___________  
**Status**: ? Pass | ? Fail | ? Needs Review
