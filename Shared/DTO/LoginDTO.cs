﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO;
public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsRemember { get; set; }
}
public class AuthResponseDto
{
    public string Token { get; set; }
    public string Email { get; set; }
}