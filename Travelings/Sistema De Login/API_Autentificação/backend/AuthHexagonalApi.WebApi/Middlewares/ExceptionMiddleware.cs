using System.Net;
using System.Text.Json;
using AuthHexagonalApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AuthHexagonalApi.WebApi.Middlewares;

public sealed class ExceptionMiddleware
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (statusCode, response) = exception switch
        {
            ValidationException validation => (
                HttpStatusCode.BadRequest,
                new ErrorResponse(
                    validation.Message,
                    validation.Errors,
                    (int)HttpStatusCode.BadRequest)),

            DomainException domain when domain.ErrorCode == "email_already_exists" => (
                HttpStatusCode.Conflict,
                new ErrorResponse(
                    domain.Message,
                    null,
                    (int)HttpStatusCode.Conflict)),

            DomainException domain when domain.ErrorCode == "username_already_exists" => (
                HttpStatusCode.Conflict,
                new ErrorResponse(
                    domain.Message,
                    null,
                    (int)HttpStatusCode.Conflict)),

            DomainException domain when domain.ErrorCode == "invalid_credentials" => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse(
                    domain.Message,
                    null,
                    (int)HttpStatusCode.Unauthorized)),

            DomainException domain when domain.ErrorCode == "invalid_refresh_token" => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse(
                    domain.Message,
                    null,
                    (int)HttpStatusCode.Unauthorized)),

            DomainException domain when domain.ErrorCode == "user_not_found" => (
                HttpStatusCode.NotFound,
                new ErrorResponse(
                    domain.Message,
                    null,
                    (int)HttpStatusCode.NotFound)),

            DomainException domain => (
                HttpStatusCode.BadRequest,
                new ErrorResponse(
                    domain.Message,
                    null,
                    (int)HttpStatusCode.BadRequest)),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse(
                    "Ocorreu um erro interno.",
                    null,
                    (int)HttpStatusCode.InternalServerError))
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}

public sealed record ErrorResponse(string Message, IReadOnlyDictionary<string, string[]>? Errors, int StatusCode);
