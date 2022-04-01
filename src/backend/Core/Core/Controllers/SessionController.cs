using Core.ApiRequests.Session;
using Core.ApiResponses.Base;
using Core.Mapping.Interfaces;
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
        private readonly ISessionService _sessionService;
        private readonly ISessionServiceMapper _sessionServiceMapper;
        
        public SessionController(IServiceProvider serviceProvider,
            IUserRepository userRepository,
            ISessionService sessionService,
            ISessionServiceMapper sessionServiceMapper) 
                : base(serviceProvider, userRepository)
        {
            _sessionService = sessionService;
            _sessionServiceMapper = sessionServiceMapper;
        }
        
        
        /// <summary>
        /// Creates a session.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Created sessionData data</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /createSession
        ///     {
        ///         "publicKey": "string"
        ///     }     
        ///
        /// </remarks>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the register user not sucess</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = CommonConstants.UserRole)]
        [HttpPost("createSession")]
        public async Task<IActionResult> CreateSessionAsync([FromBody]CreateSessionRequest request)
        {
            var result = await _sessionService.CreateSessionAsync(UserId, request.PublicKey);
            if (!result.IsSucess)
            {
                var errorResponse = ErrorResponseBuilder.Build(result.ErrorMessage);
                return BadRequest(errorResponse);
            }

            var mapResult = _sessionServiceMapper.MapSessionResponse(result);
            var response = SucessResponseBuilder.Build(mapResult);
            
            return Ok(response);
        }
    }
}