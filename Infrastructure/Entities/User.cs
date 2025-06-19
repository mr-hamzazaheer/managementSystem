using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;
public class User:BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? ContactNumber { get; set; }
    [ForeignKey("IdentityUser")]
    public string AspNetUserId { get; set; }
    public virtual IdentityUser AspNetUser { get; set; }
}