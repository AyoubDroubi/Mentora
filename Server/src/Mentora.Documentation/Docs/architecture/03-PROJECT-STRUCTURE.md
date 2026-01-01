# Project Structure - Mentora Platform

## Table of Contents
- [Overview](#overview)
- [Domain Layer](#domain-layer)
- [Application Layer](#application-layer)
- [Infrastructure Layer](#infrastructure-layer)
- [API Layer](#api-layer)
- [Configuration Files](#configuration-files)

---

## Overview

```
Mentora/
??? Server/
?   ??? src/
?   ?   ??? Mentora.Domain/           # Domain Layer
?   ?   ??? Mentora.Application/      # Application Layer
?   ?   ??? Mentora.Infrastructure/   # Infrastructure Layer
?   ?   ??? Mentora.Api/              # API Layer
?   ??? docs/                         # Documentation
?   ??? scripts/                      # Helper scripts
?   ??? README.md
??? Client/                           # React Frontend
??? .git/                             # Git Repository
```

---

## Domain Layer (Mentora.Domain)

### Complete Structure

```
Mentora.Domain/
??? Common/
?   ??? BaseEntity.cs                 # Base entity for all entities
?
??? Entities/
?   ??? Auth/
?   ?   ??? User.cs                   # User entity
?   ?   ?   ??? Id, Email, PasswordHash
?   ?   ?   ??? FirstName, LastName
?   ?   ?   ??? IsEmailConfirmed
?   ?   ?   ??? Navigation: UserProfile, RefreshTokens
?   ?   ?   ??? Navigation: CareerPlans, StudyPlans
?   ?   ?
?   ?   ??? PasswordResetToken.cs    # Password reset tokens
?   ?       ??? Token, UserId
?   ?       ??? ExpiresAt, IsUsed
?   ?       ??? Navigation: User
?   ?
?   ??? Profile/
?   ?   ??? UserProfile.cs           # User profile
?   ?   ?   ??? UserId, Bio, DateOfBirth
?   ?   ?   ??? CurrentRole, TargetRole
?   ?   ?   ??? YearsOfExperience
?   ?   ?   ??? Navigation: User
?   ?   ?   ??? Navigation: Skills, Achievements
?   ?   ?
?   ?   ??? UserSkill.cs             # User skills
?   ?   ?   ??? UserProfileId, SkillId
?   ?   ?   ??? ProficiencyLevel (0-100)
?   ?   ?   ??? Navigation: UserProfile, Skill
?   ?   ?
?   ?   ??? UserAchievement.cs       # User achievements
?   ?       ??? UserProfileId, Title
?   ?       ??? Description, AchievedAt
?   ?       ??? Navigation: UserProfile
?   ?
?   ??? Career/
?   ?   ??? CareerPlan.cs            # Career plan
?   ?   ?   ??? UserId, Title, TargetRole
?   ?   ?   ??? Description, Summary
?   ?   ?   ??? TimelineMonths, CurrentStepIndex
?   ?   ?   ??? IsActive, Status (PlanStatus)
?   ?   ?   ??? Navigation: User
?   ?   ?   ??? Navigation: Steps (List<CareerStep>)
?   ?   ?
?   ?   ??? CareerStep.cs            # Career step
?   ?       ??? CareerPlanId, StepNumber
?   ?       ??? Title, Description
?   ?       ??? EstimatedDuration, Status
?   ?       ??? DueDate, CompletedAt
?   ?       ??? Navigation: CareerPlan
?   ?
?   ??? Study/
?   ?   ??? StudyPlan.cs             # Study plan
?   ?       ??? UserId, Title, Subject
?   ?       ??? StartDate, EndDate
?   ?       ??? IsActive, Status
?   ?       ??? Navigation: User
?   ?
?   ??? Skills/
?   ?   ??? Skill.cs                 # Available skills (Master Data)
?   ?       ??? Name, Category
?   ?       ??? Description
?   ?       ??? Navigation: UserSkills
?   ?
?   ??? RefreshToken.cs              # Refresh tokens
?       ??? Token, UserId
?       ??? DeviceInfo, IpAddress
?       ??? ExpiresAt, IsRevoked
?       ??? Navigation: User
?
??? Enums/
?   ??? Enums.cs                     # All enumerations
?       ??? PlanStatus (Active, Completed, Archived)
?       ??? StepStatus (NotStarted, InProgress, Completed)
?       ??? SkillLevel (Beginner, Intermediate, Advanced)
?       ??? StudyLevel (Freshman, Sophomore, Junior, Senior, Graduate)
?       ??? UserRole (Student, Admin)
?
??? Mentora.Domain.csproj            # Project file
    ??? Target Framework: net9.0
```

### Dependencies

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  
  <ItemGroup>
    <!-- NO external dependencies! -->
    <!-- Domain has zero dependencies -->
  </ItemGroup>
</Project>
```

---

## Application Layer (Mentora.Application)

### Complete Structure

```
Mentora.Application/
??? DTOs/
?   ??? Auth/
?   ?   ??? LoginDTO.cs              # Login request
?   ?   ?   ??? Email
?   ?   ?   ??? Password
?   ?   ?   ??? DeviceInfo (optional)
?   ?   ?
?   ?   ??? SignupDTO.cs             # Registration request
?   ?   ?   ??? FirstName, LastName
?   ?   ?   ??? Email, Password
?   ?   ?   ??? ConfirmPassword
?   ?   ?
?   ?   ??? ForgotPasswordDto.cs     # Password reset request
?   ?   ?   ??? Email
?   ?   ?
?   ?   ??? ResetPasswordDto.cs      # Password reset
?   ?   ?   ??? Token
?   ?   ?   ??? NewPassword
?   ?   ?   ??? ConfirmPassword
?   ?   ?
?   ?   ??? AuthResponseDTO.cs       # Auth response
?   ?       ??? IsSuccess
?   ?       ??? Message
?   ?       ??? AccessToken
?   ?       ??? RefreshToken
?   ?       ??? User (UserDTO)
?   ?
?   ??? User/
?   ?   ??? UserDTO.cs               # User data
?   ?   ??? UserProfileDTO.cs        # Profile data
?   ?   ??? UpdateProfileDTO.cs      # Profile update
?   ?
?   ??? Career/
?   ?   ??? CareerPlanDTO.cs         # Career plan data
?   ?   ??? CareerStepDTO.cs         # Career step data
?   ?   ??? CreateCareerPlanDTO.cs   # Create plan
?   ?   ??? UpdateCareerPlanDTO.cs   # Update plan
?   ?
?   ??? Common/
?       ??? PagedResult.cs           # Pagination helper
?       ??? ApiResponse.cs           # Standard API response
?
??? Interfaces/
?   ??? Services/
?   ?   ??? IAuthService.cs          # Authentication service
?   ?   ?   ??? RegisterAsync()
?   ?   ?   ??? LoginAsync()
?   ?   ?   ??? RefreshTokenAsync()
?   ?   ?   ??? ForgotPasswordAsync()
?   ?   ?   ??? ResetPasswordAsync()
?   ?   ?   ??? LogoutAsync()
?   ?   ?   ??? GetCurrentUserAsync()
?   ?   ?
?   ?   ??? IUserProfileService.cs   # User profile service
?   ?   ?   ??? GetProfileAsync()
?   ?   ?   ??? UpdateProfileAsync()
?   ?   ?   ??? GetCompletionAsync()
?   ?   ?
?   ?   ??? IEmailService.cs         # Email service
?   ?       ??? SendEmailAsync()
?   ?       ??? SendPasswordResetEmailAsync()
?   ?
?   ??? Repositories/
?       ??? IUserRepository.cs       # User repository
?       ??? ICareerPlanRepository.cs # Career plan repository
?       ??? IGenericRepository.cs    # Generic repository
?
??? Mentora.Application.csproj       # Project file
    ??? Target Framework: net9.0
    ??? Project Reference: Mentora.Domain
```

### Dependencies

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\Mentora.Domain\Mentora.Domain.csproj" />
  </ItemGroup>
</Project>
```

---

## Infrastructure Layer (Mentora.Infrastructure)

### Complete Structure

```
Mentora.Infrastructure/
??? Persistence/
?   ??? ApplicationDbContext.cs      # Main DbContext
?   ?   ??? DbSet<User> Users
?   ?   ??? DbSet<UserProfile> UserProfiles
?   ?   ??? DbSet<CareerPlan> CareerPlans
?   ?   ??? DbSet<CareerStep> CareerSteps
?   ?   ??? DbSet<StudyPlan> StudyPlans
?   ?   ??? DbSet<Skill> Skills
?   ?   ??? DbSet<UserSkill> UserSkills
?   ?   ??? DbSet<RefreshToken> RefreshTokens
?   ?   ??? DbSet<PasswordResetToken> PasswordResetTokens
?   ?   ??? OnModelCreating() - Configurations
?   ?
?   ??? Configurations/
?       ??? UserConfiguration.cs     # User entity config
?       ??? CareerPlanConfiguration.cs
?       ??? CareerStepConfiguration.cs
?       ??? RefreshTokenConfiguration.cs
?
??? Data/
?   ??? DatabaseSeeder.cs            # Database seeding
?
??? Repositories/
?   ??? GenericRepository.cs         # Generic repository
?   ?   ??? GetByIdAsync()
?   ?   ??? GetAllAsync()
?   ?   ??? AddAsync()
?   ?   ??? UpdateAsync()
?   ?   ??? DeleteAsync()
?   ?
?   ??? UserRepository.cs            # User repository
?   ?   ??? GetByEmailAsync()
?   ?   ??? EmailExistsAsync()
?   ?   ??? GetWithProfileAsync()
?   ?
?   ??? CareerPlanRepository.cs      # Career plan repository
?       ??? GetUserPlansAsync()
?       ??? GetWithStepsAsync()
?       ??? UpdateStepStatusAsync()
?
??? Services/
?   ??? AuthService.cs               # Authentication service
?   ?   ??? RegisterAsync()
?   ?   ?   ??? 1. Check email uniqueness
?   ?   ?   ??? 2. Hash password (BCrypt)
?   ?   ?   ??? 3. Create user
?   ?   ?   ??? 4. Generate JWT Token
?   ?   ?   ??? 5. Generate Refresh Token
?   ?   ?
?   ?   ??? LoginAsync()
?   ?   ?   ??? 1. Check email exists
?   ?   ?   ??? 2. Verify password (BCrypt)
?   ?   ?   ??? 3. Generate Tokens
?   ?   ?   ??? 4. Save Refresh Token
?   ?   ?
?   ?   ??? RefreshTokenAsync()
?   ?       ??? 1. Validate Token
?   ?       ??? 2. Generate new Tokens
?   ?       ??? 3. Update Refresh Token
?   ?
?   ??? UserProfileService.cs        # Profile service
?   ?   ??? GetProfileAsync()
?   ?   ??? UpdateProfileAsync()
?   ?   ??? GetCompletionAsync()
?   ?
?   ??? TokenService.cs              # Token service
?   ?   ??? GenerateAccessToken()
?   ?   ??? GenerateRefreshToken()
?   ?   ??? ValidateToken()
?   ?
?   ??? TokenCleanupService.cs       # Background service
?       ??? BackgroundService
?       ??? Runs every 24 hours
?       ??? Removes expired Refresh Tokens
?
??? Migrations/
?   ??? 20241231_InitialCreate.cs
?   ??? 20260101_UserProfile.cs
?   ??? ApplicationDbContextModelSnapshot.cs
?
??? Mentora.Infrastructure.csproj    # Project file
    ??? Target Framework: net9.0
    ??? Project References: Domain, Application
```

### Dependencies

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\Mentora.Domain\Mentora.Domain.csproj" />
    <ProjectReference Include="..\Mentora.Application\Mentora.Application.csproj" />
  </ItemGroup>
  
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.0" />
    <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.0" />
  </ItemGroup>
</Project>
```

---

## API Layer (Mentora.Api)

### Complete Structure

```
Mentora.Api/
??? Controllers/
?   ??? AuthController.cs            # Authentication endpoints
?   ?   ??? POST /api/auth/register
?   ?   ??? POST /api/auth/login
?   ?   ??? POST /api/auth/refresh-token
?   ?   ??? POST /api/auth/logout
?   ?   ??? POST /api/auth/logout-all
?   ?   ??? POST /api/auth/forgot-password
?   ?   ??? POST /api/auth/reset-password
?   ?   ??? GET /api/auth/me [Authorize]
?   ?
?   ??? UserProfileController.cs     # Profile endpoints
?   ?   ??? GET /api/userprofile
?   ?   ??? PUT /api/userprofile
?   ?   ??? GET /api/userprofile/exists
?   ?   ??? GET /api/userprofile/completion
?   ?   ??? GET /api/userprofile/timezones
?   ?
?   ??? BaseController.cs            # Base controller
?
??? Configuration/
?   ??? SwaggerExtensions.cs         # Swagger configuration
?
??? Tests/
?   ??? auth-tests.http              # Auth HTTP tests
?   ?   ??? Register Request
?   ?   ??? Login Request
?   ?   ??? Refresh Token Request
?   ?   ??? Forgot Password Request
?   ?   ??? Reset Password Request
?   ?
?   ??? userprofile-tests.http       # Profile HTTP tests
?
??? Program.cs                       # Application entry point
?   ??? 1. Builder Configuration
?   ?   ??? Add DbContext
?   ?   ??? Add Authentication (JWT)
?   ?   ??? Add Swagger
?   ?   ??? Add CORS
?   ?   ??? Register Services (DI)
?   ?
?   ??? 2. Middleware Pipeline
?   ?   ??? UseSwagger()
?   ?   ??? UseHttpsRedirection()
?   ?   ??? UseCors()
?   ?   ??? UseAuthentication()
?   ?   ??? UseAuthorization()
?   ?   ??? MapControllers()
?   ?
?   ??? 3. Run Application
?
??? appsettings.json                 # Main configuration
?   ??? ActiveDatabase               # Active DB provider
?   ??? ConnectionStrings
?   ?   ??? Local
?   ?   ??? RemoteServer
?   ?   ??? Azure
?   ?   ??? Docker
?   ?   ??? Development
?   ?   ??? Production
?   ??? Jwt
?   ?   ??? Key
?   ?   ??? Issuer
?   ?   ??? Audience
?   ??? Logging
?
??? appsettings.Development.json     # Development config
?
??? launchSettings.json              # Launch settings
?   ??? https: localhost:7000
?   ??? http: localhost:5000
?
??? Mentora.Api.csproj               # Project file
    ??? Target Framework: net9.0
    ??? Project References: Application, Infrastructure
```

### Dependencies

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\Mentora.Application\Mentora.Application.csproj" />
    <ProjectReference Include="..\Mentora.Infrastructure\Mentora.Infrastructure.csproj" />
  </ItemGroup>
  
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
  </ItemGroup>
</Project>
```

---

## Configuration Files

### 1. appsettings.json

```json
{
  "ActiveDatabase": "Local",
  
  "ConnectionStrings": {
    "Local": "Server=.;Database=MentoraDb;User ID=sa;Password=YOUR_PASSWORD;...",
    "RemoteServer": "Server=REMOTE_IP;Database=MentoraDb;...",
    "Azure": "Server=*.database.windows.net;Database=MentoraDb;...",
    "Docker": "Server=localhost,1433;Database=MentoraDb;...",
    "Development": "Server=.;Database=MentoraDb_Dev;...",
    "Production": "Server=PROD_SERVER;Database=MentoraDb_Prod;..."
  },
  
  "Jwt": {
    "Key": "super_secret_key_123456789_must_be_long",
    "Issuer": "MentoraApi",
    "Audience": "MentoraClient"
  },
  
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  
  "AllowedHosts": "*"
}
```

### 2. launchSettings.json

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7000;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

---

## Project Statistics

### Entity Count by Layer

| Layer | Entities | DTOs | Services | Controllers |
|-------|----------|------|----------|-------------|
| Domain | 12 | 0 | 0 | 0 |
| Application | 0 | 15+ | 0 (interfaces only) | 0 |
| Infrastructure | 0 | 0 | 6 | 0 |
| API | 0 | 0 | 0 | 3 |

### Lines of Code (Approximate)

| Layer | Lines of Code |
|-------|---------------|
| Domain | ~800 |
| Application | ~500 |
| Infrastructure | ~1,500 |
| API | ~600 |
| **Total** | **~3,400** |

---

## Naming Conventions

### Entities
```
- PascalCase
- Singular (User, CareerPlan)
- No "Entity" suffix (? User, ? UserEntity)
```

### DTOs
```
- PascalCase
- End with DTO (LoginDTO, SignupDTO)
- Descriptive (CreateCareerPlanDTO, UpdateUserProfileDTO)
```

### Services
```
- PascalCase
- End with Service (AuthService, EmailService)
- Interface prefix I (IAuthService)
```

### Repositories
```
- PascalCase
- End with Repository (UserRepository)
- Interface prefix I (IUserRepository)
```

### Controllers
```
- PascalCase
- End with Controller (AuthController)
- Route matches name (api/auth)
```

---

## Related Documentation

- [01-ARCHITECTURE-OVERVIEW.md](./01-ARCHITECTURE-OVERVIEW.md) - Architecture overview
- [../domain/01-DOMAIN-OVERVIEW.md](../domain/01-DOMAIN-OVERVIEW.md) - Domain layer details
- [../application/01-APPLICATION-OVERVIEW.md](../application/01-APPLICATION-OVERVIEW.md) - Application layer details
- [DATABASE-CONFIGURATION.md](../../../DATABASE-CONFIGURATION.md) - Database setup

---

**Last Updated**: 2024-12-31  
**Version**: 1.0.0
