using Core.ApiResponses.Base;
using Core.Mapping.Interfaces;
using Core.Repositories.Interfaces;
using Core.SearchServices.Interfaces;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = CommonConstants.ProtocoledUser)]
public class SearchController : BaseController
{
    private readonly IUserSearchService _userSearchService;
    private readonly ISearchMapper _searchMapper;
    
    public SearchController(IServiceProvider serviceProvider, 
        IUserRepository userRepository,
        IUserSearchService userSearchService,
        ISearchMapper searchMapper) : base(serviceProvider, userRepository)
    {
        _userSearchService = userSearchService;
        _searchMapper = searchMapper;
    }
    
    /// <summary>
    /// Get a user by username 
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Users and marks is had dialogs</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /searchUser/Igor2123
    /// </remarks>
    /// <response code="200">Returns founded users</response>
    /// <response code="400">If something was wrong or server error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("searchUser/{predicate}")]
    public async Task<IActionResult> SearchUserAsync(string predicate)
    {
        var searchedUserResult = await _userSearchService.SearchUsersAsync(UserId, UserName, predicate);
        if (!searchedUserResult.IsSucess)
        {
            var errorResponse = ErrorResponseBuilder.Build(searchedUserResult.ErrorMessage);
            return BadRequest(errorResponse);
        }

        var mapResult = _searchMapper.MapSearchUserResponse(searchedUserResult.Result);
        var response = SucessResponseBuilder.Build(mapResult);
        return Ok(response);
    }
}