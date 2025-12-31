# Mentora Backend API

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Swagger](https://img.shields.io/badge/Swagger-Auto--Enabled-success)](https://localhost:7xxx/)

AI-powered academic excellence and career success platform.

---

## ?? Quick Start

### Prerequisites
- .NET 9 SDK
- SQL Server (Local or Express)
- Visual Studio 2022 or VS Code

### Run the Project
```bash
cd Server/src/Mentora.Api
dotnet run
```

### Access Swagger UI ?
Open your browser and navigate to:
```
https://localhost:7xxx/
```

**? NEW**: Swagger UI is now **auto-enabled** and opens directly at the root URL!
- No need to add `/swagger`
- Works in all environments (Development & Production)
- Ready to test immediately

---

## ?? Project Structure

```
Server/
??? src/
?   ??? Mentora.Api/              # Web API Layer
?   ??? Mentora.Application/      # Business Logic Layer
?   ??? Mentora.Domain/           # Domain Entities
?   ??? Mentora.Infrastructure/   # Data Access Layer
??? docs/                         # Documentation
?   ??? DATABASE-SETUP-SUMMARY.md
?   ??? QUICK-START-AR.md
?   ??? CHANGES-SUMMARY.md
?   ??? MODULE-1-AUTHENTICATION.md
?   ??? SWAGGER-GUIDE-AR.md
?   ??? SWAGGER-AUTO-ENABLE.md    ? NEW
?   ??? API-QUICK-REFERENCE.md
??? scripts/                      # Utility Scripts
    ??? health-check.ps1
    ??? health-check.sh
```

---

## ?? Authentication Module (Module 1)

### Available Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | ? |
| POST | `/api/auth/login` | Login user | ? |
| POST | `/api/auth/refresh-token` | Refresh access token | ? |
| POST | `/api/auth/logout` | Logout from current device | ? |
| POST | `/api/auth/logout-all` | Logout from all devices | ? |
| POST | `/api/auth/forgot-password` | Request password reset | ? |
| POST | `/api/auth/reset-password` | Reset password with token | ? |
| GET | `/api/auth/me` | Get current user info | ? |

---

## ?? Testing

### Using Swagger UI ? (Recommended)
1. Run the project
2. Open browser at `https://localhost:7xxx/`
3. **Swagger opens automatically!**
4. Try endpoints directly from the UI

### Using HTTP Files
See `Server/src/Mentora.Api/Tests/auth-tests.http` for sample requests.

### Example: Register User
```http
POST https://localhost:7xxx/api/auth/register
Content-Type: application/json

{
  "firstName": "????",
  "lastName": "????",
  "email": "ahmed@example.com",
  "password": "Test@123"
}
```

### Example: Login
```http
POST https://localhost:7xxx/api/auth/login
Content-Type: application/json

{
  "email": "ahmed@example.com",
  "password": "Test@123",
  "deviceInfo": "Chrome/Windows"
}
```

---

## ??? Database

### Connection String
Located in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;..."
  }
}
```

### Migrations
```bash
# List migrations
cd Server/src/Mentora.Infrastructure
dotnet ef migrations list --startup-project ../Mentora.Api

# Apply migrations
dotnet ef database update --startup-project ../Mentora.Api
```

---

## ?? Configuration

### JWT Settings
Located in `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "super_secret_key_123456789_must_be_long",
    "Issuer": "MentoraApi",
    "Audience": "MentoraClient"
  }
}
```

### Password Requirements
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 special character

---

## ?? Features

### ? Implemented
- User Registration with BCrypt hashing
- JWT Access Tokens (60 min expiry)
- Refresh Tokens (7 days expiry)
- Password Reset Flow
- Logout & Logout All Devices
- Token Cleanup Background Service (runs every 24 hours)
- **? Swagger UI Auto-Enabled** (NEW!)

### ?? In Progress
- Career Builder Module
- Study Scheduler Module
- AI Integration Module

---

## ?? Documentation

### Quick Links
- [Database Setup Guide (Arabic)](docs/DATABASE-SETUP-SUMMARY.md)
- [Quick Start Guide (Arabic)](docs/QUICK-START-AR.md)
- [Swagger Guide (Arabic)](docs/SWAGGER-GUIDE-AR.md) ?
- [Swagger Auto-Enable](docs/SWAGGER-AUTO-ENABLE.md) ? NEW
- [API Quick Reference](docs/API-QUICK-REFERENCE.md)
- [Changes Summary](docs/CHANGES-SUMMARY.md)
- [Authentication Module Documentation](docs/MODULE-1-AUTHENTICATION.md)

### API Documentation
Swagger UI provides comprehensive API documentation with:
- Request/Response examples
- Model schemas
- Authentication requirements
- Try-it-out functionality
- **? Auto-enabled in all environments!**

---

## ??? Development

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Health Check
```bash
# PowerShell
.\Server\scripts\health-check.ps1

# Bash
./Server/scripts/health-check.sh
```

---

## ?? CORS Configuration

Default CORS policy allows all origins for development:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

?? **Note**: Update CORS policy for production deployment!

---

## ?? Security Features

- BCrypt password hashing
- JWT token-based authentication
- Refresh token rotation
- Token expiration validation
- Soft delete for data retention
- Background service for token cleanup
- **? Swagger accessible but can be secured if needed**

---

## ?? Troubleshooting

### Common Issues

**Database Connection Failed**
- Check SQL Server is running
- Verify connection string in `appsettings.json`

**Migrations Not Found**
```bash
cd Server/src/Mentora.Infrastructure
dotnet ef migrations add InitialMigration --startup-project ../Mentora.Api
dotnet ef database update --startup-project ../Mentora.Api
```

**Swagger Not Loading**
- Ensure project is running
- Check browser console for errors
- Try `https://localhost:7xxx/` (should open automatically!)
- **? No need for Development environment anymore**

---

## ? What's New in v1.1

### Swagger Auto-Enable
- ? Swagger now works automatically in **all environments**
- ? Opens directly at root URL (`/`)
- ? No need to add `/swagger` to URL
- ? Perfect for API documentation and testing
- ? Can be secured or disabled if needed

See [SWAGGER-AUTO-ENABLE.md](docs/SWAGGER-AUTO-ENABLE.md) for details.

---

## ?? Support

For issues or questions:
1. Check documentation in `Server/docs/`
2. Review Swagger API documentation at `https://localhost:7xxx/`
3. Check build logs for errors

---

## ?? License

This project is licensed under the MIT License.

---

## ?? Team

Mentora Development Team

**Last Updated**: December 31, 2024 - v1.1
**Latest Feature**: Swagger Auto-Enable! ??
