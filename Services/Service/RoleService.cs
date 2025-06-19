using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Logger.ILogger;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;

namespace Services.Service;
public class RoleService : IRoleService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityLogger _activityLogger;
    public Response _response;
    private readonly Jwt _jwt;
    public RoleService(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
      IActivityLogger activityLogger, IOptions<Jwt> options, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _activityLogger = activityLogger;
        _response = new(); _jwt = options.Value;
        _roleManager = roleManager;
    }

    public async Task<Response> GetAll()
    {
         await _unitOfWork._roleRepository.GetAllAsync();
        return _response;
    }
    public async Task<Response> CreateAsync(ApplicationRole role)
    {
        ApplicationRole newRole = new()
        {
            Name = role.Name,
            NormalizedName = role.NormalizedName ?? role.Name.ToUpperInvariant(),
            GroupEmail = role.GroupEmail,
            IsDisplay = role.IsDisplay,
            IsAccess = role.IsAccess,
            IsDelete = role.IsDelete,
            IsDeleteAllow = role.IsDeleteAllow
        };
        // Check if role already exists
        var existingRole = await _roleManager.FindByNameAsync(newRole.Name);
        if (existingRole != null)
        {
            _response.HttpCode = System.Net.HttpStatusCode.NotAcceptable; _response.Message = Message.AlreadyExist;
        }
        // Create role
        var result = await _roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            _response.HttpCode = System.Net.HttpStatusCode.NotAcceptable;_response.Message = Message.Exception;
        }
        else
            _response.Message = Message.Success;
        return _response;
    }
    public async Task<Response> UpdateAsync(string id,ApplicationRole updatedRole)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
            _response.Message = Message.NotFound;_response.HttpCode = System.Net.HttpStatusCode.NotFound; // 404
        // Update fields
        role.Name = updatedRole.Name;
        role.NormalizedName = updatedRole.NormalizedName ?? updatedRole.Name.ToUpperInvariant();
        role.GroupEmail = updatedRole.GroupEmail;
        role.IsDisplay = updatedRole.IsDisplay;
        role.IsAccess = updatedRole.IsAccess;
        role.IsDelete = updatedRole.IsDelete;
        role.IsDeleteAllow = updatedRole.IsDeleteAllow;
        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
            _response.Message = Message.Error; _response.HttpCode = System.Net.HttpStatusCode.BadRequest; // 404

        _response.Message = Message.Success;
        return _response;
    }
}
