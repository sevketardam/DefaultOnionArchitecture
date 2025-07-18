﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;

namespace DefaultOnionArchitecture.Application.Exceptions;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        int statusCode = GetStatusCode(exception);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        if (exception.GetType() == typeof(ValidationException) || exception.GetType() == typeof(ApplicationException))
            return httpContext.Response.WriteAsync(new ExceptionModel
            {
                errors = ((ValidationException)exception).Errors.Select(x => x.ErrorMessage),
                statusCode = StatusCodes.Status400BadRequest,
                isValid = true
            }.ToString());

        

        List<string> errors = new()
        {
            exception.Message,
        };

        return httpContext.Response.WriteAsync(new ExceptionModel
        {
            errors = errors,
            statusCode = statusCode,
            isValid = true
        }.ToString());

    }

    private static int GetStatusCode(Exception exception)
        => exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError,
        };
}
