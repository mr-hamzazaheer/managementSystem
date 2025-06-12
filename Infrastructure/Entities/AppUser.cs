﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities;
public class AppUser
{
    public string? FirstName { get; set; }
    public string? LasttName { get; set; }
    public string? Address { get; set; }
    public string? ContactNumber { get; set; }
}