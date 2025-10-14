using System.Net;
using System.Text.Json;
using FluentValidation;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.API.Middleware;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        object response;

        var exceptionType = exception.GetType();

        if (exceptionType == typeof(ValidationException))
        {
            statusCode = HttpStatusCode.BadRequest;
            var validationException = (ValidationException)exception;
            var errors = validationException.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            response = new { Title = Constants.ExceptionMessages.ValidationErrorTitle, Status = (int)statusCode, Errors = errors };
        }
        else if (exceptionType == typeof(NotFoundException))
        {
            statusCode = HttpStatusCode.NotFound;
            response = new { Title = Constants.ExceptionMessages.NotFoundTitle, Status = (int)statusCode, Detail = exception.Message };
        }
        else if (exception is DomainException)
        {
            statusCode = HttpStatusCode.Conflict; // 409 Conflict is a good choice for business rule violations
            response = new { Title = Constants.ExceptionMessages.BusinessRuleViolationTitle, Status = (int)statusCode, Detail = exception.Message };
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError;
            response = new { Title = Constants.ExceptionMessages.InternalServerErrorTitle, Status = (int)statusCode, Detail = Constants.ExceptionMessages.UnexpectedErrorDetail };
            _logger.LogError(exception, Constants.ExceptionMessages.UnhandledExceptionLogMessage, exception.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}