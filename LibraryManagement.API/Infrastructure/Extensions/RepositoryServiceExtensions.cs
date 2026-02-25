using LibraryManagement.Application;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Authors;
using LibraryManagement.Infrastructure.Books;
using LibraryManagement.Infrastructure.BorrowRecords;
using LibraryManagement.Infrastructure.Patrons;
using LibraryManagement.Persistence;

namespace LibraryManagement.API.Infrastructure.Extensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPatronRepository, PatronRepository>();
            services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
