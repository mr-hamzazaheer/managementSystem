using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;

namespace Shared.Common;
public class Response
{
    public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;
    public string Token { get; set; }
    public string Message { get; set; }
    public dynamic Data { get; set; }
    [JsonIgnore] // Removed duplicate attribute  
    public bool IsValidated { get { return this.HttpCode == HttpStatusCode.OK; } }

    public Response() { }
    public virtual Response Format(string message, string[] values = null)
    {
        Message = message;
        return this;
    }
    public bool Status { get { return HttpCode == HttpStatusCode.OK; } }
}
public class BaseDto
{
    public int Id { get; set; }
    [JsonIgnore]
    public string Encode { get { return this.Id.ToString(); } }
    [NotMapped]
    public DateTime CreatedAt { get; set; }
    [NotMapped]
    public DateTime LastUpdatedAt { get; set; }
}
