using Mentora.Api.Configuration;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Services;
using Mentora.Domain.Entities.Auth;
using Mentora.Infrastructure.BackgroundServices;
using Mentora.Infrastructure.Data;
using Mentora.Infrastructure.Persistence;
using Mentora.Infrastructure.Repositories;
using Mentora.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration - Support Multiple Providers
var activeDatabase = builder.Configuration["ActiveDatabase"] ?? "Local";
var connectionString = builder.Configuration.GetConnectionString(activeDatabase);

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException($"Connection string '{activeDatabase}' not found. Check appsettings.json");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Log active database
Console.WriteLine($"? Using Database: {activeDatabase}");
Console.WriteLine($"? Connection: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

// 2. Identity Configuration with GUID and BCrypt
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    // Password settings per SRS 1.1.1
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // User settings
    options.User.RequireUniqueEmail = true;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. JWT Authentication per SRS 1.2.1
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero // Remove default 5-minute tolerance
    };
});

// 4. Authorization
builder.Services.AddAuthorization();

// 5. Background Services
builder.Services.AddHostedService<TokenCleanupService>();

// 6. Dependency Injection - Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICareerPlanRepository, CareerPlanRepository>();
builder.Services.AddScoped<ICareerStepRepository, CareerStepRepository>();
builder.Services.AddScoped<ICareerPlanSkillRepository, CareerPlanSkillRepository>();
builder.Services.AddScoped<ICareerQuizRepository, CareerQuizRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserProfileSkillRepository, UserProfileSkillRepository>(); // NEW: Skills Repository
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IPlannerRepository, PlannerRepository>();
builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddScoped<IStudyQuizRepository, StudyQuizRepository>();
builder.Services.AddScoped<IStudySessionsRepository, StudySessionsRepository>();

// 7. Dependency Injection - Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserProfileSkillService, UserProfileSkillService>(); // NEW: Skills Service
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IPlannerService, PlannerService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IStudyQuizService, StudyQuizService>();
builder.Services.AddScoped<IStudySessionsService, StudySessionsService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

// Career Builder Services per SRS
builder.Services.AddScoped<ICareerPlanService, Mentora.Application.Services.CareerPlanService>();

// AI Services - Choose one (Google Gemini recommended)
builder.Services.AddHttpClient(); // Required for AI services
builder.Services.AddScoped<IAiCareerService, Mentora.Application.Services.GeminiAiCareerService>(); // Google Gemini (FREE)
// builder.Services.AddScoped<IAiCareerService, Mentora.Application.Services.OpenAiCareerService>(); // OpenAI (Paid)
// builder.Services.AddScoped<IAiCareerService, Mentora.Application.Services.MockAiCareerService>(); // Mock (Development)

Console.WriteLine("?? AI Service: Google Gemini 2.0 Flash");

// 8. Controllers and API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Support UTF-8 for Arabic text
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });

// 9. Swagger Documentation (Always enabled)
builder.Services.AddSwaggerDocumentation();

// 10. CORS - Updated for port 8000
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:8000",   // Main Vite dev server (NEW PORT)
                "https://localhost:8000",  // HTTPS version
                "http://localhost:5173",   // Alternative Vite port
                "http://localhost:3000",   // Alternative React dev server
                "https://localhost:5173",
                "https://localhost:3000"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Important for cookies/auth headers
    });

    // Keep AllowAll for development/testing
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed Database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Attempting to seed database...");
        await DatabaseSeeder.SeedAsync(services);
        logger.LogInformation("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

// Swagger enabled in ALL environments (Development & Production)
app.UseSwaggerDocumentation();

// Enable static files for custom Swagger CSS
app.UseStaticFiles();

app.UseHttpsRedirection();

// Use the specific CORS policy for frontend
app.UseCors("AllowFrontend");

app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();