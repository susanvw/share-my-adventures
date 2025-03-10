using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShareMyAdventures.Application.Common.Middleware;

/// <summary>
/// Middleware to handle unhandled exceptions and log them.
/// Converts exceptions into a JSON response with appropriate status codes.
/// </summary>
/// <remarks>
/// Constructor to inject dependencies.
/// </remarks>
/// <param name="next">Next middleware in the pipeline.</param>
/// <param name="logger">Logger instance.</param>
public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{

    /// <summary>
    /// Middleware pipeline to catch and log unhandled exceptions.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Proceed to the next middleware
            await next(context);
        }
        catch (Exception ex)
        {
            // Log the unhandled exception
            Serilog.Log.Error(ex, "Unhandled Exception: {Message}", ex.Message);

            // Handle the exception and return a structured JSON response
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles exceptions by returning an appropriate HTTP response.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="exception">Caught exception.</param>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Set default response values
        var response = new
        {
            Message = "An unexpected error occurred.",
            Details = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Serialize and return the response
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
