using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logger.ILogger;
public interface IActivityLogger
{
    Task LogAsync(ActivityAction action, string entityName, string entityId, object requestData, object responseData = null);
}