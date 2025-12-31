# Module 1: Identity, Authentication & Security

## ?? Overview
Complete implementation of **SRS Module 1** covering secure user registration, authentication, session management, and password reset functionality.

## ? Implemented Requirements

### 1.1 User Registration (Onboarding)
- ? **1.1.1 Credential Validation**: Regex-based validation for email format and password complexity
  - Email: Standard email format validation
  - Password: Minimum 8 characters, 1 uppercase letter, 1 special character
  - Location: `Mentora.Application.Validators.PasswordValidator`
  
- ? **1.1.2 Uniqueness Check**: Email uniqueness validation returning 409 Conflict
  - Location: `AuthService.RegisterAsync()`
  - Returns HTTP 409 if email exists
  
- ? **1.1.3 Cryptographic Hashing**: BCrypt password hashing via ASP.NET Identity
  - No plaintext passwords stored
  - Configured in `Program.cs` Identity setup

### 1.2 Authentication & Session Management
- ? **1.2.1 Token Issuance**: JWT Access Token generation
  - Claims: UserId, Email, FirstName, LastName
  - Expiration: 60 minutes
  - Location: `TokenService.GenerateAccessToken()`
  
- ? **1.2.2 Persistent Sessions**: Refresh Token implementation
  - Cryptographically secure random string (64 bytes)
  - 30-day validity period
  - Device tracking (DeviceInfo, IpAddress)
  - Token rotation on refresh
  - Location: `TokenService.GenerateRefreshToken()`
  
- ? **1.2.3 Token Revocation Strategy**: Multi-device logout support
  - Single device logout: `AuthService.LogoutAsync()`
  - All devices logout: `AuthService.LogoutAllDevicesAsync()`
  - Revokes all active RefreshTokens for user

### Password Reset Flow
- ? **Forgot Password**: Secure token generation
  - 64-byte cryptographically secure token
  - 1-hour validity
  - Stored in PasswordResetToken entity
  - Location: `AuthService.ForgotPasswordAsync()`
  
- ? **Reset Password**: Token validation and password update
  - Token expiry validation
  - Password complexity validation
  - Auto-revokes all refresh tokens after reset
  - Location: `AuthService.ResetPasswordAsync()`

### Background Services
- ? **Token Cleanup Service**: Daily cleanup of expired tokens
  - Removes expired password reset tokens (7+ days old)
  - Removes expired refresh tokens (7+ days old)
  - Location: `TokenCleanupService`

## ??? Architecture

### Domain Layer (`Mentora.Domain`)
```
Entities/
??? Auth/
?   ??? User.cs                    # User entity with GUID primary key
?   ??? PasswordResetToken.cs     # Password reset token storage
??? RefreshToken.cs                # Refresh token with device tracking
??? Common/
    ??? BaseEntity.cs              # Base entity with soft delete support
```

### Application Layer (`Mentora.Application`)
```
DTOs/
??? SignupDTO.cs                   # Registration with Regex validation
??? LoginDTO.cs                    # Login credentials with DeviceInfo
??? RefreshTokenDto.cs             # Refresh token request
??? ForgotPasswordDto.cs           # Password reset request
??? ResetPasswordDto.cs            # Password reset with token
??? AuthResponseDto.cs             # Standard auth response

Interfaces/
??? IAuthService.cs                # Authentication service contract
??? ITokenService.cs               # Token generation & validation
??? IUserRepository.cs             # User data access contract

Validators/
??? PasswordValidator.cs           # Regex-based password validation
```

### Infrastructure Layer (`Mentora.Infrastructure`)
```
Services/
??? AuthService.cs                 # Complete auth implementation
??? TokenService.cs                # JWT & RefreshToken generation

Repositories/
??? UserRepository.cs              # User data access with uniqueness checks

BackgroundServices/
??? TokenCleanupService.cs         # Daily token cleanup

Persistence/
??? ApplicationDbContext.cs        # EF Core DbContext with Identity
```

### API Layer (`Mentora.Api`)
```
Controllers/
??? AuthController.cs              # All authentication endpoints

Configuration/
??? Program.cs                     # Identity, JWT, DI setup
```

## ?? API Endpoints

### Authentication Endpoints

#### 1. Register
```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "Ahmed",
  "lastName": "Mohammed",
  "email": "ahmed@example.com",
  "password": "Test@1234",
  "confirmPassword": "Test@1234"
}
```

**Response Codes:**
- `200 OK`: Registration successful
- `400 Bad Request`: Validation failed
- `409 Conflict`: Email already exists

