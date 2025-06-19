using Services.Logger.ILogger;
using Services.Repository.IRepository; 

namespace Services.UnitOfWork.IUnitOfWork;
public interface IUnitOfWork : IDisposable
{
    //IGenericRepository<T> Repository<T>() where T : BaseEntity;
    IRoleRepository _roleRepository { get; }
    IUserRepository _userRepository { get; }
    IActivityLogger _activityLog { get; }
    Task<int> SaveChangesAsync();

}