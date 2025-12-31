# Swagger UI Guide - Complete Swagger Documentation

## Introduction

Swagger UI is a powerful interactive tool for testing and documenting the Mentora API.

? **Important Note**: Swagger is enabled in Development AND Production environments!

---

## Accessing Swagger

### 1. Run the Application
```bash
cd Server/src/Mentora.Api
dotnet run
```

### 2. Open Browser
Navigate to one of these URLs:

**Direct to Swagger** (recommended):
```
https://localhost:7xxx/
```

OR

**Explicit Swagger path**:
```
https://localhost:7xxx/swagger
```

? **Note**: Swagger opens automatically when you navigate to the root URL `/`

---

## Testing Authentication Flow

### Step 1: Register a New User
1. Find endpoint: **POST /api/auth/register**
2. Click "Try it out"
3. Enter request body:
```json
{
  "firstName": "Ahmed",
  "lastName": "Ali",
  "email": "ahmed@example.com",
  "password": "Test@123"
}
```
4. Click "Execute"
5. Verify you receive a 200 OK response

### Step 2: Login
1. Find endpoint: **POST /api/auth/login**
2. Click "Try it out"
3. Enter credentials:
```json
{
  "email": "ahmed@example.com",
  "password": "Test@123",
  "deviceInfo": "Chrome/Windows"
}
```
4. Click "Execute"
5. Copy the `accessToken` from the response

### Step 3: Authorize with Token
1. Click the **"Authorize"** button at the top right (green lock icon)
2. In the authorization modal, enter:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```
   (Make sure to include the word `Bearer` followed by a space before the token)
3. Click "Authorize"
4. Click "Close"

? Now you can test all protected endpoints!

---

## API Endpoints Overview

### Public Endpoints (No Authentication Required)

#### 1. POST /api/auth/register
Register a new user
- **Validation Rules**:
  - Password must be at least 8 characters
  - Must contain uppercase letter
  - Must contain lowercase letter

#### 2. POST /api/auth/login
Login and receive tokens
- **Returns**:
  - `accessToken`: Valid for 60 minutes
  - `refreshToken`: Valid for 7 days

#### 3. POST /api/auth/refresh-token
Refresh access token
- Use this when access token expires
- Requires valid refresh token

#### 4. POST /api/auth/forgot-password
Request password reset
- Generates a token for password reset

#### 5. POST /api/auth/reset-password
Reset password using token

---

### Protected Endpoints (Authentication Required)

#### 1. POST /api/auth/logout
Logout from current device
- Revokes current refresh token

#### 2. POST /api/auth/logout-all
Logout from all devices
- Revokes all refresh tokens for user

#### 3. GET /api/auth/me
Get current user information
- Returns user details from JWT token

---

## Swagger UI Features

### ? Available Features
- **Try it out**: Test endpoints directly in browser
- **Request samples**: View example request bodies
- **Response samples**: See expected responses
- **Model schemas**: View detailed structure of DTOs
- **Request duration**: See how long requests take
- **Deep linking**: Share links to specific endpoints
- **Filter**: Search through endpoints
- **Always Available**: Works in both Development and Production ?

### Filters & Search
Use the "Filter by tags" input at the top to search for specific endpoints.

### Models Section
Scroll down to see all DTOs and Models used in the API.

---

## Common Testing Workflows

### Workflow 1: Complete Authentication Flow
```
1. POST /api/auth/register  ? Register new user
2. POST /api/auth/login     ? Get tokens
3. Authorize                ? Set bearer token
4. GET  /api/auth/me        ? View current user
5. POST /api/auth/logout    ? Logout
```

### Workflow 2: Password Reset Flow
```
1. POST /api/auth/forgot-password  ? Request reset token
2. (Check database for token)
3. POST /api/auth/reset-password   ? Reset password
4. POST /api/auth/login            ? Login with new password
```

### Workflow 3: Token Refresh Flow
```
1. POST /api/auth/login           ? Get tokens
2. (Wait 60+ minutes)
3. POST /api/auth/refresh-token   ? Get new access token
```

---

## Best Practices

### ? Do's
- Use "Try it out" to test all endpoints
- Read the "Description" for each endpoint
- Check "Response samples" to understand expected responses
- Use the "Authorize" button for protected endpoints
- Use Swagger in both Development and Production - it's always available! ?

### ? Don'ts
- Don't forget "Bearer" prefix when entering token
- Don't use expired access tokens
- Don't share tokens publicly
- Don't store tokens in browser console

---

## Troubleshooting

### Issue: "401 Unauthorized"
**Solution**:
1. Verify you're logged in
2. Check you've authorized using "Authorize" button
3. Ensure "Bearer " prefix in token
4. Verify token hasn't expired (60 minutes)

### Issue: "400 Bad Request"
**Solution**:
1. Check JSON syntax
2. Verify all required fields are provided
3. Ensure password meets requirements

### Issue: "Swagger not loading"
**Solution**:
1. Verify application is running
2. Navigate to: `https://localhost:7xxx/`
3. Try hard refresh (F5)
4. Check Console logs in browser

? **Remember**: Swagger works in both Development and Production!

---

## Response Status Codes

### Standard Status Codes
- **200 OK**: Request successful
- **400 Bad Request**: Invalid request data
- **401 Unauthorized**: Authentication required or token invalid
- **409 Conflict**: Resource already exists
- **500 Internal Server Error**: Server error

### Response Structure
All responses follow this structure:
```json
{
  "isSuccess": true/false,
  "message": "Response message",
  "accessToken": "...",      // Only for login/refresh
  "refreshToken": "...",     // Only for login/refresh
  "errors": ["..."]          // Only on error
}
```

---

## Swagger Customization

The Mentora Swagger UI uses custom styling:
- **Primary Color**: #6B9080 (Green)
- **Secondary Color**: #A4C3B2 (Light Green)

Custom styles are located at:
`Server/src/Mentora.Api/wwwroot/swagger-ui/custom.css`

---

## Swagger in Different Environments

### Development
```
? Swagger fully enabled
? Interactive testing
? Try it out functionality
```

### Production
```
? Swagger fully enabled
? API Documentation
? Interactive testing available
```

### To Disable in Production
If you need to disable Swagger in Production, edit `Program.cs`:
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
```

---

## Getting Help

If you encounter issues:
1. Check Console logs in browser
2. Review Response body for error details
3. Verify database migrations are applied
4. See `docs/QUICK-START.md`
5. Ensure you're using correct URL: `https://localhost:7xxx`

---

## Quick Access

To quickly access Swagger:
1. Run application: `dotnet run`
2. Navigate to: `https://localhost:7xxx/`
3. Start testing! ??

---

**Last Updated**: December 31, 2024  
**Version**: 1.0.0
