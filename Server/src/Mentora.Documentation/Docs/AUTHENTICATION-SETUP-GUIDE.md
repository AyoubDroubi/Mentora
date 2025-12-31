# Authentication Setup Guide - Mentora Platform

## Overview
This document provides a complete guide for the authentication system connecting the frontend (React) and backend (.NET 9 API).

---

## ?? Features Implemented

### ? User Authentication
- **Login** - JWT token-based authentication
- **Registration** - New user signup with validation
- **Password Reset** - Forgot password flow
- **Token Refresh** - Automatic token renewal
- **Logout** - Single device and all devices logout

### ? Security Features
- JWT access tokens (60 minutes validity)
- Refresh tokens (7 days validity with rotation)
- BCrypt password hashing
- CORS configuration for secure frontend communication
- Automatic token refresh on 401 errors
- Protected routes requiring authentication

---

## ?? File Structure

### Frontend (Client)
```
Client/
??? src/
?   ??? services/
?   ?   ??? api.js                      # Axios configuration with interceptors
?   ?   ??? authService.js              # Authentication API calls
?   ??? contexts/
?   ?   ??? AuthContext.jsx             # Global authentication state
?   ??? components/
?   ?   ??? ProtectedRoute.jsx          # Route protection wrapper
?   ?   ??? LogoutButton.jsx            # Reusable logout button
?   ?   ??? UserInfo.jsx                # User info display component
?   ??? pages/
?   ?   ??? Login.jsx                   # Login page with API integration
?   ?   ??? SignUp.jsx                  # Registration page
?   ?   ??? ForgotPassword.jsx          # Password reset request
?   ??? App.jsx                         # App routes with auth protection
?   ??? main.jsx
??? .env                                # Environment variables
??? package.json
```

### Backend (Server)
```
Server/src/
??? Mentora.Api/
?   ??? Controllers/
?   ?   ??? AuthController.cs           # Authentication endpoints
?   ??? Program.cs                      # CORS and JWT configuration
?   ??? appsettings.json                # JWT settings
??? Mentora.Application/
?   ??? Interfaces/
?   ?   ??? IAuthService.cs
?   ??? DTOs/
?       ??? LoginDTO.cs
?       ??? SignupDTO.cs
?       ??? AuthResponseDto.cs
??? Mentora.Infrastructure/
    ??? Services/
        ??? AuthService.cs              # Authentication business logic
        ??? TokenService.cs             # JWT token management
```

---

## ?? Configuration

### 1. Frontend Environment Variables

Create or update `Client/.env`:
```env
VITE_API_URL=https://localhost:7136/api
```

### 2. Backend Configuration

Update `Server/src/Mentora.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=False"
  },
  "Jwt": {
    "Key": "super_secret_key_123456789_must_be_long",
    "Issuer": "MentoraApi",
    "Audience": "MentoraClient"
  }
}
```

### 3. Install Dependencies

**Frontend:**
```bash
cd Client
npm install
```

**Backend:**
```bash
cd Server/src/Mentora.Api
dotnet restore
```

---

## ?? Usage

### Starting the Application

**1. Start Backend:**
```bash
cd Server/src/Mentora.Api
dotnet run
```
Backend will run on: `https://localhost:7136`

**2. Start Frontend:**
```bash
cd Client
npm run dev
```
Frontend will run on: `http://localhost:5173`

---

## ?? API Endpoints

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | Login user | No |
| POST | `/api/auth/refresh-token` | Refresh access token | No |
| POST | `/api/auth/logout` | Logout current device | Yes |
| POST | `/api/auth/logout-all` | Logout all devices | Yes |
| POST | `/api/auth/forgot-password` | Request password reset | No |
| POST | `/api/auth/reset-password` | Reset password with token | No |
| GET | `/api/auth/me` | Get current user info | Yes |

### Request/Response Examples

**Register:**
```json
// POST /api/auth/register
{
  "firstName": "Ahmed",
  "lastName": "Mohammed",
  "email": "ahmed@example.com",
  "password": "Test@123"
}

// Response
{
  "isSuccess": true,
  "message": "User registered successfully"
}
```

**Login:**
```json
// POST /api/auth/login
{
  "email": "ahmed@example.com",
  "password": "Test@123"
}

// Response
{
  "isSuccess": true,
  "message": "Login successful",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "8f5e7d4c3b2a1...",
  "tokenExpiration": "2024-12-31T23:59:59Z"
}
```

---

## ?? Authentication Flow

### 1. User Login Flow
```
User enters credentials
    ?
Frontend calls authService.login()
    ?
Backend validates credentials
    ?
Backend generates JWT + Refresh Token
    ?
Frontend stores tokens in localStorage
    ?
Frontend updates AuthContext
    ?
User redirected to Dashboard
```

