using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.Services;

namespace LibraryManagement.API.Infrastructure.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPatronService, PatronService>();
            services.AddScoped<IBorrowRecordService, BorrowRecordService>();

            return services;
        }
    }
}
