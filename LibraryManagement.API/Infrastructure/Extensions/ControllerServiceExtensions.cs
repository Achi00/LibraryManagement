using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManagement.Application.Validators.Book;

namespace LibraryManagement.API.Infrastructure.Extensions
{
    public static class ControllerServiceExtensions
    {
        public static IServiceCollection AddControllersWithValidation(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
                config.Filter = type => type.Name.EndsWith("Request");
            });
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

            return services;
        }
    }
}
