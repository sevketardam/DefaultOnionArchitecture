using System.Text.Json;
using FluentValidation;
using DefaultOnionArchitecture.Application.Exceptions;
using DefaultOnionArchitecture.Application.Interface.ExceptionLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public ExceptionLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _serviceProvider);
        }
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
    private static bool IsApiRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/api") ||
               context.Request.Headers["Accept"].ToString().Contains("application/json");
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, IServiceProvider serviceProvider)
    {
        int statusCode = GetStatusCode(exception);
        string errorCode = string.Empty;

        if (exception is ValidationException validationEx)
        {
            var errorModel = new ExceptionModel
            {
                errors = validationEx.Errors.Select(x => x.ErrorMessage),
                statusCode = statusCode,
                isValid = true
            };

            if (IsApiRequest(context))
            {
                await WriteJsonResponse(context, statusCode, errorModel);
            }
            else
            {
                var codeParam = "validation-error";
                context.Response.Redirect($"/error?code={codeParam}&status={statusCode}");
            }
            return;
        }

        if (exception is ApplicationException appEx)
        {
            var errorModel = new ExceptionModel
            {
                errors = new List<string> { appEx.Message },
                statusCode = statusCode,
                isValid = true
            };

            if (IsApiRequest(context))
            {
                await WriteJsonResponse(context, statusCode, errorModel);
            }
            else
            {
                var codeParam = "application-error";
                context.Response.Redirect($"/error?code={codeParam}&status={statusCode}");
            }
        }


        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<IExceptionLogger>();
        errorCode = await logger.LogAsync(context, exception);

        if (IsApiRequest(context))
        {
            var errorModel = new ExceptionModel
            {
                errors = new List<string> { $"Bilinmeyen bir hata oluştu. Hata Kodu: {errorCode}" },
                statusCode = statusCode,
                isValid = false
            };

            await WriteJsonResponse(context, statusCode, errorModel);
        }
        else
        {
            context.Response.Redirect($"/error?code={errorCode}&status={statusCode}");
        }
    }

    private static async Task WriteJsonResponse(HttpContext context, int statusCode, object model)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(model));
    }
}
