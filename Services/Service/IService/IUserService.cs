﻿using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service.IService
{
    public interface IUserService
    {
        Task<Response> GetAll(string filter);
    }
}
