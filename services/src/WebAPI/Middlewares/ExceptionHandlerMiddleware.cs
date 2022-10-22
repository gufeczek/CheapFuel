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
            ValidationException validationException => HandleValidationException(validationException),
            _ => HandleUnexpectedException(e)
        };

        context.Response.StatusCode = (int)errorMessage.StatusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage, _jsonSerializerSettings));
    }

    private static ErrorMessage HandleBadRequestException(BadRequestException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.BadRequest,
            Title: "Bad request",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private static ErrorMessage HandleNotFoundException(NotFoundException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.NotFound,
            Title: "The specified resource was not found.",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private static ErrorMessage HandleConflictException(ConflictException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.Conflict,
            Title: "Conflict",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private static ErrorMessage HandleUnauthorizedException(UnauthorizedException e)
    {
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.Unauthorized,
            Title: "Unauthorized",
            Details: e.Message,
            Timestamp: DateTime.UtcNow
        );
    }

    private static ErrorMessage HandleValidationException(ValidationException e)
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

    private static ErrorMessage HandleUnexpectedException(Exception e)
    {
        Console.WriteLine(e.StackTrace); // Should be change to logger
        
        return new ErrorMessage
        (
            StatusCode: HttpStatusCode.InternalServerError,
            Title: "Internal server error",
            Details: "Something went wrong :(",
            Timestamp: DateTime.UtcNow
        );
    }
}