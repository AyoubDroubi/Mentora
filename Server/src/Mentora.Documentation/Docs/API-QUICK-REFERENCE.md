# API Quick Reference

## ?? Base URLs

| Service | URL |
|---------|-----|
| **Backend API** | `https://localhost:7000/api` |
| **Swagger UI** | `https://localhost:7000/swagger` |
| **Frontend** | `http://localhost:8000` |

---

## ?? Authentication Endpoints

### Register User
```http
POST /api/auth/register

{
  "firstName": "Ahmed",
  "lastName": "Ali",
  "email": "ahmed@mentora.com",
  "password": "Test@123"
}
```

### Login
```http
POST /api/auth/login

{
  "email": "ahmed@mentora.com",
  "password": "Test@123",
  "deviceInfo": "Chrome/Windows"
}

Response:
{
  "accessToken": "eyJ...",
  "refreshToken": "abc...",
  "tokenExpiration": "2024-12-31T11:30:00Z"
}
```

### Get Current User
```http
GET /api/auth/me
Authorization: Bearer {token}

Response:
{
  "userId": "guid",
  "email": "ahmed@mentora.com",
  "firstName": "Ahmed",
  "lastName": "Ali"
}
```

### Refresh Token
```http
POST /api/auth/refresh-token

{
  "refreshToken": "abc..."
}
```

### Logout
```http
POST /api/auth/logout
Authorization: Bearer {token}

{
  "refreshToken": "abc..."
}
```

### Forgot Password
```http
POST /api/auth/forgot-password

{
  "email": "ahmed@mentora.com"
}
```

### Reset Password
```http
POST /api/auth/reset-password

{
  "email": "ahmed@mentora.com",
  "token": "reset-token",
  "newPassword": "NewPass@123",
  "confirmPassword": "NewPass@123"
}
```

---

## ?? User Profile Endpoints

### Get Profile
```http
GET /api/userprofile
Authorization: Bearer {token}

Response:
{
  "id": "guid",
  "bio": "Student bio",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman",
  "completionPercentage": 85
}
```

### Update Profile
```http
PUT /api/userprofile
Authorization: Bearer {token}

{
  "bio": "CS Student",
  "location": "Amman, Jordan",
  "phoneNumber": "+962791234567",
  "university": "University of Jordan",
  "major": "Computer Science",
  "expectedGraduationYear": 2025,
  "currentLevel": "Junior",
  "timezone": "Asia/Amman",
  "linkedInUrl": "https://linkedin.com/in/user",
  "gitHubUrl": "https://github.com/user"
}
```

### Check Profile Exists
```http
GET /api/userprofile/exists
Authorization: Bearer {token}

Response:
{
  "exists": true
}
```

### Get Profile Completion
```http
GET /api/userprofile/completion
Authorization: Bearer {token}

Response:
{
  "completionPercentage": 85
}
```

### Get Timezones
```http
GET /api/userprofile/timezones?location=Jordan

Response:
["Asia/Amman", "Asia/Jerusalem", ...]
```

### Validate Timezone
```http
GET /api/userprofile/validate-timezone?timezone=Asia/Amman

Response:
{
  "isValid": true,
  "timezone": "Asia/Amman"
}
```

---

## ?? Request/Response Format

### Required Headers
```
Content-Type: application/json
Authorization: Bearer {token}  (for protected endpoints)
```

### Status Codes
| Code | Meaning |
|------|---------|
| 200 | Success |
| 400 | Bad Request |
| 401 | Unauthorized |
| 404 | Not Found |
| 409 | Conflict (e.g., email exists) |
| 500 | Server Error |

---

## ?? Authentication Flow

```
1. Register    ? POST /api/auth/register
2. Login       ? POST /api/auth/login
3. Get Token   ? Save accessToken
4. Use API     ? Add Authorization header
5. Refresh     ? POST /api/auth/refresh-token (when expired)
```

---

## ?? Password Requirements

- ? Minimum 8 characters
- ? At least 1 uppercase letter
- ? At least 1 special character

**Valid**: `Test@123`, `Pass!word1`  
**Invalid**: `test123`, `Test123`

---

## ?? Test Users

| Email | Password | Profile |
|-------|----------|---------|
| saad@mentora.com | Saad@123 | Senior, CS |
| maria@mentora.com | Maria@123 | Graduate, Data Science |

---

## ??? Quick Testing

### Using Swagger
1. Open `https://localhost:7000/swagger`
2. Click endpoint (e.g., POST /api/auth/login)
3. Click "Try it out"
4. Enter request body
5. Click "Execute"
6. Copy access token
7. Click "Authorize" button
8. Paste token
9. Test protected endpoints

### Using Postman
1. Import collection
2. POST `/api/auth/login`
3. Copy `accessToken` from response
4. Go to "Authorization" tab
5. Select "Bearer Token"
6. Paste token
7. Test other endpoints

---

## ?? More Details

- [MODULE-1-AUTHENTICATION.md](./MODULE-1-AUTHENTICATION.md) - Authentication details
- [MODULE-2-USER-PROFILE.md](./MODULE-2-USER-PROFILE.md) - Profile management details
- [QUICK-START.md](./QUICK-START.md) - Setup guide

---

**Last Updated**: 2024-12-31
