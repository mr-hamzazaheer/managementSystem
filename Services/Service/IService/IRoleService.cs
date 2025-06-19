using Infrastructure.Entities;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service.IService
{
    public interface IRoleService
    {
        Task<Response> GetAll();
        Task<Response> CreateAsync(ApplicationRole role);
        Task<Response> UpdateAsync(string id, ApplicationRole updatedRole);
    }
}
