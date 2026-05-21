using ComputerStore.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace ComputerStore.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errors) = exception switch
        {
            NotFoundException nfe => (HttpStatusCode.NotFound, new[] { nfe.Message }),
            ValidationException ve => (HttpStatusCode.BadRequest, ve.Errors.ToArray()),
            InsufficientStockException ise => (HttpStatusCode.UnprocessableEntity, new[] { ise.Message }),
            _ => (HttpStatusCode.InternalServerError, new[] { "An unexpected error occurred. Please try again later." })
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}