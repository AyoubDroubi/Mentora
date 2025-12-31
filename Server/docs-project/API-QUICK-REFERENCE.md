# API Quick Reference - ???? ???? ??? API

## ?? Authentication Endpoints

### Base URL
```
https://localhost:7xxx/api/auth
```

---

## ?? Endpoints Overview

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/register` | ? | Register new user |
| POST | `/login` | ? | Login user |
| POST | `/refresh-token` | ? | Refresh access token |
| POST | `/logout` | ? | Logout from current device |
| POST | `/logout-all` | ? | Logout from all devices |
| POST | `/forgot-password` | ? | Request password reset |
| POST | `/reset-password` | ? | Reset password |
| GET | `/me` | ? | Get current user info |

---

## ?? Request Examples

### 1. Register
```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "????",
  "lastName": "????",
  "email": "ahmed@example.com",
  "password": "Test@123"
}
```

**Response (200 OK)**:
```json
{
  "isSuccess": true,
  "message": "User registered successfully"
}
```

---

### 2. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "ahmed@example.com",
  "password": "Test@123",
  "deviceInfo": "Chrome/Windows"
}
```

**Response (200 OK)**:
```json
{
  "isSuccess": true,
  "message": "Login successful",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "AbC123XyZ789...",
  "tokenExpiration": "2024-12-31T11:30:00Z"
}
```

---

### 3. Refresh Token
```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "AbC123XyZ789..."
}
```

**Response (200 OK)**:
```json
{
  "isSuccess": true,
  "message": "Token refreshed successfully",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "NewRefreshToken123...",
  "tokenExpiration": "2024-12-31T12:30:00Z"
}
```

---

### 4. Logout
```http
POST /api/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "refreshToken": "AbC123XyZ789..."
}
```

**Response (200 OK)**:
```json
{
  "message": "Logged out successfully"
}
```

---

### 5. Logout All
```http
POST /api/auth/logout-all
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK)**:
```json
{
  "message": "Logged out from all devices successfully"
}
```

---

### 6. Forgot Password
```http
POST /api/auth/forgot-password
Content-Type: application/json

{
  "email": "ahmed@example.com"
}
```

**Response (200 OK)**:
```json
{
  "message": "If the email exists, a password reset link has been sent"
}
```

---

### 7. Reset Password
```http
POST /api/auth/reset-password
Content-Type: application/json

{
  "email": "ahmed@example.com",
  "token": "reset-token-from-email",
  "newPassword": "NewPass@123",
  "confirmPassword": "NewPass@123"
}
```

**Response (200 OK)**:
```json
{
  "isSuccess": true,
  "message": "Password reset successfully"
}
```

---

### 8. Get Current User
```http
GET /api/auth/me
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK)**:
```json
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "email": "ahmed@example.com",
  "firstName": "????",
  "lastName": "????"
}
```

---

## ?? JWT Token Format

### Access Token Claims
```json
{
  "userId": "guid",
  "email": "user@example.com",
  "firstName": "First",
  "lastName": "Last",
  "exp": 1704108000,
  "iss": "MentoraApi",
  "aud": "MentoraClient"
}
```

### Token Expiry
- **Access Token**: 60 minutes
- **Refresh Token**: 7 days

---

## ?? Status Codes

| Code | Description |
|------|-------------|
| 200 | Success |
| 400 | Bad Request - Invalid input |
| 401 | Unauthorized - Invalid credentials or token |
| 409 | Conflict - User already exists |
| 500 | Internal Server Error |

---

## ?? Password Requirements

- ? Minimum 8 characters
- ? At least 1 uppercase letter
- ? At least 1 special character (!@#$%^&*...)

**Valid Examples**:
- `Test@123`
- `MyPass!2024`
- `SecureP@ss`

**Invalid Examples**:
- `test123` (no uppercase, no special char)
- `Test123` (no special char)
- `Test@` (too short)

---

## ?? Using Bearer Token

### In Headers
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### In Swagger UI
1. Click "Authorize" button (??)
2. Enter: `Bearer {your-token}`
3. Click "Authorize"
4. Click "Close"

### In Postman
1. Go to "Authorization" tab
2. Select "Bearer Token" type
3. Paste your token
4. Send request

---

## ?? Testing Flow

### Complete Authentication Flow
```
1. POST /register          ? Create account
2. POST /login             ? Get tokens
3. GET  /me               ? Verify token works
4. POST /logout           ? Logout current device
```

### Password Reset Flow
```
1. POST /forgot-password   ? Request reset
2. (Check database/email for token)
3. POST /reset-password    ? Reset with token
4. POST /login             ? Login with new password
```

### Token Refresh Flow
```
1. POST /login             ? Get initial tokens
2. (Use access token for 60 min)
3. POST /refresh-token     ? Get new access token
4. (Continue using new token)
```

---

## ?? Device Management

### Track Active Devices
```sql
SELECT UserId, DeviceInfo, IpAddress, CreatedOn, ExpiresOn
FROM RefreshTokens
WHERE RevokedOn IS NULL
ORDER BY CreatedOn DESC;
```

### Logout Specific Device
Use `/logout` with specific refresh token

### Logout All Devices
Use `/logout-all` to revoke all tokens

---

## ??? cURL Examples

### Register
```bash
curl -X POST "https://localhost:7xxx/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "????",
    "lastName": "????",
    "email": "ahmed@example.com",
    "password": "Test@123"
  }'
```

### Login
```bash
curl -X POST "https://localhost:7xxx/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ahmed@example.com",
    "password": "Test@123",
    "deviceInfo": "cURL/Linux"
  }'
```

### Get Current User
```bash
curl -X GET "https://localhost:7xxx/api/auth/me" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

---

## ?? Tips

### ? Best Practices
- Store tokens securely (HttpOnly cookies recommended)
- Always use HTTPS in production
- Implement token refresh before expiry
- Use device info for tracking
- Handle 401 errors gracefully

### ? Avoid
- Storing tokens in localStorage (XSS risk)
- Sharing tokens between users
- Using expired tokens
- Hardcoding tokens in code
- Exposing tokens in logs

---

## ?? Quick Help

**Issue**: "401 Unauthorized"
- Check token is valid and not expired
- Ensure "Bearer " prefix is included
- Verify token was obtained from `/login`

**Issue**: "400 Bad Request"
- Check JSON format
- Verify all required fields are present
- Check password requirements

**Issue**: "409 Conflict"
- User with this email already exists
- Try logging in instead

---

**Last Updated**: December 31, 2024
**API Version**: v1.0
