using Azure.Core;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Logger.ILogger;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;
using Shared.DTO;
using Shared.Enums;
using Shared.Extensions;
using Shared.Service.Interfaces;
using System;
using System.Security.Claims;

namespace Services.Service;
public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUnitOfWork _unitOfWork; 
    public Response _response; 
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IEmailSenderService _emailSender;
    private Jwt _jwt { get; }
    public AuthService(UserManager<IdentityUser> userManager,IUnitOfWork unitOfWork, IOptions<Jwt> options, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork; 
        _response = new(); 
        _jwt = options.Value;
        _emailSender = emailSender;
        httpContextAccessor = _httpContextAccessor;
    }

    public async Task<Response> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        //await _emailSender.SendEmailAsync(
        //    from: "hamza.zaheer.work@gmail.com",
        //    to: dto.Email,
        //    subject: "Login Attempt",
        //    bodyHtml: $"<p>Login attempt with email: {dto.Email}</p>",
        //    bodyHtml:  $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>",
        //    cc: null,
        //    bcc: null,
        //    attachments: null
        //);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            _response.Message = Message.LoginFaild;
            _response.HttpCode = System.Net.HttpStatusCode.Unauthorized;
            return _response;
        }
        User userInfo = await _unitOfWork._userRepository.GetByIdOrAspNetUserIdAsync(0,user.Id);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserName", userInfo.FirstName)
        };
        var token = _jwt.GenerateToken(claims, isRemember: dto.IsRemember);
        _response.Data = new
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email
        };
        _response.Message = Message.Success; 
        await _unitOfWork._activityLog.LogAsync(
            action: ActivityAction.Login,
            entityName: typeof(User).Name,
            entityId: user.Id,
            requestData: userInfo,
            responseData: user
        );
        await _unitOfWork.SaveChangesAsync();
        return _response;
    }
    public async Task<Response> RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            _response.Message = Message.AlreadyExist;
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email, 
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            _response.Message = Message.Success;
        }
        var newUser = await _userManager.FindByEmailAsync(dto.Email);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var request = _httpContextAccessor.HttpContext?.Request;
        var scheme = request?.Scheme ?? "https"; // fallback to https
        var host = request?.Host.Value ?? "localhost:7165"; // fallback host

        var url = $"{scheme}://{host}/api/Auth/ConfirmEmail?userId={user.Id}&token={token.Encode()}";
        await _unitOfWork._userRepository.AddUserAsync(new UserDto
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            ContactNumber = dto.ContactNo,
            AspnetUserId = newUser.Id
        });
        await _unitOfWork._activityLog.LogAsync(
            action: ActivityAction.Register,
            entityName: typeof(User).Name,
            entityId: newUser.Id,
            requestData: existingUser,
            responseData: user
        );
        await _unitOfWork.SaveChangesAsync();
        await _emailSender.SendEmailAsync(
            from: "hamza.zaheer.work@gmail.com",
            to: dto.Email,
            subject: "Conform Email",
            bodyHtml: $"Please confirm your account by clicking this link: <a href='{url}'>Conform Account</a>",
            cc: null,
            bcc: null,
            attachments: null 
        );
        _response.Message = Message.Success;
        return _response;
    }

    public async Task<Response> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _response.Message = Message.NotFound;
            _response.HttpCode = System.Net.HttpStatusCode.NotFound;
            return _response;
        }
        var result = await _userManager.ConfirmEmailAsync(user, token.Decode());
        if (!result.Succeeded)
        {
            _response.Message = Message.Error;
            _response.HttpCode = System.Net.HttpStatusCode.BadRequest;
            return _response;
        }
        _response.Message = Message.Success;
        return _response;   
    }
}
