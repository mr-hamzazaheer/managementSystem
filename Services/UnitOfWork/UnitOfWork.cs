using Infrastructure;
using Infrastructure.Entities;
using Services.Repository;
using Services.Repository.IRepository;
using Services.UnitOfWork.IUnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();
    public IRoleRepository _roleRepository { get; }
    public IUserRepository _userRepository { get; }
    public UnitOfWork(ApplicationDbContext context, IRoleRepository roleRepository,
        IUserRepository userRepository)
    {
        _context = context;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }

    //public IGenericRepository<T> Repository<T>() where T : BaseEntity
    //{
    //    var type = typeof(T);
    //    if (!_repositories.ContainsKey(type))
    //        _repositories[type] = new GenericRepository<T>(_context);

    //    return (IGenericRepository<T>)_repositories[type];
    //}

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}