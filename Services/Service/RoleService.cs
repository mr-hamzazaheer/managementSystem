using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Logger.ILogger;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;

namespace Services.Service;
public class RoleService : IRoleService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityLogger _activityLogger;
    public Response _response;
    private readonly Jwt _jwt;
    public RoleService(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
      IActivityLogger activityLogger, IOptions<Jwt> options)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _activityLogger = activityLogger;
        _response = new(); _jwt = options.Value;
    }

    public async Task<Response> GetAll()
    {
         await _unitOfWork._roleRepository.GetAll();
        return _response;
    }
}
