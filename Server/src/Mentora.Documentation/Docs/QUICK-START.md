# Quick Start Guide - Mentora Platform

## ?? Get Started in 5 Minutes

### Prerequisites
- ? .NET 9 SDK
- ? SQL Server
- ? Node.js (for frontend)

---

## Backend Setup

### 1. Run the Backend
```bash
cd Server/src/Mentora.Api
dotnet run
```

**? Expected Output**:
```
Now listening on: https://localhost:7000
Application started.
```

### 2. Access Swagger
Open browser: `https://localhost:7000/swagger`

---

## Frontend Setup

### 1. Install Dependencies
```bash
cd Client
npm install
```

### 2. Run Frontend
```bash
npm run dev
```

**? Expected Output**:
```
VITE ready in XXX ms
?  Local:   http://localhost:8000/
```

### 3. Access App
Open browser: `http://localhost:8000`

---

## Test the System

### Using Swagger UI

1. **Register User**
   - POST `/api/auth/register`
   ```json
   {
     "firstName": "Test",
     "lastName": "User",
     "email": "test@mentora.com",
     "password": "Test@123"
   }
   ```

2. **Login**
   - POST `/api/auth/login`
   ```json
   {
     "email": "test@mentora.com",
     "password": "Test@123"
   }
   ```

3. **Copy Access Token** from response

4. **Click "Authorize"** button in Swagger

5. **Paste Token** and click "Authorize"

6. **Test Protected Endpoints**
   - GET `/api/userprofile`
   - PUT `/api/userprofile`

---

## Using the Frontend

### 1. Register Account
- Go to: `http://localhost:8000/signup`
- Fill form and submit

### 2. Login
- Go to: `http://localhost:8000/login`
- Enter credentials

### 3. Complete Profile
- Go to: `http://localhost:8000/profile`
- Click "Edit Profile"
- Fill required fields:
  - University
  - Major
  - Graduation Year
  - Study Level
  - Timezone
- Click "Save"

---

## Test Users (Pre-seeded)

| Name | Email | Password | Profile |
|------|-------|----------|---------|
| Saad Ahmad | saad@mentora.com | Saad@123 | Senior, CS Student |
| Maria Haddad | maria@mentora.com | Maria@123 | Graduate, Data Science |

---

## Available Endpoints

### Authentication
- POST `/api/auth/register` - Register new user
- POST `/api/auth/login` - Login
- POST `/api/auth/refresh-token` - Refresh access token
- POST `/api/auth/logout` - Logout
- POST `/api/auth/forgot-password` - Request password reset
- POST `/api/auth/reset-password` - Reset password
- GET `/api/auth/me` - Get current user info

### User Profile
- GET `/api/userprofile` - Get profile
- PUT `/api/userprofile` - Update profile
- GET `/api/userprofile/exists` - Check if profile exists
- GET `/api/userprofile/completion` - Get completion %
- GET `/api/userprofile/timezones` - Get timezone list
- GET `/api/userprofile/validate-timezone` - Validate timezone

---

## Project Structure

```
Mentora/
??? Server/              # Backend (.NET 9)
?   ??? src/
?       ??? Mentora.Api           # API Layer
?       ??? Mentora.Application   # Business Logic
?       ??? Mentora.Domain        # Entities
?       ??? Mentora.Infrastructure# Data Access
?
??? Client/              # Frontend (React)
    ??? src/
    ?   ??? pages/       # Pages
    ?   ??? components/  # Components
    ?   ??? contexts/    # State Management
    ?   ??? services/    # API Services
    ??? public/          # Assets
```

---

## Troubleshooting

### Backend not starting?
```bash
# Check if port 7000 is available
netstat -ano | findstr :7000

# If busy, kill the process or change port in launchSettings.json
```

### Frontend not starting?
```bash
# Clear cache and reinstall
rm -rf node_modules
npm install
npm run dev
```

### Database issues?
```bash
# Drop and recreate database
cd Server/src/Mentora.Api
dotnet ef database drop --force
dotnet run
```

---

## Next Steps

1. ? Complete your profile
2. ?? Explore Module 2 features
3. ?? Check [MODULE-1-AUTHENTICATION.md](./MODULE-1-AUTHENTICATION.md)
4. ?? Check [MODULE-2-USER-PROFILE.md](./MODULE-2-USER-PROFILE.md)
5. ?? Review [API-QUICK-REFERENCE.md](./API-QUICK-REFERENCE.md)

---

**Last Updated**: 2024-12-31  
**Status**: ? Ready to Use
