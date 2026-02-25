
using LibraryManagement.API.Infrastructure.Extensions;
using Serilog;

namespace LibraryManagement.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/log-.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();


            // Add services to the container.
            builder.Services
                .AddDatabase(builder.Configuration)
                .AddControllersWithValidation()
                .AddServices()
                .AddMapster()
                .AddSwagger();

            var app = builder.Build();

            app.UseGlobalExceptionHandler();
            app.UseRequestLogger();
            app.ConfigureMiddleware();

            await app.SeedDatabaseAsync();

            app.Run();
        }
    }
}