### 2. Protected Route Access
```
User navigates to protected route
    ?
ProtectedRoute checks isAuthenticated
    ?
If authenticated: Render page
    ?
If not: Redirect to /login
```

### 3. Token Refresh Flow
```
API call returns 401 Unauthorized
    ?
Axios interceptor catches error
    ?
Calls /api/auth/refresh-token with refreshToken
    ?
Backend validates and issues new tokens
    ?
Frontend stores new tokens
    ?
Retries original request with new token
    ?
If refresh fails: Redirect to /login
```

---

## ?? Using Authentication in Components

### 1. Using Auth Context
```jsx
import { useAuth } from '../contexts/AuthContext';

function MyComponent() {
  const { user, isAuthenticated, logout } = useAuth();

  if (!isAuthenticated) {
    return <div>Please login</div>;
  }

  return (
    <div>
      <p>Welcome, {user.firstName}!</p>
      <button onClick={logout}>Logout</button>
    </div>
  );
}
```

### 2. Using Logout Button
```jsx
import LogoutButton from '../components/LogoutButton';

function Header() {
  return (
    <header>
      <LogoutButton variant="default" />
      {/* or */}
      <LogoutButton variant="icon" />
      {/* or */}
      <LogoutButton variant="link" />
    </header>
  );
}
```

### 3. Using User Info Component
```jsx
import UserInfo from '../components/UserInfo';

function Sidebar() {
  return (
    <aside>
      <UserInfo className="mb-4" />
    </aside>
  );
}
```

### 4. Making Authenticated API Calls
```jsx
import api from '../services/api';

// The api instance automatically adds Authorization header
const fetchUserData = async () => {
  try {
    const response = await api.get('/users/profile');
    return response.data;
  } catch (error) {
    console.error('Error:', error);
  }
};
```

---

## ?? Security Best Practices

### Implemented
? JWT tokens with short expiration (60 minutes)
? Refresh token rotation
? BCrypt password hashing
? HTTPS enforcement
? CORS restricted to specific origins
? Password complexity requirements
? Token stored in localStorage (consider httpOnly cookies for production)

### Recommendations for Production
- [ ] Move tokens to httpOnly cookies
- [ ] Implement rate limiting
- [ ] Add 2FA/MFA support
- [ ] Implement session management
- [ ] Add IP address validation
- [ ] Implement account lockout after failed attempts
- [ ] Add email verification for new accounts
- [ ] Implement CSRF protection

---

## ?? Troubleshooting

### CORS Errors
**Problem:** "Access to XMLHttpRequest has been blocked by CORS policy"

**Solution:**
1. Ensure backend is running on `https://localhost:7136`
2. Verify frontend is running on `http://localhost:5173`
3. Check CORS policy in `Program.cs` includes your frontend URL

### Token Expiration
**Problem:** User gets logged out unexpectedly

**Solution:**
- Check if refresh token is working (check browser console)
- Verify token expiration times in backend configuration
- Ensure localStorage has both `accessToken` and `refreshToken`

### Login Not Working
**Problem:** Login fails with "Invalid credentials"

**Solution:**
1. Verify backend is running and database is accessible
2. Check if user exists in database
3. Ensure password meets requirements (8+ chars, uppercase, special char)
4. Check backend logs for detailed error messages

### Protected Routes Not Working
**Problem:** Can access protected routes without login

**Solution:**
1. Ensure routes are wrapped with `<ProtectedRoute>` in App.jsx
2. Check AuthContext is providing correct `isAuthenticated` value
3. Verify tokens exist in localStorage

---

## ?? Additional Resources

- [JWT.io - JWT Decoder](https://jwt.io/)
- [React Router - Protected Routes](https://reactrouter.com/en/main/start/tutorial)
- [Axios - Interceptors](https://axios-http.com/docs/interceptors)
- [.NET Identity Documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)

---

## ?? Next Steps

### Recommended Enhancements
1. **Email Service Integration**
   - Send welcome emails on registration
   - Send password reset emails
   - Email verification

2. **User Profile Management**
   - Update profile information
   - Change password
   - Upload profile picture

3. **Social Authentication**
   - Google OAuth
   - Microsoft OAuth
   - GitHub OAuth

4. **Advanced Security**
   - Two-factor authentication (2FA)
   - Security questions
   - Login activity tracking

5. **User Management Dashboard**
   - Admin panel for user management
   - Role-based access control (RBAC)
   - User activity logs

---

**Last Updated:** 2025-01-XX  
**Version:** 1.0.0  
**Author:** Mentora Development Team
