using Core.ApiResponses.Session;
using Core.IdentityService.Interfaces;
using Core.Repositories.Interfaces;
using Core.SessionServices.Services.Interfaces;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly ISessionService _sessionService;
        private readonly ISessionGetterService _sessionGetterService;
        
        public SessionController(IServiceProvider serviceProvider,
            IUserRepository userRepository,
            IIdentityService identityService,
            ISessionService sessionService,
            ISessionGetterService sessionGetterService) 
                : base(serviceProvider, userRepository)
        {
            _identityService = identityService;
            _sessionService = sessionService;
            _sessionGetterService = sessionGetterService;
        }

        [Authorize(Roles = CommonConstants.UserRole)]
        [HttpPost("createSession")]
        public async Task<IActionResult> CreateSessionAsync([FromBody]CreateSessionRequest request)
        {
            return Ok();
        }
    }
}