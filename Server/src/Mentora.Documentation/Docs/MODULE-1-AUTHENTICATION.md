# Module 1: Authentication & Security

## Overview
Complete implementation of user authentication system including registration, login, session management, and password reset functionality.

---

## Features Implemented

### User Registration
- **Email & Password Validation**
  - Email format validation
  - Password: Minimum 8 characters, 1 uppercase, 1 special character
  - Email uniqueness check (returns 409 if exists)

- **Password Security**
  - BCrypt hashing via ASP.NET Identity
  - No plaintext passwords stored

### Authentication & Sessions
- **JWT Access Tokens**
  - Claims: UserId, Email, FirstName, LastName
  - Expiration: 60 minutes
  - Used for API authentication

- **Refresh Tokens**
  - Cryptographically secure (64 bytes)
  - 30-day validity
  - Device tracking (DeviceInfo, IpAddress)
  - Token rotation on refresh

- **Multi-Device Support**
  - Single device logout
  - Logout from all devices
  - Session tracking per device

### Password Reset
- **Forgot Password**
  - Secure token generation (64 bytes)
  - 1-hour validity
  - Email delivery (configured)

- **Reset Password**
  - Token validation
  - Password complexity check
  - Auto-revokes all sessions

### Background Tasks
- **Token Cleanup Service**
  - Runs daily
  - Removes expired tokens (7+ days old)

---

## Architecture

### Domain Layer
```
Entities/Auth/
??? User.cs                    # User entity (GUID primary key)
??? PasswordResetToken.cs      # Password reset tokens
??? RefreshToken.cs            # Refresh tokens with device tracking
```

### Application Layer
```
DTOs/
??? SignupDTO.cs               # Registration request
??? LoginDTO.cs                # Login credentials
??? RefreshTokenDto.cs         # Token refresh
??? ForgotPasswordDto.cs       # Password reset request
??? ResetPasswordDto.cs        # Password reset
??? AuthResponseDto.cs         # Auth response

Interfaces/
??? IAuthService.cs            # Authentication service
??? ITokenService.cs           # Token management
??? IUserRepository.cs         # User data access

Validators/
??? PasswordValidator.cs       # Password validation
```

### Infrastructure Layer
```
Services/
??? AuthService.cs             # Authentication logic
??? TokenService.cs            # JWT & refresh tokens

Repositories/
??? UserRepository.cs          # User data access

BackgroundServices/
??? TokenCleanupService.cs     # Token cleanup

Persistence/
??? ApplicationDbContext.cs    # EF Core context
```

### API Layer
```
Controllers/
??? AuthController.cs          # Authentication endpoints

Configuration/
??? Program.cs                 # DI & JWT setup
```

---

## API Endpoints

### 1. Register
```http
POST /api/auth/register

{
  "firstName": "Ahmed",
  "lastName": "Mohammed",
  "email": "ahmed@mentora.com",
  "password": "Test@1234",
  "confirmPassword": "Test@1234"
}
```

**Responses:**
- `200 OK` - Registration successful
- `400 Bad Request` - Validation failed
- `409 Conflict` - Email already exists

---

### 2. Login
```http
POST /api/auth/login

{
  "email": "ahmed@mentora.com",
  "password": "Test@1234",
  "deviceInfo": "Chrome/Windows 11"
}
```

**Response:**
```json
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "abc123...",
  "tokenExpiration": "2024-01-20T15:30:00Z"
}
```

---

### 3. Refresh Token
```http
POST /api/auth/refresh-token

{
  "refreshToken": "abc123..."
}
```

---

### 4. Get Current User
```http
GET /api/auth/me
Authorization: Bearer {accessToken}
```

---

### 5. Logout
```http
POST /api/auth/logout
Authorization: Bearer {accessToken}

{
  "refreshToken": "abc123..."
}
```

---

### 6. Logout All Devices
```http
POST /api/auth/logout-all
Authorization: Bearer {accessToken}
```

---

### 7. Forgot Password
```http
POST /api/auth/forgot-password

{
  "email": "ahmed@mentora.com"
}
```

---

### 8. Reset Password
```http
POST /api/auth/reset-password

{
  "email": "ahmed@mentora.com",
  "token": "reset_token_from_email",
  "newPassword": "NewTest@1234",
  "confirmPassword": "NewTest@1234"
}
```

---

## Security Features

### Password Requirements
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 special character
- BCrypt hashing

### Token Security
- **Access Token**: JWT, 60 minutes
- **Refresh Token**: Secure random, 30 days, device tracking

### Account Protection
- Max 5 failed login attempts
- 15-minute lockout

---

## Database Schema

### Users (ASP.NET Identity)
```
Id (GUID)
Email (string, unique)
FirstName (string)
LastName (string)
CreatedAt (DateTime)
LastLoginAt (DateTime?)
IsActive (bool)
```

### RefreshTokens
```
Id (GUID)
UserId (GUID, FK)
Token (string)
ExpiresOn (DateTime)
CreatedOn (DateTime)
RevokedOn (DateTime?)
DeviceInfo (string?)
IpAddress (string?)
```

### PasswordResetTokens
```
Id (GUID)
UserId (GUID, FK)
Token (string)
ExpiresAt (DateTime)
Used (bool)
```

---

## Testing

**Test File**: `Server/src/Mentora.Api/Tests/auth-tests.http`

### Test Scenarios
1. Successful registration
2. Duplicate email (409 Conflict)
3. Invalid password format
4. Invalid email format
5. Successful login
6. Get authenticated user
7. Token refresh
8. Single device logout
9. All devices logout
10. Forgot password
11. Password reset

---

## Configuration

### JWT Settings (appsettings.json)
```json
{
  "Jwt": {
    "Key": "your-secret-key-at-least-32-characters-long",
    "Issuer": "MentoraApi",
    "Audience": "MentoraClient"
  }
}
```

### Identity Settings (Program.cs)
- Password requirements
- Email uniqueness
- Lockout: 5 attempts, 15 minutes

---

## Quick Start

### 1. Run Backend
```bash
cd Server/src/Mentora.Api
dotnet run
```

### 2. Access Swagger
`https://localhost:7000/swagger`

### 3. Test Authentication
- Register user via Swagger
- Login to get tokens
- Use access token for protected endpoints

---

## Status

**Module 1**: Complete

**Next Module**: Module 2 - User Profile & Personalization

---

**Last Updated**: 2024-12-31
