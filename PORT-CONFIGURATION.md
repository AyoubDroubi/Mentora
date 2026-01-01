# ?? Mentora - Port Configuration

## ?? Application Ports

| Service | Protocol | Port | URL |
|---------|----------|------|-----|
| **Backend API** | HTTPS | **7000** | https://localhost:7000 |
| **Backend API** | HTTP | 5000 | http://localhost:5000 |
| **Frontend** | HTTP | **8000** | http://localhost:8000 |
| **Swagger UI** | HTTPS | 7000 | https://localhost:7000/swagger |

---

## ?? Configuration Files

### Backend (API)
?? **File**: `Server/src/Mentora.Api/Properties/launchSettings.json`

```json
{
  "profiles": {
    "https": {
      "applicationUrl": "https://localhost:7000;http://localhost:5000"
    }
  }
}
```

### Frontend (React)
?? **File**: `Client/vite.config.js`

```javascript
export default defineConfig({
  server: {
    port: 8000,
    open: true
  }
});
```

?? **File**: `Client/.env`

```env
VITE_API_URL=https://localhost:7000/api
```

### API Service
?? **File**: `Client/src/services/api.js`

```javascript
const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7000/api';
```

### CORS Configuration
?? **File**: `Server/src/Mentora.Api/Program.cs`

```csharp
policy.WithOrigins(
    "http://localhost:8000",
    "https://localhost:8000"
)
```

---

## ?? How to Run

### 1. Start Backend (Port 7000)
```bash
cd Server/src/Mentora.Api
dotnet run
```

**Expected Output**:
```
Now listening on: https://localhost:7000
Now listening on: http://localhost:5000
```

**Swagger**: Automatically opens at `https://localhost:7000/swagger`

---

### 2. Start Frontend (Port 8000)
```bash
cd Client
npm run dev
```

**Expected Output**:
```
VITE v4.4.9  ready in XXX ms

?  Local:   http://localhost:8000/
?  Network: use --host to expose
```

**Browser**: Automatically opens at `http://localhost:8000`

---

## ?? Verification Checklist

### Backend Health Check
- [ ] Navigate to: https://localhost:7000/swagger
- [ ] Should see Swagger UI with all API endpoints
- [ ] Test `/api/auth/register` endpoint

### Frontend Health Check
- [ ] Navigate to: http://localhost:8000
- [ ] Should see Mentora homepage
- [ ] Check browser console for no errors

### API Connection Test
- [ ] Open browser DevTools ? Network tab
- [ ] Login to the app
- [ ] Should see requests to `https://localhost:7000/api/*`
- [ ] Status code should be 200 OK

### CORS Test
- [ ] Login from frontend (port 8000)
- [ ] Should not see CORS errors in console
- [ ] API calls should succeed

---

## ??? Troubleshooting

### Issue: Port Already in Use

**Backend (7000)**:
```bash
# Windows
netstat -ano | findstr :7000
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:7000 | xargs kill -9
```

**Frontend (8000)**:
```bash
# Windows
netstat -ano | findstr :8000
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:8000 | xargs kill -9
```

---

### Issue: CORS Error

**Symptom**: 
```
Access to XMLHttpRequest at 'https://localhost:7000/api/...' 
from origin 'http://localhost:8000' has been blocked by CORS policy
```

**Solution**:
1. Check `Program.cs` has port 8000 in CORS policy
2. Restart backend server
3. Clear browser cache
4. Try in incognito mode

---

### Issue: SSL Certificate Error

**Symptom**: 
```
NET::ERR_CERT_AUTHORITY_INVALID
```

**Solution**:
```bash
# Trust .NET development certificate
dotnet dev-certs https --trust
```

---

### Issue: API Not Found (404)

**Checklist**:
- [ ] Backend is running on port 7000
- [ ] `Client/.env` has correct API URL
- [ ] `Client/src/services/api.js` has correct base URL
- [ ] Restart frontend dev server after .env changes

---

## ?? HTTP Test Files

### Auth Tests
?? **File**: `Server/src/Mentora.Api/Tests/auth-tests.http`
```
@baseUrl = https://localhost:7000/api
```

### Profile Tests
?? **File**: `Server/src/Mentora.Api/Tests/userprofile-tests.http`
```
@baseUrl = https://localhost:7000/api
```

**How to Use**:
1. Open `.http` file in VS Code
2. Install "REST Client" extension
3. Click "Send Request" above any request
4. View response in side panel

---

## ?? Security Notes

### Development Certificates
- Backend uses **HTTPS** on port 7000
- Certificate is self-signed for development
- Trust the certificate: `dotnet dev-certs https --trust`

### Environment Variables
- Never commit `.env` with production values
- `.env` is in `.gitignore`
- Use `.env.example` for documentation

### API Keys
- JWT Secret is in `appsettings.json`
- Change secret for production
- Use Azure Key Vault in production

---

## ?? Port Summary

```
???????????????????????????????????????????
?         Mentora Architecture            ?
???????????????????????????????????????????
?                                         ?
?  Frontend (React + Vite)                ?
?  ?? http://localhost:8000               ?
?         ?                               ?
?         ? HTTP/HTTPS                    ?
?         ?                               ?
?  Backend (.NET 9 API)                   ?
?  ?? https://localhost:7000              ?
?         ?                               ?
?         ? EF Core                       ?
?         ?                               ?
?  Database (SQL Server)                  ?
?  ?? localhost:1433                      ?
?                                         ?
???????????????????????????????????????????
```

---

## ?? Quick Reference

| Need | URL |
|------|-----|
| **API Docs** | https://localhost:7000/swagger |
| **Frontend** | http://localhost:8000 |
| **Login** | http://localhost:8000/login |
| **Profile** | http://localhost:8000/profile |
| **Dashboard** | http://localhost:8000/dashboard |

---

## ?? Related Documentation

- [Module 1: Authentication](Server/src/Mentora.Documentation/Docs/MODULE-1-AUTHENTICATION.md)
- [Module 2: User Profile](Server/src/Mentora.Documentation/Docs/MODULE-2-USER-PROFILE.md)
- [Frontend Testing Guide](Client/FRONTEND-TESTING-GUIDE.md)

---

**Last Updated**: 2024-12-31  
**Configuration Version**: 1.0.0  
**Status**: ? Active
