using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectionHandler.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AliveController : ControllerBase
{
    [HttpGet]
    public IActionResult IsAlive()
    {
        return Ok(new
        {
            isAlive=  true,
            date = DateTime.Now
        });
    }
}