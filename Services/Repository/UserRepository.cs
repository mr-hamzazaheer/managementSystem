using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Repository.IRepository;
using Shared.Common;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public class UserRepository :IUserRepository
    {
        private Response _response;
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) {
            _response = new Response();
            //_context = context; Console.WriteLine($"GenericRepository DbContext Hash: {_context.GetHashCode()}");
        }

        public async Task<Response> GetAll()
        {
             _response.Data = await _context.Users.AsNoTracking().ToListAsync();
            _response.Message = "";
            return _response;
        }
        public async Task<User?> GetByIdOrAspNetUserIdAsync(int? userId, string? aspNetUserId)
        {
            if (userId.HasValue && userId.Value > 0)
            {
                return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId.Value);
            }
            if (!string.IsNullOrWhiteSpace(aspNetUserId))
            {
                return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.AspNetUserId == aspNetUserId);
            }
            // Both params missing → return null (null ref return)
            return null;
        }


    }
}
