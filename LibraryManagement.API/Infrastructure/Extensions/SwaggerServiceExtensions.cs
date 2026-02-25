using LibraryManagement.Api.Infrastructure.Swagger;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace LibraryManagement.API.Infrastructure.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Library Management API",
                    Version = "v1",
                    Description = "REST API for managing books, authors, patrons, and borrow records"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);

                ConfigureDateTimeSchemas(options);
                ConfigureXmlComments(options);
                ConfigureSecurityDefinition(options);
                ConfigureTagGrouping(options);
            });

            return services;
        }

        private static void ConfigureDateTimeSchemas(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            // Register DateTime schema filter for proper documentation
            options.SchemaFilter<DateTimeSchemaFilter>();

            // Map DateTime types explicitly
            options.MapType<DateTime>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date-time",
                Example = new OpenApiString("2026-04-15T09:00:00Z"),
                Description = "UTC datetime in ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ)"
            });

            options.MapType<DateTime?>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date-time",
                Nullable = true,
                Example = new OpenApiString("2026-04-15T09:00:00Z"),
                Description = "UTC datetime in ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ) or null"
            });
        }

        private static void ConfigureXmlComments(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            // Include XML comments from API project
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        }

        private static void ConfigureSecurityDefinition(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            // JWT Bearer authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = """
                JWT Authorization header using the Bearer scheme.
                
                Enter your token in the text input below.
                
                Example: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
                """
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
        }

        private static void ConfigureTagGrouping(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            // Group endpoints by controller
            options.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return [api.GroupName];
                }

                if (api.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
                {
                    return [controllerActionDescriptor.ControllerName];
                }

                return ["Other"];
            });

            options.DocInclusionPredicate((name, api) => true);
        }
    }
}
