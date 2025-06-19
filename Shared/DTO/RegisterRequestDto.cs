using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO;
public class RegisterRequestDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }
    public string ContactNo { get; set; }
    public string Password { get; set; }
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}

