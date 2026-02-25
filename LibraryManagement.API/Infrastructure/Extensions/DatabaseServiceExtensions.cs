using LibraryManagement.Persistence;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Infrastructure.Extensions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<LibraryManagementContext>(options => options.UseSqlServer(
                configuration.GetConnectionString(nameof(ConnectionString.DefaultConnection))
                ));

            return services;
        }
    }
}
