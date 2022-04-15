using Core.ApiRequests.Dialog;
using Core.ApiResponses.Base;
using Core.DialogServices.Interfaces;
using Core.Mapping.Interfaces;
using Core.Repositories.Interfaces;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[Produces("application/json")]
[Authorize(Roles = CommonConstants.ProtocoledUser)]
[Route("api/[controller]")]
[ApiController]
public class DialogController : BaseController
{
    private readonly IDialogService _dialogService;
    private readonly IDialogRequestService _dialogRequestService;
    private readonly IDialogMapper _dialogMapper;
    
    public DialogController(IServiceProvider serviceProvider, 
        IUserRepository userRepository,
        IDialogService dialogService,
        IDialogRequestService dialogRequestService,
        IDialogMapper dialogMapper) : base(serviceProvider, userRepository)
    {
        _dialogService = dialogService;
        _dialogRequestService = dialogRequestService;
        _dialogMapper = dialogMapper;
    }
    
    
    /// <summary>
    /// Create dialog.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A newly created dialog data</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /createDialog
    ///     {
    ///          "RequestUserId": "string"
    ///     }     
    ///
    /// </remarks>
    /// <response code="200">Returns the newly created item</response>
    /// <response code="400">If the register user not sucess</response>
    /// <response code="500">server error</response>

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("createDialog")]
    public async Task<IActionResult> CreateDialogAsync([FromBody] CreateDialogRequest dialogRequest)
    {
        var createDialogResult =
            await _dialogService.CreateDialogRequestAsync(UserId, dialogRequest.RequestUserId, SessionId);
        
        if (!createDialogResult.IsSucess)
        {
            var errorResponse = ErrorResponseBuilder.Build(createDialogResult.ErrorMessage);
            return BadRequest(errorResponse);
        }

        var mapResult = _dialogMapper.Map(createDialogResult);
        var response = SucessResponseBuilder.Build(mapResult);

        return CreateResult(response);
    }

    
    /// <summary>
    /// Get dialogs.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A newly created dialog data</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /getdialogs
    ///
    /// </remarks>
    /// <response code="200">Returns all user dialogs</response>
    /// <response code="400">If buisness logic error</response>
    /// <response code="500">server error</response>

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("getdialogs")]
    public async Task<IActionResult> GetDialogsAsync()
    {
        var getDialogResult = await _dialogService.GetDialogsAsync(UserId, SessionId);
        if (!getDialogResult.IsSucess)
        {
            var errorResponse = ErrorResponseBuilder.Build(getDialogResult.ErrorMessage);
            return BadRequest(errorResponse);
        }

        var mapResult = _dialogMapper.Map(getDialogResult);
        var response = SucessResponseBuilder.Build(mapResult);
        
        return Ok(response);
    }
    
    
    /// <summary>
    /// Process dialogs
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A processDialog</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /processDialogRequest
    ///     {
    ///         "requestUserId" : "string",
    ///         "isAccept" : "boolean"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns all user dialogs</response>
    /// <response code="400">If buisness logic error</response>
    /// <response code="500">server error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("processDialogRequest")]
    public async Task<IActionResult> ProcessDialogRequestAsync([FromBody]ProcessDialogRequest request)
    {
        var result =
            await _dialogRequestService.ProcessDialogRequestAsync(UserId, request.RequestUserId, request.IsAccept);

        if (!result.IsSucess)
        {
            var errorResponse = ErrorResponseBuilder.Build(result.ErrorMessage);
            return BadRequest(errorResponse);
        }

        var response = new MessageResponse("dialog was updated");
        var sucessResponse = SucessResponseBuilder.Build(response);

        return Ok(sucessResponse);
    }
}