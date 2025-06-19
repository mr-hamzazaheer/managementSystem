using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public class RoleRepository :IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
            //Console.WriteLine($"GenericRepository DbContext Hash: {_context.GetHashCode()}");
        }

    }
}
