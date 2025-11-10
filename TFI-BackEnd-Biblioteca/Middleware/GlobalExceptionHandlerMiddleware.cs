using System.Net;
using System.Text.Json;

namespace TFI_BackEnd_Biblioteca.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
      private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

  public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
        catch (Exception ex)
      {
            _logger.LogError(ex, "Ocurrió una excepción no controlada: {Message}", ex.Message);
         await HandleExceptionAsync(context, ex);
       }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
   context.Response.ContentType = "application/json";
            
     var response = new
            {
   message = "Ocurrió un error en el servidor",
          details = exception.Message,
            timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = exception switch
        {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
    ArgumentException => (int)HttpStatusCode.BadRequest,
        KeyNotFoundException => (int)HttpStatusCode.NotFound,
   UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
        _ => (int)HttpStatusCode.InternalServerError
    };

     var jsonResponse = JsonSerializer.Serialize(response);
    return context.Response.WriteAsync(jsonResponse);
  }
    }

    // Extensión para facilitar el uso del middleware
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
   return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
