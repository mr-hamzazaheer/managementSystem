using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;
using Shared.DTO;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public UserController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
        IUserService userService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork; _userService = userService;
    } 

    [HttpGet("get-all")]
    public async Task<Response> GetAll([FromQuery] string filter) =>
       await _userService.GetAll(filter);
}
