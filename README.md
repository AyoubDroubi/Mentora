# Mentora - AI-Powered Academic & Career Platform

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18-61DAFB)](https://reactjs.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

?? **AI-powered platform for academic excellence and career success**

---

## ?? Quick Start

### Backend (ASP.NET Core 9)
```bash
cd Server/src/Mentora.Api
dotnet run
```

**Swagger UI**: https://localhost:7263/

### Frontend (React + Vite)
```bash
cd Client
npm install
npm run dev
```

**App**: http://localhost:5173/

---

## ?? Project Structure

```
Mentora/
??? Server/                      # Backend (.NET 9)
?   ??? src/
?   ?   ??? Mentora.Api/        # Web API
?   ?   ??? Mentora.Application/ # Business Logic
?   ?   ??? Mentora.Domain/     # Entities & Interfaces
?   ?   ??? Mentora.Infrastructure/ # Data Access
?   ??? docs/                   # Documentation
?   ??? scripts/                # Utility Scripts
?
??? Client/                     # Frontend (React)
?   ??? src/
?   ?   ??? pages/             # React Pages
?   ?   ??? contexts/          # Context API
?   ?   ??? components/        # Reusable Components
?   ?   ??? assets/            # Images & Assets
?   ??? package.json
?
??? README.md                   # This file
```

---

## ? Features

### ?? Authentication (Module 1)
- ? User Registration & Login
- ? JWT Access & Refresh Tokens
- ? Password Reset Flow
- ? Multi-device Session Management

### ?? Study Tools
- ?? Pomodoro Timer
- ?? To-Do Lists
- ?? Digital Notes
- ?? Attendance Tracking

### ?? Career Builder
- ?? Career Assessments
- ?? Personalized Career Plans
- ?? Skills Tracking
- ?? Progress Analytics

### ?? AI Features
- ?? Intelligent Study Plans
- ?? Smart Recommendations
- ?? Career Guidance

---

## ??? Database

### Connection
SQL Server with Entity Framework Core

### Seeding
**Automatic seeding with 2 test users:**

**User 1**: Saad Ahmad
```
Email: saad@mentora.com
Password: Saad@123
Role: Full Stack Developer Path
```

**User 2**: Maria Haddad
```
Email: maria@mentora.com
Password: Maria@123
Role: Data Scientist Path
```

---

## ?? Documentation

### Getting Started
- [Quick Start Guide (Arabic)](Server/docs/QUICK-START-AR.md)
- [Database Setup](Server/docs/DATABASE-SETUP-SUMMARY.md)
- [Database Seeder](Server/docs/DATABASE-SEEDER.md)

### API Documentation
- [Swagger Guide (Arabic)](Server/docs/SWAGGER-GUIDE-AR.md)
- [API Quick Reference](Server/docs/API-QUICK-REFERENCE.md)
- [Authentication Module](Server/docs/MODULE-1-AUTHENTICATION.md)

### Recent Updates
- [Swagger Integration](Server/docs/SWAGGER-INTEGRATION-SUMMARY.md)
- [Changes Summary](Server/docs/CHANGES-SUMMARY.md)

---

## ??? Technologies

### Backend
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt Password Hashing
- Swagger/OpenAPI

### Frontend
- React 18
- Vite
- Tailwind CSS
- React Router
- Lucide Icons

---

## ?? Testing

### Using Swagger UI
1. Run backend: `dotnet run`
2. Open: https://localhost:7263/
3. Try endpoints with built-in UI

### Test Users
Login with seeded users (see Database section above)

---

## ?? Current Status

### ? Implemented
- Authentication System (Complete)
- Database with Seeding
- Swagger Documentation
- User Profiles
- Career Plans Structure
- Skills & Achievements System

### ?? In Progress
- Career Builder UI
- Study Scheduler
- AI Integration

### ?? Planned
- Real-time Notifications
- Team Collaboration
- Mobile App

---

## ?? Contributing

This is an academic project. For major changes, please open an issue first.

---

## ?? Support

For issues or questions:
1. Check documentation in `Server/docs/`
2. Review Swagger API docs
3. Check build logs

---

## ?? License

MIT License - See LICENSE file for details

---

## ?? Team

Mentora Development Team

**Last Updated**: December 31, 2024
**Version**: 1.1
**Status**: ? Active Development

---

## ?? Quick Commands

```bash
# Backend
cd Server/src/Mentora.Api
dotnet run                          # Run API
dotnet build                        # Build solution
dotnet ef database update           # Apply migrations

# Frontend
cd Client
npm install                         # Install dependencies
npm run dev                         # Run dev server
npm run build                       # Build for production

# Database
cd Server/src/Mentora.Infrastructure
dotnet ef migrations add NAME       # Create migration
dotnet ef database drop --force     # Drop database
dotnet ef database update           # Apply migrations
```

---

**Made with ?? by Mentora Team**