using LibraryManagement.API.Middlewares;
using LibraryManagement.Persistence;
using LibraryManagement.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.API.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
                });
            }

            app.UseHttpsRedirection();

            //app.UseCors(CorsServiceExtensions.AllowAllPolicyName);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<LibraryManagementContext>();

                await SeedData.SeedAsync(context);
            }

            return app;
        }
    }
}
