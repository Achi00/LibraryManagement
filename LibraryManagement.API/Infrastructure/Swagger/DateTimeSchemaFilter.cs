using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LibraryManagement.Api.Infrastructure.Swagger;

public class DateTimeSchemaFilter : ISchemaFilter
{
    private const string UtcExample = "2026-04-15T09:00:00Z";
    private const string UtcDescription = "UTC datetime in ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ). Must end with 'Z' to indicate UTC.";

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(DateTime) || context.Type == typeof(DateTime?))
        {
            schema.Type = "string";
            schema.Format = "date-time";
            schema.Example = new OpenApiString(UtcExample);
            schema.Description = UtcDescription;
        }

        if (schema.Properties == null) return;

        foreach (var property in schema.Properties)
        {
            var propertyInfo = context.Type.GetProperty(
                property.Key,
                System.Reflection.BindingFlags.IgnoreCase |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);

            if (propertyInfo == null) continue;

            var propertyType = propertyInfo.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (underlyingType == typeof(DateTime))
            {
                property.Value.Type = "string";
                property.Value.Format = "date-time";
                property.Value.Example = new OpenApiString(UtcExample);
                property.Value.Description = string.IsNullOrEmpty(property.Value.Description)
                    ? UtcDescription
                    : $"{property.Value.Description} ({UtcDescription})";
            }
        }
    }
}
