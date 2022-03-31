using System;
using Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : BaseController
    {
        public ChatController(IServiceProvider serviceProvider,
            IUserRepository userRepository)
                : base(serviceProvider, userRepository)
        {
        }
    }
}
