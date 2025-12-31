using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Mentora.Api.Configuration
{
    /// <summary>
    /// Swagger/OpenAPI configuration extensions
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Add and configure Swagger services
        /// </summary>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // API Information
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Mentora API",
                    Version = "v1",
                    Description = @"
**Mentora** - AI-powered platform for academic excellence and career success

## Features
- ?? **Authentication**: JWT-based auth with refresh tokens
- ?? **Career Builder**: AI-powered career planning
- ?? **Study Scheduler**: Intelligent study planning
- ?? **AI Integration**: Smart recommendations

## Getting Started
1. Register a new account using `/api/auth/register`
2. Login using `/api/auth/login` to get your access token
3. Click the 'Authorize' button and enter: `Bearer {your-token}`
4. Start exploring the API!

## Documentation
- [Quick Start Guide](https://github.com/AyoubDroubi/Mentora)
- [Database Setup](docs/DATABASE-SETUP-SUMMARY.md)
- [API Documentation](docs/MODULE-1-AUTHENTICATION.md)
",
                    Contact = new OpenApiContact
                    {
                        Name = "Mentora Team",
                        Email = "support@mentora.com",
                        Url = new Uri("https://github.com/AyoubDroubi/Mentora")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // JWT Authentication
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                    
**Enter 'Bearer' [space] and then your token in the text input below.**

Example: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`

You can get a token by calling the `/api/auth/login` endpoint.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // XML Comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                // Enable annotations
                options.EnableAnnotations();

                // Order actions by HTTP method
                options.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            });

            return services;
        }

        /// <summary>
        /// Configure Swagger UI middleware
        /// </summary>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mentora API v1");
                options.RoutePrefix = string.Empty; // Swagger UI at root
                options.DocumentTitle = "Mentora API - Documentation";
                
                // UI Customization
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
                options.ShowExtensions();
                options.EnableValidator();
                options.EnableTryItOutByDefault();
                
                // Persistence
                options.DisplayOperationId();
                options.DefaultModelsExpandDepth(2);
                options.DefaultModelExpandDepth(2);
                
                // Custom CSS
                options.InjectStylesheet("/swagger-ui/custom.css");
            });

            return app;
        }
    }
}
