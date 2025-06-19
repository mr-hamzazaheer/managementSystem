using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Refit;
using Shared.Common;
using System.Net;

namespace Api.Middleware;

public class Middleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Middleware> _logger;  
    private readonly IWebHostEnvironment _hostingEnvironment;
    public Middleware(RequestDelegate next, ILogger<Middleware> logger, IWebHostEnvironment hostingEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostingEnvironment = hostingEnvironment; 
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            if (httpContext.Request.Path.Value != null)
            {
                if (!string.IsNullOrEmpty(_hostingEnvironment.WebRootPath))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, httpContext.Request.Path.Value.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        await ServeStaticFileAsync(httpContext, filePath);
                        return; // Stop pipeline after serving
                    }
                }
                // If WebRootPath is null OR file does not exist → continue pipeline
                await _next(httpContext);

            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Default values
        var statusCode = StatusCodes.Status500InternalServerError;
        var message = Message.Exception;

        // Example categorization based on exception type or your custom exceptions
        switch (exception)
        {
            case ArgumentNullException:
            case ArgumentException:
                statusCode = StatusCodes.Status400BadRequest;
                message = Message.InvalidInput;
                break;

            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                message = Message.Unauthorized;
                break;

            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = Message.NotFound;
                break;

            // You can add more categories here...

            default:
                // Unknown exception → Keep 500
                break;
        }

        // Log the error
        _logger.LogInformation("Handling request: {Method} {Url} from {IP}",
            context.Request.Method,
            context.Request.Path,
            context.Connection.RemoteIpAddress);
        _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);
        _logger.LogInformation("Response: {StatusCode} for {Method} {Url}",
            context.Response.StatusCode,
            context.Request.Method,
            context.Request.Path);

        // Return structured JSON response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        Response response = new()
        {
            HttpCode = (HttpStatusCode)statusCode,
            Message = message, 
        };

        await context.Response.WriteAsJsonAsync(response);
    }
    public static async Task ServeStaticFileAsync(HttpContext httpContext, string filePath)
    {
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        if (!contentTypeProvider.TryGetContentType(filePath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        httpContext.Response.ContentType = contentType;
        httpContext.Response.StatusCode = StatusCodes.Status200OK;

        await using var stream = File.OpenRead(filePath);
        await stream.CopyToAsync(httpContext.Response.Body);
    }

}