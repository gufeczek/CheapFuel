using System.Net;
using System.Net.Mime;
using Application.Common.Exceptions;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebAPI.Common.Json;
using WebAPI.Models;

namespace WebAPI.Middlewares;

public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    
    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
        
    }
    
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        ContractResolver = new BaseFieldsFirstContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.None,
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        NullValueHandling = NullValueHandling.Ignore
    };
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception e)
    {
        var errorMessage = e switch
        {
            BadRequestException badRequestException => HandleBadRequestException(badRequestException),
            NotFoundException notFoundException => HandleNotFoundException(notFoundException),
            ConflictException conflictException => HandleConflictException(conflictException),
            UnauthorizedException unauthorizedException => HandleUnauthorizedException(unauthorizedException),
            ForbiddenException forbiddenException => HandleForbiddenException(forbiddenException),
            ValidationException validationException => HandleValidationException(validationException),
            FilterValidationException filterValidationException => HandleFilterValidationException(filterValidationException),
            _ => HandleUnexpectedException(e)
        };

        context.Response.StatusCode = (int)errorMessage.StatusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage, _jsonSerializerSettings));
    }

    private ErrorMessage HandleBadRequestException(BadRequestException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.BadRequest,
            Title: "Bad request",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private ErrorMessage HandleNotFoundException(NotFoundException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.NotFound,
            Title: "The specified resource was not found.",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private ErrorMessage HandleConflictException(ConflictException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.Conflict,
            Title: "Conflict",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private ErrorMessage HandleUnauthorizedException(UnauthorizedException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.Unauthorized,
            Title: "Unauthorized",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private ErrorMessage HandleForbiddenException(ForbiddenException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.Forbidden,
            Title: "Forbidden",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private ErrorMessage HandleValidationException(ValidationException e)
    {
        var failures = e.Errors
            .GroupBy(
                v => v.PropertyName,
                v => v.ErrorMessage,
                (propertyName, errorMessage) => new PropertyValidationFailure
                (
                    Property: propertyName,
                    Errors: errorMessage.Distinct().ToArray()
                ))
            .ToArray();

        return new ValidationErrorMessage
        (
            StatusCode: HttpStatusCode.BadRequest,
            Title: "Bad request",
            Details: "Validation failed",
            Timestamp: DateTime.UtcNow,
            Violations: failures
        );
    }

    private ErrorMessage HandleFilterValidationException(FilterValidationException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.BadRequest,
            Title: "Bad request",
            Details: string.Join("\n", e.ValidationErrors),
            Timestamp: DateTime.UtcNow
        );
    }

    private ErrorMessage HandleUnexpectedException(Exception e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
        
        _logger.LogError(e.Message);
        _logger.LogError(e.StackTrace);

        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.InternalServerError,
            Title: "Internal server error",
            Details: "Something went wrong :(",
            Timestamp: DateTime.UtcNow
        );
    }
}