using Core.ApiRequests.Account;
using Core.ApiResponses.Account;
using Core.ApiResponses.Base;
using Core.IdentityService.Interfaces;
using Core.Mapping.Interfaces;
using Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IIdentityServiceMapper _identityServiceMapper;

        public AccountController(IServiceProvider serviceProvider,
            IUserRepository userRepository,
            IIdentityService identityService,
            IIdentityServiceMapper identityServiceMapper) : base(serviceProvider, userRepository)
        {
            _identityService = identityService;
            _identityServiceMapper = identityServiceMapper;
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A newly created auth data</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SignUpAsync
        ///     {
        ///          "userName": "string",
        ///          "password": "string",
        ///          "phone": "string",
        ///          "email": "user@example.com"
        ///     }     
        ///
        /// </remarks>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the register user not sucess</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpRequestModel request)
        {
            var result = await _identityService.SignUpAsync(request.UserName, request.Phone, request.Email, request.Password);

            if (!result.IsSucess)
            {
                var errorResponse = ErrorResponseBuilder.Build(result.ErrorMessage);
                return BadRequest(errorResponse);
            }

            var mapResult = _identityServiceMapper.MapSignUpResponse(result);
            var response = SucessResponseBuilder.Build(mapResult);

            return Ok(response);
        }


        /// <summary>
        /// Authorize user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A newly created auth data</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SignInAsync
        ///     {
        ///          "userName": "string",
        ///          "password": "string",
        ///     }     
        ///
        /// </remarks>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the register user not sucess</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody]SignInRequestModel request)
        {
            var result = await _identityService.SignInAsync(request.UserName, request.Password);

            if (!result.IsSucess)
            {
                var errorResponse = ErrorResponseBuilder.Build(result.ErrorMessage);
                return result.IsUnAuthorizedError 
                    ? Unauthorized(errorResponse)
                    : BadRequest(errorResponse);
            }

            var mapResult = _identityServiceMapper.MapSignInResponse(result);
            var response = SucessResponseBuilder.Build(mapResult);
            
            return Ok(response);
        }


        /// <summary>
        /// update user refresh token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A newly created auth data</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SignInAsync
        ///     {
        ///          "acessToken": "string",
        ///     }     
        ///
        /// </remarks>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the auth user not sucess</response>
        /// <response code="401">If the auth user not sucess</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("updatetoken")]
        public async Task<IActionResult> UpdateAcessTokenAsync([FromBody]UpdateAcessTokenRequest request)
        {
            var result = await _identityService.UpdateJwtAsync(request.RefreshToken);

            if (!result.IsSucess)
            {
                var errorResponse = ErrorResponseBuilder.Build(result.ErrorMessage);
                return result.IsUnAuthorizedError
                    ? Unauthorized(errorResponse)
                    : BadRequest(errorResponse);
            }

            var mapResult = _identityServiceMapper.MapSignInResponse(result);
            var response = SucessResponseBuilder.Build(mapResult);

            return Ok(response);
        }

        /// <summary>
        /// get user data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A newly created auth data</returns>
        /// <response code="200">Returns the newly created item</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("getAuthInfo")]
        [Authorize]
        public IActionResult GetAuthInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = new AuthInfo()
                {
                    Roles = RoleNames,
                    SessionId = SessionId,
                    TokenId = TokenId,
                    UserId = UserId,
                    UserName = UserName
                };

                var response = SucessResponseBuilder.Build(result);
                return Ok(response);
            }

            return Ok(SucessResponseBuilder.Build(new { message = "User is not authorized" }));
        }
    }
}
