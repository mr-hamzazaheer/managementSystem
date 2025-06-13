using Infrastructure.Entities;
using Services.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UnitOfWork.IUnitOfWork;
public interface IUnitOfWork : IDisposable
{
    //IGenericRepository<T> Repository<T>() where T : BaseEntity;
    IRoleRepository _roleRepository { get; }
    IUserRepository _userRepository { get; }
    Task<int> SaveChangesAsync();

}