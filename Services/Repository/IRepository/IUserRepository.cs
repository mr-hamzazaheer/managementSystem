using Shared.Common;
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
    }
}
