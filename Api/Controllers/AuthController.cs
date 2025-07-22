using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;
using Shared.DTO;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager; 
    private readonly IAuthService _authService;

    public AuthController(UserManager<IdentityUser> userManager,
        IAuthService authService)
    {
        _userManager = userManager; _authService = authService;
    }

    [HttpPost("login")]
    //[Authorize]
    public async Task<Response> Login([FromBody] LoginDto dto)=>
       await _authService.LoginAsync(dto);

    [HttpPost("register")]
    public async Task<Response> Register([FromBody] RegisterRequestDto dto) =>
       await _authService.RegisterAsync(dto);
    [HttpGet("ConfirmEmail")]
    public async Task<Response> ConfirmEmail(string userId, string token)=>
        await _authService.ConfirmEmail(userId, token);
}
