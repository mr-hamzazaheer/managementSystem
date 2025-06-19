using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO;
public class UserDto : BaseDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? ContactNumber { get; set; }
    public string? AspnetUserId { get; set; }
}
