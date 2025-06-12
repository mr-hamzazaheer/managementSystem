

namespace Shared.Common;  
public class Response<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public static Response<T> SuccessResponse(T data, string message = null)
    {
        return new Response<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = null
        };
    }

    public static Response<T> ErrorResponse(string message, IEnumerable<string> errors = null)
    {
        return new Response<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors
        };
    }
}
