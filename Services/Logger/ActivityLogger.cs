using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Services.Logger.ILogger;
using Shared.Enums; 
using Shared.Extensions;
using System.Security.Claims;

namespace Services.Logger;
public class ActivityLogger : IActivityLogger
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActivityLogger(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context; _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(ActivityAction action, string entityName, string entityId, object requestData, object responseData = null)
    {
        var context = _httpContextAccessor.HttpContext;
        var userId = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
        var userName = context?.User?.Identity?.Name ?? "Anonymous";
        var log = new ActivityLog
        {
            UserId = userId,
            UserName = userName,
            Action = action.GetEnumDisplayName(),
            EntityName = entityName,
            EntityId = entityId,
            DataJson = JsonConvert.SerializeObject(requestData),
            ResponseJson = responseData != null ? JsonConvert.SerializeObject(responseData) : null,
            IpAddress = context?.Connection?.RemoteIpAddress?.ToString(),
            UserAgent = context?.Request?.Headers["User-Agent"].ToString(),
            Url = $"{context?.Request?.Scheme}://{context?.Request?.Host}{context?.Request?.Path}{context?.Request?.QueryString}",
            CreatedDate = DateTime.UtcNow
        };

        await _context.ActivityLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
