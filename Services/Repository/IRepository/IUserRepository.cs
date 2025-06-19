using Infrastructure.Entities;
using Shared.Common;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<Response> GetAll();
        Task<User> AddUserAsync(UserDto user);
        Task<User?> GetByIdOrAspNetUserIdAsync(int? userId = null, string? aspNetUserId= null);
    }
}
