# ?? Database Quick Reference - Mentora

## ?? Quick Switch Commands

### Switch to Local
```json
"ActiveDatabase": "Local"
```

### Switch to Azure
```json
"ActiveDatabase": "Azure"
```

### Switch to Docker
```json
"ActiveDatabase": "Docker"
```

### Switch to Remote
```json
"ActiveDatabase": "RemoteServer"
```

---

## ?? Connection String Templates

### Local SQL Server
```
Server=.;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=False
```

### Remote SQL Server
```
Server=192.168.1.100;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=False
```

### Azure SQL Database
```
Server=YOUR_SERVER.database.windows.net;Database=MentoraDb;User ID=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=False
```

### Docker SQL Server
```
Server=localhost,1433;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=False
```

---

## ? Common Commands

### Check Active Database
```bash
cd Server/src/Mentora.Api
dotnet run
```

Look for:
```
? Using Database: Local
? Connection: Server=.;Database=...
```

### Apply Migrations
```bash
dotnet ef database update
```

### Create New Migration
```bash
dotnet ef migrations add MigrationName
```

---

## ?? Troubleshooting

| Issue | Solution |
|-------|----------|
| Connection string not found | Check spelling in `ActiveDatabase` |
| Cannot connect | Verify server is running |
| Login failed | Check username/password |
| Migration error | Run `dotnet ef database update` |

---

## ?? Files to Edit

| File | Purpose |
|------|---------|
| `appsettings.json` | Production settings, add providers |
| `appsettings.Development.json` | Development overrides |
| `Program.cs` | Already configured ? |

---

**Quick Help**: See [DATABASE-CONFIGURATION.md](./DATABASE-CONFIGURATION.md) for full guide
