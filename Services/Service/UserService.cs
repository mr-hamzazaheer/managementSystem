using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Logger.ILogger;
using Services.Service.IService;
using Services.UnitOfWork.IUnitOfWork;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service;
    public class UserService :IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityLogger _activityLogger;
        public Response _response;
        private readonly Jwt _jwt;
        public UserService(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork,
          IActivityLogger activityLogger, IOptions<Jwt> options)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _activityLogger = activityLogger;
            _response = new(); _jwt = options.Value;
        }

        public async Task<Response> GetAll()
        {
            return _response = await _unitOfWork._userRepository.GetAll();
        }
    }
