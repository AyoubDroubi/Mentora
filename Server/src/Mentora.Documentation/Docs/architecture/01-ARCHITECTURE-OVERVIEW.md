# Architecture Overview - Mentora Platform

## Table of Contents
- [Introduction](#introduction)
- [Clean Architecture](#clean-architecture)
- [Layer Details](#layer-details)
- [Data Flow](#data-flow)
- [SOLID Principles](#solid-principles)
- [Benefits of Architecture](#benefits-of-architecture)

---

## Introduction

Mentora is an AI-powered platform for career and study planning built with professional architecture. The project follows **Clean Architecture** principles using **.NET 9** with:

- **Separation of Concerns** - Each layer has distinct responsibilities  
- **Testability** - Easy to test  
- **Maintainability** - Easy to maintain  
- **Scalability** - Easy to scale  
- **Independence** - Framework independent

---

## Clean Architecture

### What is Clean Architecture?

Clean Architecture is a software design pattern that ensures:
- **Business Logic** is independent of frameworks
- **Testing** is possible without UI, database, or external services
- **Database** can be changed without affecting business logic

### Visual Representation

```
???????????????????????????????????????
?   Clean Architecture Layers         ?
???????????????????????????????????????
?                                     ?
?    ???????????????????             ?
?    ?  Domain (Core)  ?             ?
?    ?  - Entities     ?             ?
?    ?  - Rules        ?             ?
?    ???????????????????             ?
?             ?                       ?
?    ???????????????????             ?
?    ?  Application    ?             ?
?    ?  - Use Cases    ?             ?
?    ?  - Interfaces   ?             ?
?    ???????????????????             ?
?             ?                       ?
?    ???????????????????             ?
?    ? Infrastructure  ?             ?
?    ?  - Data Access  ?             ?
?    ?  - External     ?             ?
?    ???????????????????             ?
?             ?                       ?
?    ???????????????????             ?
?    ?     API/UI      ?             ?
?    ?  - Controllers  ?             ?
?    ?  - Middleware   ?             ?
?    ???????????????????             ?
?                                     ?
???????????????????????????????????????
```

---

## Layer Details

### 1. Domain Layer (Mentora.Domain)

**Purpose:** Core business logic - no external dependencies

```
Mentora.Domain/
??? Common/
?   ??? BaseEntity.cs              # Base entity
??? Entities/
?   ??? Auth/
?   ?   ??? User.cs                # User entity
?   ?   ??? PasswordResetToken.cs  # Password reset token
?   ??? CareerPlan.cs              # Career plan
?   ??? CareerStep.cs              # Career step
?   ??? StudyPlan.cs               # Study plan
?   ??? UserProfile.cs             # User profile
?   ??? UserSkill.cs               # User skills
?   ??? UserAchievement.cs         # User achievements
?   ??? Skill.cs                   # Available skills
?   ??? RefreshToken.cs            # Refresh tokens
??? Enums/
?   ??? Enums.cs                   # All enumerations
??? AiResponse/
    ??? AiCareerPlanResponse.cs    # AI career plan responses
    ??? AiResource.cs              # AI resources
```

**Characteristics:**
- No external dependencies
- Contains only business entities
- POCO (Plain Old CLR Objects)
- Contains business validation logic

**Example - BaseEntity:**
```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
```

---

### 2. Application Layer (Mentora.Application)

**Purpose:** Business use cases and application logic

```
Mentora.Application/
??? DTOs/
?   ??? LoginDTO.cs                # Login data transfer
?   ??? SignupDTO.cs               # Registration data
?   ??? ForgotPasswordDto.cs       # Password reset request
?   ??? ResetPasswordDto.cs        # Password reset with token
??? Interfaces/
?   ??? IAuthService.cs            # Auth service interface
?   ??? ICareerService.cs          # Career service interface
?   ??? IEmailService.cs           # Email service interface
??? Services/
    ??? (Implementations in Infrastructure)
```

**Characteristics:**
- Depends on Domain only
- Contains interfaces
- Contains DTOs for data transfer
- Contains validation logic

**Example - IAuthService:**
```csharp
public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(SignupDTO dto);
    Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(Guid userId, string refreshToken);
}
```

---

### 3. Infrastructure Layer (Mentora.Infrastructure)

**Purpose:** External services implementation

```
Mentora.Infrastructure/
??? Persistence/
?   ??? ApplicationDbContext.cs    # Main DbContext
?   ??? Configurations/            # Entity Configurations
??? Services/
?   ??? AuthService.cs             # Auth implementation
?   ??? EmailService.cs            # Email service (future)
?   ??? TokenCleanupService.cs     # Token cleanup
??? Repositories/
?   ??? UserRepository.cs          # User data access
?   ??? CareerPlanRepository.cs    # Career plan data access
??? Migrations/                    # EF Core Migrations
```

**Characteristics:**
- Depends on Domain & Application
- Implements interfaces from Application
- Database access
- External service integration

**Example - ApplicationDbContext:**
```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CareerPlan> CareerPlans { get; set; }
    public DbSet<CareerStep> CareerSteps { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Entity Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```

---

### 4. API Layer (Mentora.Api)

**Purpose:** HTTP endpoints and request handling

```
Mentora.Api/
??? Controllers/
?   ??? AuthController.cs          # Authentication endpoints
?   ??? CareerController.cs        # Career endpoints
?   ??? UserController.cs          # User endpoints
??? Middleware/
?   ??? ExceptionHandlingMiddleware.cs  # Error handling
?   ??? JwtMiddleware.cs           # JWT middleware
??? Program.cs                     # Application entry point
??? appsettings.json               # Configuration
??? Tests/
    ??? auth-tests.http            # HTTP tests
```

**Characteristics:**
- Depends on Application & Infrastructure
- Contains Controllers
- Contains Middleware
- Handles Authentication & Authorization

**Example - AuthController:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] SignupDTO dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result);
    }
}
```

---

## Data Flow

### Example: User Registration Flow

```
????????????????????????????????????????
?         Request Flow                 ?
????????????????????????????????????????

1. HTTP Request
   ?
   ? API Layer (AuthController)
     - Receives SignupDTO
     - Validates input
   ?
2. Application Layer (IAuthService)
     - Business logic
     - Calls UserRepository
   ?
3. Infrastructure Layer (AuthService + UserRepository)
     - Checks email uniqueness
     - Hashes password (BCrypt)
     - Saves to database
   ?
4. Domain Layer (User Entity)
     - Entity validation
     - Business rules
   ?
5. Infrastructure Layer
     - DbContext.SaveChanges()
   ?
6. Application Layer
     - Generate JWT Token
     - Generate Refresh Token
     - Create AuthResponseDTO
   ?
7. API Layer
     - Return HTTP 200 OK
     - Return response
   ?
   ? HTTP Response (JSON)

????????????????????????????????????????
?      Response Structure              ?
????????????????????????????????????????
{
  "isSuccess": true,
  "message": "User registered successfully",
  "data": {
    "accessToken": "eyJhbGc...",
    "refreshToken": "abc123...",
    "user": { ... }
  }
}
```

---

## SOLID Principles

### 1. Dependency Inversion Principle (DIP)

```csharp
// ? Bad: Direct dependency
public class AuthService
{
    private SqlUserRepository _repo = new SqlUserRepository();
}

// ? Good: Dependency on abstraction
public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;
    
    public AuthService(IUserRepository repo)
    {
        _repo = repo;
    }
}
```

### 2. Single Responsibility Principle (SRP)

Each class has one responsibility:
- `User.cs` ? User entity only
- `AuthService.cs` ? Authentication only
- `UserRepository.cs` ? User data access only

### 3. Open/Closed Principle (OCP)

```csharp
// Open for extension, closed for modification
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class User : BaseEntity  // Extends without modifying
{
    public string Email { get; set; }
}
```

### 4. Interface Segregation Principle (ISP)

```csharp
// ? Fat interface
public interface IService
{
    Task Register();
    Task Login();
    Task SendEmail();
    Task GenerateReport();
}

// ? Segregated interfaces
public interface IAuthService
{
    Task Register();
    Task Login();
}

public interface IEmailService
{
    Task SendEmail();
}
```

---

## Benefits of Architecture

### Dependency Rules

```
??????????????????????????????
?     Dependency Flow        ?
??????????????????????????????

Domain ? Application
  ?           ?
  ?           ?
  ?     Infrastructure
  ?           ?
  ?           ?
  ?????????? API

Rules:
? API ? Infrastructure ? Application ? Domain
? Application ? Domain
? Infrastructure ? Application, Domain
? Domain ? No dependencies
? Application ? Infrastructure
```

### Code Examples:

```csharp
// ? Good - API depends on Application
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService; // From Application
}

