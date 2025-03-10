using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ShareMyAdventures.Application.Common.Middleware;

/// <summary>
/// Middleware to validate incoming HTTP requests using FluentValidation.
/// </summary>
/// <remarks>
/// Constructor for injecting the next middleware in the pipeline.
/// </remarks>
/// <param name="next">Next middleware delegate.</param>
public sealed class ValidationMiddleware(RequestDelegate next)
{

    /// <summary>
    /// Middleware pipeline to validate request models before processing.
    /// </summary>
    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint is null)
        {
            await next(context);
            return;
        }

        // Get the request model from the route or body
        var requestType = endpoint.Metadata
            .OfType<ValidationFilterMetadata>()
            .FirstOrDefault()?.RequestType;

        if (requestType is not null)
        {
            // Resolve validators from DI
            var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
            var validator = serviceProvider.GetService(validatorType) as IValidator;

            if (validator is not null)
            {
                var requestBody = await JsonSerializer.DeserializeAsync(
                    context.Request.Body, requestType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (requestBody is not null)
                {
                    var validationContext = new ValidationContext<object>(requestBody);
                    var validationResult = await validator.ValidateAsync(validationContext);

                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";

                        var errors = validationResult.Errors.Select(f => new { f.PropertyName, f.ErrorMessage });

                        var response = JsonSerializer.Serialize(new { Errors = errors });

                        await context.Response.WriteAsync(response);
                        return;
                    }
                }
            }
        }

        await next(context);
    }
}

/// <summary>
/// Marker attribute to determine which request types need validation.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ValidationFilterMetadata : Attribute
{
    public Type RequestType { get; }

    public ValidationFilterMetadata(Type requestType)
    {
        RequestType = requestType;
    }
}
