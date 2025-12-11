using DirectoryService.Application.Exceptions;
using Shared;

namespace DirectoryService.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);
            
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        int statusCode;
        Errors errorList;

        switch (exception)
        {
            case BadRequestException:
                statusCode = StatusCodes.Status400BadRequest;

                errorList = new Errors(new[] 
                {
                    Error.Failure(new[] { new ErrorMessage("bad_request", exception.Message, null) })
                });
                break;

            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                errorList = new Errors(new[] 
                {
                    Error.NotFound(new[] { new ErrorMessage("not_found", 
                        exception.Message, null) })
                });
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                errorList = new Errors(new[] 
                {
                    Error.Failure(new[] { new ErrorMessage("internal_error", "Internal server error", null) })
                });
                break;
        }

        var envelope = Envelope.Error(errorList);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(envelope);
    }

}