// ? Good - Application depends on Domain
public interface IAuthService
{
    Task<User> RegisterAsync(SignupDTO dto); // User from Domain
}

// ? Good - Infrastructure implements Application
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    // Implements IAuthService from Application
}

// ? Good - Domain has no dependencies
public class User // From Domain
{
    // No references to ApplicationDbContext
    // No references to IAuthService
}
```

---

## Why This Matters

### 1. Easy Testing
```csharp
// Can test without database
[Test]
public async Task Register_ShouldCreateUser()
{
    // Arrange
    var mockRepo = new Mock<IUserRepository>();
    var authService = new AuthService(mockRepo.Object);
    
    // Act
    var result = await authService.RegisterAsync(dto);
    
    // Assert
    Assert.NotNull(result);
}
```

### 2. Easy Changes
```csharp
// Switch from SQL Server to MongoDB without changing business logic
public class MongoUserRepository : IUserRepository
{
    // MongoDB implementation
}

// In Startup/Program.cs
services.AddScoped<IUserRepository, MongoUserRepository>(); // Just change this!
```

### 3. Independent Development
- Domain team works independently
- Infrastructure can change anytime
- API can be replaced (e.g., with gRPC)

---

## Package Diagram

```
?????????????????????????????
?      Dependencies         ?
?????????????????????????????

Mentora.Api
  ?
  ??? Mentora.Application
  ?     ?
  ?     ??? Mentora.Domain
  ?
  ??? Mentora.Infrastructure
        ?
        ??? Mentora.Application
        ??? Mentora.Domain
```

---

## Related Documentation

- [02-CLEAN-ARCHITECTURE-GUIDE.md](./02-CLEAN-ARCHITECTURE-GUIDE.md) - Detailed guide
- [03-PROJECT-STRUCTURE.md](./03-PROJECT-STRUCTURE.md) - Project structure
- [04-DESIGN-PATTERNS.md](./04-DESIGN-PATTERNS.md) - Design patterns
- [05-DEPENDENCY-INJECTION.md](./05-DEPENDENCY-INJECTION.md) - Dependency injection

---

**Last Updated:** 2024-12-31  
**Version:** 1.0.0
