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
            Console.WriteLine($"GenericRepository DbContext Hash: {_context.GetHashCode()}");
        }

        public async Task<List<ApplicationRole>> GetAllAsync()
        {
            return await _context.Roles.Where(r=> r.IsDisplay == true).AsNoTracking().ToListAsync();
        }
        //public async Task<ApplicationRole> AddAsync(ApplicationRole role)
        //{
        //    ApplicationRole newRole = new()
        //    {
        //        Name = role.Name,
        //        NormalizedName = role.NormalizedName,
        //        GroupEmail = role.GroupEmail,
        //        IsDisplay = role.IsDisplay,
        //        IsAccess = role.IsAccess,
        //        IsDelete = role.IsDelete,
        //        IsDeleteAllow = role.IsDeleteAllow
        //    };

        //    _context.Roles.Add(role);
        //    return newRole;
        //}
    }
}
