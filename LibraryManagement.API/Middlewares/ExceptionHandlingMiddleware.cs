using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Entity;
using LibraryManagement.Persistence.Context;
using System.Text.Json;

namespace LibraryManagement.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;


        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IServiceScopeFactory scopeFactory,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                AlreadyExistsException => StatusCodes.Status409Conflict,
                BusinessRuleViolationException => StatusCodes.Status409Conflict,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            // log to database
            await LogErrorToDatabase(context, ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                message = ex.Message,
                statusCode
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }

        private async Task LogErrorToDatabase(HttpContext context, Exception ex)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<LibraryManagementContext>();

                var error = new ErrorLog(
                    ex.Message,
                    ex.StackTrace?.Substring(0, Math.Min(ex.StackTrace.Length, 4000)) ?? "",
                    ex.InnerException?.Message,
                    ex.InnerException?.StackTrace?.Substring(0, Math.Min(ex.InnerException.StackTrace.Length, 4000)) ?? "",
                    context.Request.Path,
                    context.Request.Method
                );
                //ex.
                dbContext.ErrorLogs.Add(error);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                // prevent logging failure from crashing the app, just in case something wrong happends here too
                _logger.LogError(logEx, "Failed to log exception to database");
            }
        }
    }
}
