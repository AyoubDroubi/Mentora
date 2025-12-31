# Quick Start Guide - Mentora Backend

## Prerequisites

### Required Software
- ? .NET 9 SDK
- ? SQL Server (Local or Express)
- ? dotnet-ef tools version 9.0.0

---

## Setup Instructions

### 1. Verify Existing Migrations
```bash
# Navigate to Infrastructure project
cd D:\99_GitHub\001_Mentora\00_MentoraNew\Server\src\Mentora.Infrastructure

# List all migrations
dotnet ef migrations list --startup-project ..\Mentora.Api

# Expected output:
? 20251231070541_InitialMigration
```

### 2. Apply Migration (if not already applied)
```bash
dotnet ef database update --startup-project ..\Mentora.Api
```

### 3. Run the Application
```bash
cd ..\Mentora.Api
dotnet run
```

### 4. Access Swagger UI
```
https://localhost:7XXX/swagger
```

---

## Testing the API

### 1. Register a New User
**POST** `/api/auth/signup`

```json
{
  "firstName": "Ahmed",
  "lastName": "Ali",
  "email": "ahmed@example.com",
  "password": "Test@123"
}
```

**Expected Response**:
```json
{
  "isSuccess": true,
  "message": "User registered successfully"
}
```

### 2. Login
**POST** `/api/auth/login`

```json
{
  "email": "ahmed@example.com",
  "password": "Test@123",
  "deviceInfo": "Chrome/Windows"
}
```

**Expected Response**:
```json
{
  "isSuccess": true,
  "message": "Login successful",
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "AbC123..."
}
```

### 3. Refresh Token
**POST** `/api/auth/refresh`

```json
{
  "refreshToken": "AbC123..."
}
```

---

## Verifying Database

### Using SQL Server Management Studio
```sql
USE MentoraDb;

-- List all tables
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- View users
SELECT Id, Email, FirstName, LastName, IsActive, CreatedAt
FROM AspNetUsers;

-- View refresh tokens
SELECT UserId, Token, ExpiresOn, IsRevoked, DeviceInfo
FROM RefreshTokens;
```

---

## Troubleshooting Common Issues

### Issue: "Could not connect to database"
**Solution**:
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
   ```json
   "DefaultConnection": "Server=.;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;..."
   ```

### Issue: "Migration not found"
**Solution**:
```bash
# Create a new migration
dotnet ef migrations add InitialMigration --startup-project ..\Mentora.Api

# Apply the migration
dotnet ef database update --startup-project ..\Mentora.Api
```

### Issue: "TokenCleanupService not starting"
**Solution**:
Check the logs for:
```
Token Cleanup Service started ?
```

If missing, verify in `Program.cs`:
```csharp
builder.Services.AddHostedService<TokenCleanupService>();
```

---

## Project Structure Overview

```
Server/
??? src/
?   ??? Mentora.Api/              # Web API Layer
?   ?   ??? Controllers/
?   ?   ??? Program.cs
?   ?   ??? appsettings.json
?   ?
?   ??? Mentora.Application/      # Business Logic
?   ?   ??? DTOs/
?   ?   ??? Interfaces/
?   ?   ??? Validators/
?   ?
?   ??? Mentora.Domain/           # Domain Entities
?   ?   ??? Entities/
?   ?   ?   ??? Auth/
?   ?   ?   ??? ...
?   ?   ??? Common/
?   ?
?   ??? Mentora.Infrastructure/   # Data Access
?       ??? Persistence/
?       ?   ??? ApplicationDbContext.cs
?       ??? Repositories/
?       ??? Services/
?       ??? BackgroundServices/
?           ??? TokenCleanupService.cs
?
??? docs/
?   ??? MODULE-1-AUTHENTICATION.md
?   ??? DATABASE-SETUP-SUMMARY.md
?
??? scripts/
    ??? health-check.ps1
    ??? health-check.sh
```

---

## Implemented Features

### Authentication Module (Module 1)
- ? User Registration with BCrypt password hashing
- ? JWT Access Tokens (60 minutes expiration)
- ? Refresh Tokens with Device Tracking (7 days expiration)
- ? Password Reset Flow
- ? Logout & Logout All Devices
- ? Token Cleanup Background Service

### Database Features
- ? GUID Primary Keys
- ? Soft Delete
- ? Auto Timestamps (CreatedAt, UpdatedAt)
- ? Enum to String Conversion
- ? Entity Relationships

### Background Services
- ? TokenCleanupService (runs every 24 hours)

---

## Getting Help

If you encounter issues:
1. Review the `DATABASE-SETUP-SUMMARY.md` file
2. Check Console logs for errors
3. Review error messages in Swagger
4. Check for build errors in Visual Studio

---

## Next Steps

After completing the Authentication Module:

1. **Module 2**: Career Builder
2. **Module 3**: Study Scheduler
3. **Module 4**: AI Integration
4. **Module 5**: Frontend Integration

---

## Additional Resources

- [SWAGGER-GUIDE.md](./SWAGGER-GUIDE.md) - Comprehensive Swagger usage guide
- [API-QUICK-REFERENCE.md](./API-QUICK-REFERENCE.md) - Quick API reference
- [MODULE-1-AUTHENTICATION.md](./MODULE-1-AUTHENTICATION.md) - Detailed authentication documentation

---

**Last Updated**: December 31, 2024  
**Status**: ? Fully Operational
