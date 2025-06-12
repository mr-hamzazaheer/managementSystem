using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logger.ILogger;
public interface IActivityLogger
{
    Task LogAsync(string userId, string action, string data);
}