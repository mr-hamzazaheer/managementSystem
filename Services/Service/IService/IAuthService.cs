using Shared.Common;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service.IService;
public interface IAuthService
{
    Task<Response> LoginAsync(LoginDto dto);
    Task<Response> RegisterAsync(RegisterRequestDto dto);
}