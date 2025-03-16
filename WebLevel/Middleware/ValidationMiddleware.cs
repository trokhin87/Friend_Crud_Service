
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebLevel.Middleware;

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        var validationFailures = new List<string>();
        var requestServices = context.RequestServices;

        foreach (var arg in context.Request.RouteValues.Values)
        {
            if (arg == null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());
            var validator = requestServices.GetService(validatorType) as IValidator;
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(new ValidationContext<object>(arg));
                if (!validationResult.IsValid)
                {
                    validationFailures.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
                }
            }
        }

        if (validationFailures.Any())
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors = validationFailures }));
            return;
        }

        await _next(context);
    }
}

public static class ValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseValidationMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ValidationMiddleware>();
    }
}