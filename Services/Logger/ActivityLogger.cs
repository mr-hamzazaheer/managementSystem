using Infrastructure;
using Infrastructure.Entities;
using Services.Logger.ILogger;

namespace Services.Logger;
public class ActivityLogger : IActivityLogger
{
    private readonly ApplicationDbContext _context;

    public ActivityLogger(ApplicationDbContext context) => _context = context;

    public async Task LogAsync(string userId, string action, string data)
    {
        var log = new ActivityLog
        {
            UserId = userId,
            Action = action,
            Data = data
        };

        await _context.ActivityLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
