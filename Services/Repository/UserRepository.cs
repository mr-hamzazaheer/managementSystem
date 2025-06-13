using Infrastructure;
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
            _context = context; Console.WriteLine($"GenericRepository DbContext Hash: {_context.GetHashCode()}");
        }

        public async Task<Response> GetAll()
        {
             _response.Data = await _context.Users.ToListAsync();
            _response.Message = "";
            return _response;
        }
    }
}
