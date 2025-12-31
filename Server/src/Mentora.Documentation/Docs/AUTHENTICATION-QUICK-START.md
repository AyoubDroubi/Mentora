# Quick Start - Authentication Integration

## ?? Get Started in 5 Minutes

This guide will help you quickly set up and test the authentication system.

---

## Prerequisites

- Node.js 16+ installed
- .NET 9 SDK installed
- SQL Server running (local or remote)
- Visual Studio Code or Visual Studio 2022

---

## Step 1: Install Dependencies

### Backend
```bash
cd Server/src/Mentora.Api
dotnet restore
dotnet build
```

### Frontend
```bash
cd Client
npm install
```

---

## Step 2: Configure Environment

### Backend - Update Database Connection

Edit `Server/src/Mentora.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;..."
  }
}
```

### Frontend - Set API URL

Create `Client/.env`:
```env
VITE_API_URL=https://localhost:7136/api
```

---

## Step 3: Run Database Migration

```bash
cd Server/src/Mentora.Api
dotnet ef database update
```

---

## Step 4: Start the Application

### Terminal 1 - Start Backend
```bash
cd Server/src/Mentora.Api
dotnet run
```
? Backend running at: `https://localhost:7136`

### Terminal 2 - Start Frontend
```bash
cd Client
npm run dev
```
? Frontend running at: `http://localhost:5173`

---

## Step 5: Test the Authentication

### 1. Open Browser
Navigate to: `http://localhost:5173`

### 2. Register New Account
1. Click "Sign Up" or navigate to `/signup`
2. Fill in the form:
   - First Name: `Ahmed`
   - Last Name: `Mohammed`
   - Email: `ahmed@example.com`
   - Password: `Test@123`
   - Confirm Password: `Test@123`
   - Check "I agree to terms"
3. Click "Create Account"
4. You'll be redirected to login page

### 3. Login
1. Enter email: `ahmed@example.com`
2. Enter password: `Test@123`
3. Click "Login"
4. You'll be redirected to Dashboard

### 4. Test Protected Routes
Try accessing:
- `/dashboard` ? (You should see the dashboard)
- `/profile` ? (Protected, accessible)
- Logout and try `/dashboard` ? (Redirects to login)

---

## ?? Test API Endpoints with Swagger

1. Open: `https://localhost:7136/swagger`
2. Try these endpoints:

### Register User
```
POST /api/auth/register
Body:
{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "password": "Test@123"
}
```

### Login
```
POST /api/auth/login
Body:
{
  "email": "test@example.com",
  "password": "Test@123"
}
```

### Get Current User (requires token)
```
GET /api/auth/me
Headers:
Authorization: Bearer YOUR_ACCESS_TOKEN
```

---

## ?? Verify Authentication

### Check Browser Console
```javascript
// In browser console, check stored tokens:
localStorage.getItem('accessToken')
localStorage.getItem('refreshToken')
localStorage.getItem('user')
```

### Check Network Tab
1. Open DevTools (F12)
2. Go to Network tab
3. Login
4. Look for:
   - POST to `/api/auth/login` ? Should return 200 with tokens
   - Subsequent requests should include `Authorization: Bearer ...` header

---

## ? Success Checklist

After completing the quick start, you should be able to:

- [x] Register a new user account
- [x] Login with email and password
- [x] Access protected routes when logged in
- [x] See user info in the UI
- [x] Get redirected to login when accessing protected routes without auth
- [x] Logout successfully
- [x] Request password reset (backend ready, email not configured)

---

## ?? What's Next?

Now that authentication is working, you can:

1. **Integrate with existing pages**
   - Add `<UserInfo />` to headers/sidebars
   - Add `<LogoutButton />` to navigation
   - Use `useAuth()` hook to access user data

2. **Protect additional routes**
   - Wrap routes with `<ProtectedRoute>` in App.jsx

3. **Make authenticated API calls**
   ```javascript
   import api from '../services/api';
   
   const fetchData = async () => {
     const response = await api.get('/your-endpoint');
     return response.data;
   };
   ```

4. **Customize user experience**
   - Show different content based on `isAuthenticated`
   - Display user's name in welcome messages
   - Implement role-based features

---

## ? Common Issues

### Issue: Backend won't start
**Error:** "Port 7136 already in use"
```bash
# Find and kill process using port 7136
netstat -ano | findstr :7136
taskkill /PID <PID> /F
```

### Issue: Frontend won't start
**Error:** "Port 5173 already in use"
```bash
# Vite will automatically try next available port
# Or specify a different port:
npm run dev -- --port 5174
```

### Issue: CORS error
**Error:** "has been blocked by CORS policy"
- Ensure backend CORS includes your frontend URL
- Check both are running on correct ports
- Clear browser cache

### Issue: Database connection failed
**Error:** "Cannot open database"
- Verify SQL Server is running
- Check connection string in appsettings.json
- Run `dotnet ef database update`

---

## ?? Need Help?

- Check the detailed [Authentication Setup Guide](./AUTHENTICATION-SETUP-GUIDE.md)
- Review [API Documentation](./API-QUICK-REFERENCE.md)
- Check backend logs in terminal
- Check browser console for frontend errors

---

**Estimated Setup Time:** 5-10 minutes  
**Difficulty Level:** Beginner  
**Last Updated:** 2025-01-XX
