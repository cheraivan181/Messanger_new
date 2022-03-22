﻿using Core.Repositories.Interfaces;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = CommonConstants.ProtocoledUser)]
    public class SessionController : BaseController
    {
        public SessionController(IServiceProvider serviceProvider,
            IUserRepository userRepository) 
                : base(serviceProvider, userRepository)
        {
        }
    }
}