using Core.DialogServices.Interfaces;
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
    
    public DialogController(IServiceProvider serviceProvider, 
        IUserRepository userRepository,
        IDialogService dialogService) : base(serviceProvider, userRepository)
    {
        _dialogService = dialogService;
    }

    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("createDialog")]
    public async Task CreateDialogAsync()
    {
        
    }
}