using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities;
public class ActivityLog : BaseEntity
{ 
    public int Id { get; set; }
    public string UserId { get; set; } // FK to Identity User
    public string UserName { get; set; }
    public string Action { get; set; } // "Create", "Update", "Delete", "Login", etc.
    public string EntityName { get; set; } // "Customer", "Order", etc.
    public string EntityId { get; set; } // PK of the entity (if any)
    public string DataJson { get; set; } // Full JSON data (request or entity state)
    public string ResponseJson { get; set; } // Full JSON of response (optional)
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}