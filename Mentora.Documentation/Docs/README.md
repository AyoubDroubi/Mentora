# Mentora Platform - Documentation

## Welcome to the Mentora Platform Documentation

This repository contains comprehensive documentation for all aspects of the Mentora platform development.

---

## Getting Started

### Main Documentation Index
See **[00-DOCUMENTATION-INDEX.md](./00-DOCUMENTATION-INDEX.md)** - Complete documentation index with all sections

---

## Documentation Structure

### 📁 [`architecture/`](./architecture/)
Documentation on architecture and Clean Architecture
- [01-ARCHITECTURE-OVERVIEW.md](./architecture/01-ARCHITECTURE-OVERVIEW.md)
- [03-PROJECT-STRUCTURE.md](./architecture/03-PROJECT-STRUCTURE.md)

### 📁 [`domain/`](./domain/)
Documentation for Domain layer (entities and enums)
- [01-DOMAIN-OVERVIEW.md](./domain/01-DOMAIN-OVERVIEW.md)
- [03-ENUMS-REFERENCE.md](./domain/03-ENUMS-REFERENCE.md)

### 📁 [`application/`](./application/)
Documentation for Application layer (services and DTOs)
- [01-APPLICATION-OVERVIEW.md](./application/01-APPLICATION-OVERVIEW.md)

### 📁 [`infrastructure/`](./infrastructure/) (Coming Soon)
Documentation for Infrastructure layer

### 📁 [`api/`](./api/) (Coming Soon)
Documentation for API layer and Controllers

### 📁 [`database/`](./database/) (Coming Soon)
Database documentation

### 📁 [`security/`](./security/) (Coming Soon)
Security and authentication documentation

---

## Quick Start Guides

- [API-QUICK-REFERENCE.md](./API-QUICK-REFERENCE.md) - Quick API reference
- [MODULE-1-AUTHENTICATION.md](./MODULE-1-AUTHENTICATION.md) - Authentication module
- [SWAGGER-GUIDE.md](./SWAGGER-GUIDE.md) - Swagger guide
- [DATABASE-SETUP-SUMMARY.md](./DATABASE-SETUP-SUMMARY.md) - Database setup summary
- [QUICK-START.md](./QUICK-START.md) - Quick start guide

---

## Documentation by Role

| Role | Start Here |
|------|------------|
| **New Developer** | [QUICK-START.md](./QUICK-START.md) |
| **Backend Developer** | [architecture/01-ARCHITECTURE-OVERVIEW.md](./architecture/01-ARCHITECTURE-OVERVIEW.md) |
| **Frontend Developer** | [API-QUICK-REFERENCE.md](./API-QUICK-REFERENCE.md) |
| **Database Admin** | [DATABASE-SETUP-SUMMARY.md](./DATABASE-SETUP-SUMMARY.md) |

---

## Project Structure Overview

```
docs/
├── 00-DOCUMENTATION-INDEX.md        # Complete documentation index
├── README.md                         # This file
│
├── architecture/                     # Architecture documentation
│   ├── 01-ARCHITECTURE-OVERVIEW.md  
│   └── 03-PROJECT-STRUCTURE.md      
│
├── domain/                           # Domain layer
│   ├── 01-DOMAIN-OVERVIEW.md        
│   └── 03-ENUMS-REFERENCE.md        
│
├── application/                      # Application layer
│   └── 01-APPLICATION-OVERVIEW.md   
│
└── [Additional sections]
    ├── API-QUICK-REFERENCE.md       
    ├── MODULE-1-AUTHENTICATION.md   
    ├── SWAGGER-GUIDE.md             
    └── ... more files
```

---

## Tips for Navigation

Use **Ctrl+P** in VS Code to quickly search for files!

Example: `Ctrl+P` then type `01-ARCH` to find Architecture overview

---

## Contributing

All documentation is written in English using Markdown format. When contributing:
- Follow the existing structure
- Use clear headings and sections
- Include code examples where applicable
- Keep language professional and concise

For detailed contribution guidelines, see the contributing section in the main documentation index.

---

**Last Updated:** 2024-12-31  
**Version:** 1.0.0
