using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Logger.ILogger;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;
using Shared.DTO;
using Shared.Extensions;
using System.Security.Claims;

namespace Services.Service;
public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityLogger _activityLogger;
    public Response _response; 
    private readonly Jwt _jwt;
    public AuthService(UserManager<IdentityUser> userManager,IUnitOfWork unitOfWork,
      IActivityLogger activityLogger, IOptions<Jwt> options)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _activityLogger = activityLogger;
        _response = new(); _jwt = options.Value;
    }

    public async Task<Response> LoginAsync(LoginDto dto)
    {
        await _unitOfWork._userRepository.GetAll();
        await _unitOfWork._roleRepository.GetAll();
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
             _response.Message="Invalid credentials";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var token = _jwt.GenerateToken(claims, isRemember: dto.IsRemember);
        _response.Data = new
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email
        };
        _response.Message = "Invalid credentials";
        return _response;
    }
    public async Task<Response> RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            _response.Message = "Email already registered.";
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email, 
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            _response.Message = "Registration failed.";
        }
        // Log the registration
        await _activityLogger.LogAsync(user.Id, "User Registered", $"Email: {user.Email}");
        return _response;
    }
}