#### 2. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "ahmed@example.com",
  "password": "Test@1234",
  "deviceInfo": "Chrome/Windows 11"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "message": "Login successful",
  "accessToken": "eyJhbGc...",
  "refreshToken": "abc123...",
  "tokenExpiration": "2024-01-20T15:30:00Z"
}
```

#### 3. Refresh Token
```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "abc123..."
}
```

#### 4. Get Current User
```http
GET /api/auth/me
Authorization: Bearer {accessToken}
```

#### 5. Logout
```http
POST /api/auth/logout
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "refreshToken": "abc123..."
}
```

#### 6. Logout All Devices
```http
POST /api/auth/logout-all
Authorization: Bearer {accessToken}
```

#### 7. Forgot Password
```http
POST /api/auth/forgot-password
Content-Type: application/json

{
  "email": "ahmed@example.com"
}
```

#### 8. Reset Password
```http
POST /api/auth/reset-password
Content-Type: application/json

{
  "email": "ahmed@example.com",
  "token": "reset_token_from_email",
  "newPassword": "NewTest@1234",
  "confirmPassword": "NewTest@1234"
}
```

## ?? Security Features

### Password Security (SRS 1.1.1)
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 special character
- BCrypt hashing (handled by ASP.NET Identity)

### Token Security (SRS 1.2.1 & 1.2.2)
- **Access Token**: JWT with 60-minute expiration
- **Refresh Token**: 
  - Cryptographically secure (64 bytes)
  - 30-day validity
  - Device tracking for security
  - Token rotation on refresh

### Lockout Protection
- Max 5 failed login attempts
- 15-minute lockout duration

## ??? Database Schema

### Users (Identity Framework)
- `Id` (GUID): Primary key per SRS 8.1
- `Email` (string): Unique, required
- `FirstName` (string)
- `LastName` (string)
- `CreatedAt` (DateTime)
- `LastLoginAt` (DateTime?)
- `IsActive` (bool)

### RefreshTokens
- `Id` (GUID): Primary key
- `UserId` (GUID): Foreign key to Users
- `Token` (string): Cryptographically secure token
- `ExpiresOn` (DateTime)
- `CreatedOn` (DateTime)
- `RevokedOn` (DateTime?)
- `DeviceInfo` (string?)
- `IpAddress` (string?)

### PasswordResetTokens
- `Id` (GUID): Primary key
- `UserId` (GUID): Foreign key to Users
- `Token` (string): Reset token
- `ExpiresAt` (DateTime)
- `Used` (bool)

## ?? Testing

Test file location: `Server/src/Mentora.Api/Tests/auth-tests.http`

### Test Scenarios Covered:
1. ? Successful registration
2. ? Duplicate email registration (409 Conflict)
3. ? Invalid password format (validation failure)
4. ? Invalid email format (validation failure)
5. ? Successful login with tokens
6. ? Get authenticated user info
7. ? Token refresh flow
8. ? Single device logout
9. ? All devices logout
10. ? Forgot password flow
11. ? Password reset with token

## ?? Running the Application

### Prerequisites
- .NET 9 SDK
- SQL Server (or update connection string)

### Steps
1. Update connection string in `appsettings.json`
2. Run migrations:
```bash
cd Server/src/Mentora.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Mentora.Api
dotnet ef database update --startup-project ../Mentora.Api
```

3. Run the application:
```bash
cd Server/src/Mentora.Api
dotnet run
```

4. Access Swagger UI: `https://localhost:7001/swagger`

## ?? Configuration

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
- Password requirements configured per SRS 1.1.1
- Unique email enforcement
- Lockout settings (5 attempts, 15 minutes)

## ? Clean Architecture Compliance

? **Domain Layer**: Pure entities, no dependencies  
? **Application Layer**: Business logic, DTOs, interfaces  
? **Infrastructure Layer**: Data access, external services  
? **API Layer**: Controllers, middleware, configuration  

? **SOLID Principles**: Applied throughout  
? **Dependency Injection**: All services properly registered  
? **Separation of Concerns**: Clear layer boundaries  

## ?? SRS Compliance Matrix

| Requirement | Status | Location |
|------------|--------|----------|
| 1.1.1 Credential Validation | ? | `PasswordValidator.cs`, DTOs |
| 1.1.2 Uniqueness Check | ? | `AuthService.RegisterAsync()` |
| 1.1.3 BCrypt Hashing | ? | Identity Framework |
| 1.2.1 Token Issuance | ? | `TokenService.GenerateAccessToken()` |
| 1.2.2 Refresh Tokens | ? | `TokenService.GenerateRefreshToken()` |
| 1.2.3 Token Revocation | ? | `TokenService.RevokeAllUserTokensAsync()` |
| 8.1 GUID Primary Keys | ? | All entities |
| 8.2 Soft Deletes | ? | `BaseEntity`, `ApplicationDbContext` |

## ?? Next Steps

Module 1 is **COMPLETE** and ready for production use. 

Next module to implement: **Module 2: User Profile & Personalization**
