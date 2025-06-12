using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities;
public class ActivityLog : BaseEntity
{ 
    public string UserId { get; set; }
    public string Action { get; set; }
    public string Data { get; set; }
    public string Json { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}