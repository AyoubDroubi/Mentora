# Mentora Platform - Documentation

Welcome to the **Mentora Platform** - An intelligent career guidance and study planning system for university students.

---

## 📚 What is Mentora?

Mentora is a comprehensive platform designed to help university students:
- **Plan their careers** with AI-powered guidance
- **Track their progress** through gamification and analytics
- **Manage their studies** with intelligent scheduling
- **Build their skills** with personalized recommendations

---

## 🚀 Quick Start

### For Developers
1. **Setup**: See [QUICK-START.md](./QUICK-START.md)
2. **Architecture**: See [architecture/01-ARCHITECTURE-OVERVIEW.md](./architecture/01-ARCHITECTURE-OVERVIEW.md)
3. **API Reference**: See [API-QUICK-REFERENCE.md](./API-QUICK-REFERENCE.md)

### For Users
1. **Authentication**: See [MODULE-1-AUTHENTICATION.md](./MODULE-1-AUTHENTICATION.md)
2. **User Profile**: See [MODULE-2-USER-PROFILE.md](./MODULE-2-USER-PROFILE.md)

---

## 📖 Core Documentation

### Main Guides
| Document | Description |
|----------|-------------|
| [00-DOCUMENTATION-INDEX.md](./00-DOCUMENTATION-INDEX.md) | Complete documentation index |
| [QUICK-START.md](./QUICK-START.md) | Quick start guide for developers |
| [API-QUICK-REFERENCE.md](./API-QUICK-REFERENCE.md) | API endpoints reference |

### Module Documentation
| Module | Description | Status |
|--------|-------------|--------|
| [MODULE-1-AUTHENTICATION.md](./MODULE-1-AUTHENTICATION.md) | User authentication & security | ✅ Complete |
| [MODULE-2-USER-PROFILE.md](./MODULE-2-USER-PROFILE.md) | User profiles & personalization | ✅ Complete |

### Technical Documentation
| Document | Description |
|----------|-------------|
| [architecture/01-ARCHITECTURE-OVERVIEW.md](./architecture/01-ARCHITECTURE-OVERVIEW.md) | System architecture (Clean Architecture) |
| [architecture/03-PROJECT-STRUCTURE.md](./architecture/03-PROJECT-STRUCTURE.md) | Project structure and organization |
| [domain/01-DOMAIN-OVERVIEW.md](./domain/01-DOMAIN-OVERVIEW.md) | Domain entities and models |
| [domain/03-ENUMS-REFERENCE.md](./domain/03-ENUMS-REFERENCE.md) | Enumerations reference |
| [application/01-APPLICATION-OVERVIEW.md](./application/01-APPLICATION-OVERVIEW.md) | Application services and DTOs |

---

## 🏗️ System Architecture

```
┌─────────────────────────────────────────┐
│           Mentora Platform              │
├─────────────────────────────────────────┤
│                                         │
│  ┌─────────────┐     ┌──────────────┐  │
│  │  Frontend   │────▶│   Backend    │  │
│  │  (React)    │     │  (.NET 9)    │  │
│  └─────────────┘     └──────────────┘  │
│                            │            │
│                            ▼            │
│                   ┌─────────────────┐   │
│                   │  SQL Server DB  │   │
│                   └─────────────────┘   │
│                                         │
└─────────────────────────────────────────┘
```

**Clean Architecture Layers**:
1. **Domain** - Core business entities
2. **Application** - Business logic & services
3. **Infrastructure** - Data access & external services
4. **API** - REST API endpoints

---

## 🎯 Key Features

### ✅ Implemented
- **Module 1**: Authentication & Security
  - User registration & login
  - JWT token-based authentication
  - Password reset functionality
  - Multi-device session management

- **Module 2**: User Profile & Personalization
  - Complete profile management
  - Academic attributes tracking
  - Timezone synchronization
  - Profile completion tracking

### 🔄 In Development
- **Module 3**: AI Career Builder
- **Module 4**: Study Planner
- **Module 5**: Deep Integration
- **Module 6**: Gamification
- **Module 7**: System Monitoring

---

## 🛠️ Technology Stack

### Backend (.NET 9)
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI

### Frontend (React)
- React 18
- React Router
- Axios
- Tailwind CSS
- Vite

---

## 📦 Project Structure

```
Mentora/
├── Server/
│   └── src/
│       ├── Mentora.Api           # REST API Layer
│       ├── Mentora.Application   # Business Logic
│       ├── Mentora.Domain        # Core Entities
│       ├── Mentora.Infrastructure# Data Access
│       └── Mentora.Documentation # This documentation
│
└── Client/                       # React Frontend
    ├── src/
    │   ├── components/          # Reusable components
    │   ├── pages/               # Page components
    │   ├── contexts/            # React contexts
    │   └── services/            # API services
    └── public/                  # Static assets
```

---

## 🔗 Important Links

| Resource | URL |
|----------|-----|
| **API (Backend)** | `https://localhost:7000` |
| **Swagger UI** | `https://localhost:7000/swagger` |
| **Frontend** | `http://localhost:8000` |

---

## 📝 Contributing

When updating documentation:
1. Follow existing markdown structure
2. Use clear, concise language
3. Include code examples when helpful
4. Update index files accordingly

---

## 📧 Support

For questions or issues:
1. Check the documentation index
2. Review module-specific guides
3. Consult the API reference

---

**Last Updated:** 2024-12-31  
**Version:** 1.0.0  
**Status:** Active Development
