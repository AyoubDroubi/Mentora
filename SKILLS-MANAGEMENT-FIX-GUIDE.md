# Skills Management Feature - Testing Guide

## ? Fixes Implemented

### Backend Fixes

1. **Created Master Skills Controller** (`MasterSkillsController.cs`)
   - `GET /api/skills` - Get all skills with optional filters
   - `GET /api/skills/{id}` - Get specific skill
   - `GET /api/skills/category/{category}` - Get skills by category
   - `GET /api/skills/categories` - Get all categories
   - `POST /api/skills` - Create new skill (authenticated users)

2. **Validation Already in Place**
   - `AddSkillDto` has proper validation attributes
   - `UserProfileSkillService` validates:
     - Skill existence in master Skills table
     - Duplicate prevention
     - Maximum 100 skills per profile
     - Maximum 10 featured skills
     - Years of experience (0-50 range)
     - Proficiency level (0-3 range)

### Frontend Fixes

1. **Created Master Skills Service** (`masterSkillsService.js`)
   - Service for interacting with `/api/skills` endpoints
   - Handles skill library operations

2. **Updated AddSkillModal Component**
   - Loads skills from real API endpoint `/api/skills`
   - Search functionality for skills
   - Category filtering
   - Proper UUID handling
   - Visual skill selection with descriptions
   - Form validation before submission

3. **Verified Skills Management Page**
   - Uses correct `/api/userprofile/skills` endpoints
   - Proper error handling (401, 400, 409)
   - Token handling in API interceptor

---

## ?? Complete Flow

### 1. Backend API Endpoints

#### Master Skills Library (New)
```
GET    /api/skills                     - Get all skills
GET    /api/skills/{id}                - Get specific skill
GET    /api/skills/category/{category} - Get skills by category
GET    /api/skills/categories          - Get categories
POST   /api/skills                     - Create skill
```

#### User Profile Skills (Existing)
```
GET    /api/userprofile/skills                 - Get user's skills
POST   /api/userprofile/skills                 - Add skill to profile
PATCH  /api/userprofile/skills/{id}            - Update skill
DELETE /api/userprofile/skills/{id}            - Delete skill
GET    /api/userprofile/skills/featured        - Get featured skills
PATCH  /api/userprofile/skills/{id}/featured   - Toggle featured
GET    /api/userprofile/skills/summary         - Get statistics
```

### 2. Frontend Services

#### MasterSkillsService (New)
- `getAllSkills(filters)` ? GET /api/skills
- `getSkillById(id)` ? GET /api/skills/{id}
- `getSkillsByCategory(category)` ? GET /api/skills/category/{category}
- `getCategories()` ? GET /api/skills/categories
- `createSkill(data)` ? POST /api/skills

#### SkillsService (Existing)
- `getSkills(filters)` ? GET /api/userprofile/skills
- `addSkill(data)` ? POST /api/userprofile/skills
- `updateSkill(id, data)` ? PATCH /api/userprofile/skills/{id}
- `deleteSkill(id)` ? DELETE /api/userprofile/skills/{id}
- `toggleFeatured(id)` ? PATCH /api/userprofile/skills/{id}/featured
- `getSummary()` ? GET /api/userprofile/skills/summary

---

## ?? Testing Checklist

### Backend Testing

#### Test Master Skills Endpoints
```bash
# 1. Get all skills
GET https://localhost:7000/api/skills

# 2. Get skill by ID
GET https://localhost:7000/api/skills/{skillId}

# 3. Get skills by category
GET https://localhost:7000/api/skills/category/Technical

# 4. Get categories
GET https://localhost:7000/api/skills/categories

# 5. Create new skill (requires auth)
POST https://localhost:7000/api/skills
Authorization: Bearer {token}
{
  "name": "TypeScript",
  "category": "Technical",
  "description": "Typed superset of JavaScript"
}
```

#### Test User Profile Skills Endpoints
```bash
# 1. Add skill to profile
POST https://localhost:7000/api/userprofile/skills
Authorization: Bearer {token}
{
  "skillId": "guid-from-master-skills",
  "proficiencyLevel": 2,
  "acquisitionMethod": "Online Courses",
  "startedDate": "2023-01-15",
  "yearsOfExperience": 2,
  "isFeatured": true,
  "notes": "Built multiple React applications"
}

# Expected: 201 Created with skill details

# 2. Try to add duplicate skill
POST https://localhost:7000/api/userprofile/skills
Authorization: Bearer {token}
{
  "skillId": "same-guid-as-above",
  "proficiencyLevel": 1
}

# Expected: 409 Conflict - "Skill already added to profile"

# 3. Try to add skill with invalid ID
POST https://localhost:7000/api/userprofile/skills
Authorization: Bearer {token}
{
  "skillId": "00000000-0000-0000-0000-000000000000",
  "proficiencyLevel": 2
}

# Expected: 404 Not Found - "Skill not found"

# 4. Get user's skills
GET https://localhost:7000/api/userprofile/skills
Authorization: Bearer {token}

# Expected: 200 OK with array of skills
```

### Frontend Testing

#### Test AddSkillModal Component

1. **Open Skills Management Page**
   ```
   Navigate to: http://localhost:5173/skills-management
   ```

2. **Click "Add Skill" Button**
   - Modal should open
   - Skills library should load from `/api/skills`
   - Categories dropdown should populate

3. **Test Search Functionality**
   - Type "React" in search box
   - Should filter skills containing "React"

4. **Test Category Filter**
   - Select "Technical" category
   - Should show only Technical skills

