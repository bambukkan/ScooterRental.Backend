using RentalSystem.Core.Exceptions;
namespace RentalSystem.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next,
     ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex) 
        {
            _logger.LogWarning(ex, "Нарушение бизнес-правил: {Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = ex.GetType().Name, 
                Message = ex.Message
            });
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, "Нарушение бизнес-правил: {Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = ex.GetType().Name, 
                Message = ex.Message
            });
        }
        catch(Exception exception)
        {
            _logger.LogError(exception,"Internal server error: {Message}", exception.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                Error = "Internal Server Error",
                Message = "Что то пошло не так" 
            });
        }
    }
}