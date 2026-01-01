# ??? Database Configuration Guide - Mentora Platform

## ? Multi-Provider Database Setup

Mentora ???? ???? ??????? ???? ????? ?????? ??????! ?? ?? ???? ????? ??? provider ??? ??? ????.

---

## ?? ??? Providers ????????

### 1. Local (?????????)
```json
"Local": "Server=.;Database=MentoraDb;User ID=sa;Password=Infinite@XY2;..."
```
- SQL Server ??? ????? ??????
- Database name: `MentoraDb`

### 2. Development
```json
"Development": "Server=.;Database=MentoraDb_Dev;User ID=sa;Password=Infinite@XY2;..."
```
- Database ????? ???????
- Database name: `MentoraDb_Dev`

### 3. RemoteServer
```json
"RemoteServer": "Server=YOUR_REMOTE_SERVER;Database=MentoraDb;..."
```
- SQL Server ??? ????? ????
- ?? IP ?? ??? ???????

### 4. Azure
```json
"Azure": "Server=YOUR_AZURE_SERVER.database.windows.net;Database=MentoraDb;..."
```
- Azure SQL Database
- Encrypt=True (???)

### 5. Docker
```json
"Docker": "Server=localhost,1433;Database=MentoraDb;..."
```
- SQL Server ?? Docker container
- Port: 1433

### 6. Production
```json
"Production": "Server=YOUR_PROD_SERVER;Database=MentoraDb_Prod;..."
```
- Production database
- ?????? credentials ????

---

## ?? ????? ??????? ??? Providers

### Option 1: ????? appsettings.json (??????)

???? `Server/src/Mentora.Api/appsettings.json`:

```json
{
  "ActiveDatabase": "Local",  // ?? ???? ???!
  
  "ConnectionStrings": {
    "Local": "...",
    "RemoteServer": "...",
    "Azure": "...",
    "Docker": "...",
    "Development": "...",
    "Production": "..."
  }
}
```

**????**: ??????? ?? Azure:
```json
"ActiveDatabase": "Azure",
```

**????**: ??????? ?? Docker:
```json
"ActiveDatabase": "Docker",
```

---

### Option 2: ??? Environment Variable

#### Windows (CMD):
```cmd
set ActiveDatabase=Azure
dotnet run
```

#### Windows (PowerShell):
```powershell
$env:ActiveDatabase = "Azure"
dotnet run
```

#### Linux/Mac:
```bash
export ActiveDatabase=Azure
dotnet run
```

---

### Option 3: ??? Launch Settings

???? `Server/src/Mentora.Api/Properties/launchSettings.json`:

```json
{
  "profiles": {
    "https": {
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ActiveDatabase": "Docker"  // ?? ??? ???
      }
    }
  }
}
```

---

## ?? ????? ?????????

### ???? 1: ??????? Local Database

```json
// appsettings.json
{
  "ActiveDatabase": "Local"
}
```

```bash
cd Server/src/Mentora.Api
dotnet run
```

**Output**:
```
? Using Database: Local
? Connection: Server=.;Database=MentoraDb;User ID=sa;Password=...
```

---

### ???? 2: ??????? Azure SQL

1. **??? Connection String** ?? `appsettings.json`:
```json
"Azure": "Server=myserver.database.windows.net;Database=MentoraDb;User ID=myuser;Password=MyPassword123!;Encrypt=True;TrustServerCertificate=False"
```

2. **???? Azure**:
```json
"ActiveDatabase": "Azure"
```

3. **Run**:
```bash
dotnet run
```

---

### ???? 3: ??????? Docker SQL Server

1. **???? Docker Container**:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

2. **??? Connection String**:
```json
"Docker": "Server=localhost,1433;Database=MentoraDb;User ID=sa;Password=YourStrong@Password;MultipleActiveResultSets=True;Encrypt=False"
```

3. **???? Docker**:
```json
"ActiveDatabase": "Docker"
```

4. **Run**:
```bash
dotnet run
```

---

### ???? 4: ??????? Remote Server

1. **??? Connection String** ?? IP ???????:
```json
"RemoteServer": "Server=192.168.1.100;Database=MentoraDb;User ID=sa;Password=RemotePass@123;MultipleActiveResultSets=True;Encrypt=False"
```