5. **Test Skill Selection**
   - Click on a skill from the list
   - Selected skill should display in "Selected:" area
   - Background should highlight selected skill

6. **Test Form Validation**
   - Try to submit without selecting a skill
   - Should show error: "Please select a skill from the library"

7. **Test Successful Add**
   - Select a skill
   - Choose proficiency level
   - Fill optional fields
   - Click "Add Skill"
   - Should close modal and refresh skills list

#### Test Skills Management Page

1. **Load User Skills**
   - Should call `/api/userprofile/skills`
   - Display user's added skills
   - Show count: "X / 100 skills • Y / 10 featured"

2. **Test Filters**
   - Filter by proficiency level
   - Filter by featured status
   - Sort by name/proficiency/experience/date

3. **Test Featured Toggle**
   - Click star icon on a skill
   - Should toggle featured status
   - API call: PATCH `/api/userprofile/skills/{id}/featured`

4. **Test Edit Skill**
   - Click "Edit" button
   - Modal opens with pre-filled data
   - Update proficiency level
   - Click "Update Skill"
   - Should update and refresh

5. **Test Delete Skill**
   - Click "Delete" button
   - Confirm deletion
   - API call: DELETE `/api/userprofile/skills/{id}`
   - Skill should be removed from list

6. **Test Analytics View**
   - Click "Analytics" button
   - Should display skills statistics
   - API call: GET `/api/userprofile/skills/summary`

---

## ?? Common Issues & Solutions

### Issue 1: "Failed to load skills library"
**Cause**: Master skills endpoint not accessible or database has no skills
**Solution**:
1. Ensure `MasterSkillsController` is added to project
2. Run database seeder to populate Skills table
3. Check API is running on correct port (7000)

### Issue 2: "Skill not found" when adding
**Cause**: SkillId doesn't exist in Skills master table
**Solution**:
1. Verify skill ID is valid GUID
2. Check Skills table has the skill
3. Use masterSkillsService to get valid skill IDs

### Issue 3: "Maximum 10 featured skills"
**Cause**: User already has 10 featured skills
**Solution**:
1. Unfeature existing skills before adding new one
2. Or add skill without featured flag

### Issue 4: 401 Unauthorized Error
**Cause**: JWT token expired or missing
**Solution**:
1. Check localStorage has 'accessToken'
2. Token is added in API interceptor
3. Refresh token logic works in api.js

### Issue 5: UUID Format Error
**Cause**: Frontend sending string instead of GUID
**Solution**:
- SkillId is now properly validated in AddSkillDto
- Frontend sends UUID as string, backend parses to Guid

---

## ?? Validation Rules

### Backend Validation
- **SkillId**: Required, must exist in Skills table
- **ProficiencyLevel**: Required, 0-3 range
- **AcquisitionMethod**: Optional, max 200 characters
- **YearsOfExperience**: Optional, 0-50 range
- **Notes**: Optional, max 1000 characters
- **IsFeatured**: Boolean, max 10 featured per profile
- **DisplayOrder**: Integer, default 0

### Business Rules
- Maximum 100 skills per user profile
- Maximum 10 featured skills per profile
- No duplicate skills (same UserProfileId + SkillId)
- Skill must exist in master Skills table

---

## ?? Deployment Steps

### Backend
1. Add `MasterSkillsController.cs` to API project
2. Ensure Skills table is seeded with data
3. Rebuild and restart API

### Frontend
1. Add `masterSkillsService.js` to services folder
2. Update `AddSkillModal.jsx` component
3. Rebuild frontend: `npm run build`
4. Restart dev server: `npm run dev`

---

## ? User Experience Flow

### Happy Path
1. User navigates to Skills Management page
2. Clicks "Add Skill" button
3. Modal opens with master skills library
4. User searches/filters skills
5. User selects a skill (e.g., "React")
6. User chooses proficiency level (e.g., "Advanced")
7. User fills optional fields (acquisition method, experience)
8. User clicks "Add Skill"
9. Backend validates:
   - Skill exists in master library ?
   - Not already added ?
   - Under 100 skills limit ?
10. Skill added successfully
11. Modal closes
12. Skills list refreshes with new skill

### Error Path Example
1. User tries to add duplicate skill
2. Backend returns 409 Conflict
3. Frontend displays error: "Skill already added to profile"
4. User can search for different skill

---

## ?? API Response Examples

### Success Response
```json
{
  "success": true,
  "message": "Skill added successfully",
  "data": {
    "id": "guid",
    "userProfileId": "guid",
    "skillId": "guid",
    "skillName": "React",
    "skillCategory": "Technical",
    "proficiencyLevel": 2,
    "proficiencyLevelName": "Advanced",
    "acquisitionMethod": "Online Courses",
    "yearsOfExperience": 2,
    "isFeatured": true,
    "createdAt": "2025-01-20T10:00:00Z"
  }
}
```

### Error Response (409 Conflict)
```json
{
  "success": false,
  "message": "Skill already added to profile"
}
```

### Error Response (404 Not Found)
```json
{
  "success": false,
  "message": "Skill not found"
}
```

---

## ?? Success Criteria

? User can browse master skills library  
? User can search and filter skills  
? User can add skills to profile with validation  
? Duplicate skills are prevented  
? Maximum limits enforced (100 total, 10 featured)  
? UUID/GUID handling works correctly  
? Proper error messages displayed  
? All endpoints respond < 500ms  
? Frontend handles token authentication  
? API validation prevents invalid data  

---

**Last Updated**: 2025-01-20
**Status**: ? Complete
