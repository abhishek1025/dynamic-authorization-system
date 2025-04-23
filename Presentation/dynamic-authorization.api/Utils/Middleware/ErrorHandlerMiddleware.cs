using dynamic_authorization.domain.Exceptions;

namespace dynamic_authorization.api.Utils.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(
                new ApiErrorResponse(){ Success = false, Message = "Unauthorized access." });
        }
        catch (Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;
            string message = exception.Message ?? "An unexpected error occurred";
            
            if (exception is BadHttpRequestException) 
            {
                statusCode = StatusCodes.Status400BadRequest;
            }

            if (exception is NotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
            }
            
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(
                new ApiErrorResponse() { Success = false, Message = message, StatusCode = statusCode});
        }
        
    }
}