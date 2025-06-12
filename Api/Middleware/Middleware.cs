using Refit;
using Shared.Common;
using System.Net;

namespace Api.Middleware;

public class Middleware
{
    private readonly RequestDelegate _next;

    public Middleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            //await HandleExceptionAsync(httpContext, ex);
        }
    }

    //private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    //{
    //    context.Response.ContentType = "application/json";
    //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    //    if (exception is ApiException apiException)
    //    {
    //        context.Response.StatusCode = (int)apiException.StatusCode;
    //        return context.Response.WriteAsync(new Response<object>
    //        {
    //            Success = false,
    //            Message = apiException.Message,
    //            Errors = apiException.RequestMessage?.Content?.ReadAsStringAsync().Result != null
    //                ? new[] { apiException.RequestMessage.Content.ReadAsStringAsync().Result }
    //                : null
    //        }.ToString());
    //    }

    //    return context.Response.WriteAsync(new Response<object>
    //    {
    //        Success = false,
    //        Message = "An unexpected error occurred.",
    //        Errors = new[] { exception.Message }
    //    }.ToString());
    //}
}