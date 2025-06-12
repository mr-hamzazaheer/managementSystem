using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities;
public class ApplicationRole : IdentityRole
{
    public string GroupEmail { get; set; }
    public bool IsDeleteAllow { get; set; } = true;
    public bool IsDisplay { get; set; } = true;
    public bool IsDelete { set; get; }
    public bool IsAccess { set; get; }
}