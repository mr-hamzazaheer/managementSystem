using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Service;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;
using Shared.DTO;
using Shared.Generic;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IRoleService _roleService;

    public RoleController(UserManager<IdentityUser> userManager, IConfiguration config,
        IRoleService roleService)
    {
        _userManager = userManager;
        _config = config; _roleService = roleService;
    }

    [HttpGet("get-all")]
    public async Task<Response> GetAll() =>
        await _roleService.GetAllAsync();
    [HttpGet("{id}")]
    public async Task<Response> GetById(string id) =>
        await _roleService.GetById(id);

    [HttpPost]
    public async Task<Response> Create([FromBody] ApplicationRole role) =>
        await _roleService.CreateAsync(role);

    [HttpPut("{id}")]
    public async Task<Response> Update(string id, [FromBody] ApplicationRole updatedRole)=>
        await _roleService.UpdateAsync(id, updatedRole);
    [HttpDelete("{id}")]
    public async Task<Response> DeleteAsync(string id)=>
        await _roleService.DeleteAsync(id);
}