2. **???? RemoteServer**:
```json
"ActiveDatabase": "RemoteServer"
```

3. **Run**:
```bash
dotnet run
```

---

## ?? Security Best Practices

### ? ?? ???? (Development ???):
```json
"Local": "Server=.;Database=MentoraDb;User ID=sa;Password=123;..."
```

### ? ???? (Production):
```json
"Production": "Server=prod.server.com;Database=MentoraDb_Prod;User ID=prod_user;Password=VeryStrongP@ssw0rd!WithNumbers123;Encrypt=True"
```

### Best Practices:
1. ? ?????? **User Secrets** ??? Development
2. ? ?????? **Azure Key Vault** ??? Production
3. ? ?? ?commit passwords ?? Git
4. ? ?????? strong passwords
5. ? ???? Encryption ?? Production

---

## ?? ??????? User Secrets (Recommended)

### 1. Setup User Secrets

```bash
cd Server/src/Mentora.Api
dotnet user-secrets init
```

### 2. Add Connection String

```bash
dotnet user-secrets set "ConnectionStrings:Local" "Server=.;Database=MentoraDb;User ID=sa;Password=MySecretPassword;..."
```

### 3. Use in Code

?? ????? ????? ??! ???????? ???? ?? User Secrets ????????.

---

## ?? Testing

### Test Connection

```bash
cd Server/src/Mentora.Api
dotnet run
```

**Success Output**:
```
? Using Database: Local
? Connection: Server=.;Database=MentoraDb;User ID=sa;Password=...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
```

**Error Output**:
```
System.InvalidOperationException: Connection string 'WrongName' not found. Check appsettings.json
```

---

## ?? Quick Reference

| Provider | Use Case | Encrypt | Server |
|----------|----------|---------|--------|
| **Local** | Development on your PC | False | `.` (localhost) |
| **Development** | Separate dev database | False | `.` |
| **RemoteServer** | Remote SQL Server | False/True | IP or hostname |
| **Azure** | Azure SQL Database | True (required) | `*.database.windows.net` |
| **Docker** | SQL in Docker | False | `localhost,1433` |
| **Production** | Production environment | True (required) | Production server |

---

## ??? Troubleshooting

### Issue: Connection string not found

**Error**:
```
InvalidOperationException: Connection string 'MyDB' not found
```

**Solution**:
1. Check spelling in `ActiveDatabase`
2. Make sure `ConnectionStrings:MyDB` exists
3. Check `appsettings.json` syntax

---

### Issue: Cannot connect to server

**Error**:
```
SqlException: A network-related or instance-specific error occurred
```

**Solution**:
1. Check server is running
2. Check firewall rules
3. Verify connection string
4. Ping server: `ping YOUR_SERVER`
5. Test with SQL Server Management Studio first

---

### Issue: Login failed

**Error**:
```
SqlException: Login failed for user 'sa'
```

**Solution**:
1. Verify username and password
2. Check SQL Server authentication is enabled
3. Check user has permissions

---

## ?? Examples for Different Scenarios

### Scenario 1: Team Development

**Dev 1** (Local SQL Server):
```json
"ActiveDatabase": "Local"
```

**Dev 2** (Docker):
```json
"ActiveDatabase": "Docker"
```

**Dev 3** (Remote Server):
```json
"ActiveDatabase": "RemoteServer"
```

---

### Scenario 2: CI/CD Pipeline

**GitHub Actions**:
```yaml
env:
  ActiveDatabase: Azure
  ConnectionStrings__Azure: ${{ secrets.AZURE_CONNECTION_STRING }}
```

---

### Scenario 3: Multi-Environment

```json
// Development
"ActiveDatabase": "Development"

// Staging
"ActiveDatabase": "Staging"

// Production
"ActiveDatabase": "Production"
```

---

## ? Summary

### ???? ????:
- ? 6 providers ??????
- ? ????? ??? ??? `ActiveDatabase`
- ? Support ??? ????? SQL Server
- ? Logging ?????? ?? database ??????
- ? Error handling ????

### ?????????:
1. ???? provider ?? ???????
2. ??? connection string ??? ???
3. ???? `ActiveDatabase`
4. Run!

---

**Last Updated**: 2024-12-31  
**Version**: 1.0.0  
**Status**: ? Production Ready
