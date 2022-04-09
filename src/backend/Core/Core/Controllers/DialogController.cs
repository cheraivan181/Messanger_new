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
    public DialogController(IServiceProvider serviceProvider, 
        IUserRepository userRepository) : base(serviceProvider, userRepository)
    {
    }

    public async Task CreateDialogRequestAsync()
    {
        
    }

    public async Task ConfirmDialogRequestAsync()
    {
        
    }

    public async Task CanceldialogRequestAsync()
    {
        
    }
}