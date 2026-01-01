# Application Layer Overview - Mentora Platform

## Table of Contents
- [Overview](#overview)
- [Interfaces](#interfaces)
- [DTOs](#dtos)
- [Validation](#validation)
- [Design Patterns](#design-patterns)
- [Dependencies](#dependencies)

---

## Overview

The Application Layer is the **bridge** between API and Domain:

```
?????????????????????????????
?  Application Layer Role   ?
?????????????????????????????
?                           ?
?  API Layer (Controllers)  ?
?         ?                 ?
?  Application (Use Cases)  ?
?         ?                 ?
?  Domain (Business Logic)  ?
?                           ?
?????????????????????????????
```

### Key Responsibilities

- Define use cases and business flows
- Provide interfaces (contracts)
- Define DTOs for data transfer
- Define validation rules
- NO knowledge of external services (DB, API, etc.)
- NO dependency on Infrastructure
- NO dependency on API

---

## Interfaces

### 1. Use Cases (Service Interfaces)

```csharp
public interface IAuthService
{
    // Use Case: Register new user
    Task<AuthResponseDTO> RegisterAsync(SignupDTO dto);
    
    // Use Case: User login
    Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
    
    // Use Case: Refresh token
    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
}
```

### 2. Data Transfer Objects (DTOs)

```csharp
// DTO for incoming data from API
public class SignupDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// DTO for outgoing data to API
public class AuthResponseDTO
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public UserDTO? User { get; set; }
}
```

### 3. Abstractions (Interfaces)

```csharp
// Repository interface
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(User user);
}

// External service interface
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
```

---

## DTOs (Data Transfer Objects)

### DTOs Structure

```
Mentora.Application/DTOs/
??? Auth/
?   ??? LoginDTO.cs
?   ??? SignupDTO.cs
?   ??? ForgotPasswordDto.cs
?   ??? ResetPasswordDto.cs
?   ??? AuthResponseDTO.cs
??? User/
?   ??? UserDTO.cs
?   ??? UserProfileDTO.cs
?   ??? UpdateProfileDTO.cs
??? Career/
?   ??? CareerPlanDTO.cs
?   ??? CareerStepDTO.cs
?   ??? CreateCareerPlanDTO.cs
?   ??? UpdateCareerPlanDTO.cs
??? Common/
    ??? PagedResult.cs
    ??? ApiResponse.cs
```

### Detailed Examples

#### 1. Auth DTOs

```csharp
// DTOs/Auth/LoginDTO.cs
public class LoginDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;
    
    public string? DeviceInfo { get; set; }
}

// DTOs/Auth/SignupDTO.cs
public class SignupDTO
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(8)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*]).*$", 
        ErrorMessage = "Password must contain uppercase and special character")]
    public string Password { get; set; } = string.Empty;
    
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

// DTOs/Auth/AuthResponseDTO.cs
public class AuthResponseDTO
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public UserDTO? User { get; set; }
    public DateTime? TokenExpiresAt { get; set; }
}
```

#### 2. User DTOs

```csharp
// DTOs/User/UserDTO.cs
public class UserDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}

// DTOs/User/UserProfileDTO.cs
public class UserProfileDTO
{
    public Guid UserId { get; set; }
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CurrentRole { get; set; }
    public string? TargetRole { get; set; }
    public int? YearsOfExperience { get; set; }
    public List<UserSkillDTO> Skills { get; set; } = new();
    public List<UserAchievementDTO> Achievements { get; set; } = new();
}
```

#### 3. Career Plan DTOs

```csharp
// DTOs/Career/CreateCareerPlanDTO.cs
public class CreateCareerPlanDTO
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string TargetRole { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Range(1, 60, ErrorMessage = "Timeline must be between 1 and 60 months")]
    public int TimelineMonths { get; set; } = 12;
}

// DTOs/Career/CareerPlanDTO.cs
public class CareerPlanDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TargetRole { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int TimelineMonths { get; set; }
    public int CurrentStepIndex { get; set; }
    public bool IsActive { get; set; }
    public PlanStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CareerStepDTO> Steps { get; set; } = new();
    public int ProgressPercentage { get; set; }
}
```

#### 4. Common DTOs

```csharp
// DTOs/Common/PagedResult.cs
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}

// DTOs/Common/ApiResponse.cs
public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public static ApiResponse<T> Success(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }
    
    public static ApiResponse<T> Failure(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
```

---

## Service Interfaces

### Interface Structure

```
Mentora.Application/Interfaces/
??? Services/
?   ??? IAuthService.cs
?   ??? IUserProfileService.cs
?   ??? IEmailService.cs
?   ??? IAiService.cs
??? Repositories/
    ??? IGenericRepository.cs
    ??? IUserRepository.cs
    ??? ICareerPlanRepository.cs
```

### Detailed Examples

#### 1. Auth Service

```csharp
// Interfaces/Services/IAuthService.cs
public interface IAuthService
{
    /// <summary>
    /// Register new user
    /// </summary>
    Task<AuthResponseDTO> RegisterAsync(SignupDTO dto);
    
    /// <summary>
    /// User login
    /// </summary>
    Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
    
    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
    
    /// <summary>
    /// Send password reset email
    /// </summary>
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
    
    /// <summary>
    /// Reset password with token
    /// </summary>
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    
    /// <summary>
    /// Logout from current device
    /// </summary>
    Task<bool> LogoutAsync(Guid userId, string refreshToken);
    
    /// <summary>
    /// Logout from all devices
    /// </summary>
    Task<bool> LogoutAllAsync(Guid userId);
    
    /// <summary>
    /// Get current user info
    /// </summary>
    Task<UserDTO?> GetCurrentUserAsync(Guid userId);
}
```

#### 2. Career Service

```csharp
// Interfaces/Services/ICareerService.cs
public interface ICareerService
{
    Task<CareerPlanDTO> CreatePlanAsync(Guid userId, CreateCareerPlanDTO dto);
    Task<CareerPlanDTO?> GetPlanAsync(Guid planId, Guid userId);
    Task<List<CareerPlanDTO>> GetUserPlansAsync(Guid userId);
    Task<CareerPlanDTO> UpdatePlanAsync(Guid planId, Guid userId, UpdateCareerPlanDTO dto);
    Task<bool> DeletePlanAsync(Guid planId, Guid userId);
    Task<bool> ActivatePlanAsync(Guid planId, Guid userId);
    Task<bool> CompletePlanAsync(Guid planId, Guid userId);
    Task<CareerStepDTO> UpdateStepStatusAsync(Guid stepId, Guid userId, CareerStepStatus status);
}
```

#### 3. Generic Repository

```csharp
// Interfaces/Repositories/IGenericRepository.cs
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
```

#### 4. User Repository

```csharp
// Interfaces/Repositories/IUserRepository.cs
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetWithProfileAsync(Guid userId);
    Task<User?> GetWithCareerPlansAsync(Guid userId);
}
```

---

## Validation

### Using Data Annotations

```csharp
public class SignupDTO
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*]).*$")]
    public string Password { get; set; } = string.Empty;
    
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
```

### Using FluentValidation (Optional - Future)

```csharp
// Validators/SignupDTOValidator.cs
public class SignupDTOValidator : AbstractValidator<SignupDTO>
{
    public SignupDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name too long")
            .MinimumLength(2).WithMessage("First name too short");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password too short")
            .Matches(@"[A-Z]").WithMessage("Must contain uppercase")
            .Matches(@"[!@#$%^&*]").WithMessage("Must contain special char");
            
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}
```

---

## Design Patterns

### 1. Repository Pattern

```csharp
// Application defines the interface
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
}

// Infrastructure implements it
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    
    public async Task<UserDTO?> GetUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return MapToDTO(user);
    }
}
```

### 2. Service Pattern

```csharp
// Each use case gets its own service
public interface IAuthService { }
public interface ICareerService { }
public interface IEmailService { }
```

### 3. DTO Pattern

```csharp
// Separation between Domain Model and Data Transfer
// Domain Model (in Domain Layer)
public class User
{
    public string PasswordHash { get; set; } // Internal
}

// DTO (in Application Layer)
public class UserDTO
{
    // NO PasswordHash exposed (security)
    public string Email { get; set; }
}
```

### 4. CQRS (Future - Command Query Responsibility Segregation)

```csharp
// Commands (write operations)
public interface ICommand<TResult>
{
    Task<TResult> ExecuteAsync();
}

// Queries (read operations)
public interface IQuery<TResult>
{
    Task<TResult> ExecuteAsync();
}

// Example
public class CreateCareerPlanCommand : ICommand<CareerPlanDTO>
{
    public CreateCareerPlanDTO Data { get; set; }
}
```

---

## Dependencies

```
??????????????????????????????
?  Application Dependencies  ?
??????????????????????????????

Application
    ??? Depends on:
    ?   ??? Domain ?
    ?
    ??? Used by:
        ??? Infrastructure ?
        ??? API ?

? Does NOT depend on:
    - Infrastructure
    - API
    - External Libraries (except standard .NET)
```

---

## Related Documentation

- [../architecture/01-ARCHITECTURE-OVERVIEW.md](../architecture/01-ARCHITECTURE-OVERVIEW.md)
- [../domain/01-DOMAIN-OVERVIEW.md](../domain/01-DOMAIN-OVERVIEW.md)
- [02-SERVICES-GUIDE.md](./02-SERVICES-GUIDE.md)
- [03-DTOS-REFERENCE.md](./03-DTOS-REFERENCE.md)

---

**Last Updated**: 2024-12-31  
**Version**: 1.0.0
