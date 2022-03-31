using System;
using Core.ApiResponses.Alive;
using Core.ApiResponses.Base;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliveController : ControllerBase
    {
        [HttpGet]
        public IActionResult IsAlive()
        {
            var result = new IsAliveResponse()
            {
                IsAlive = true, 
                Date = DateTime.Now
            };

            var response = SucessResponseBuilder.Build(result);
            return Ok(response);
        }
    }
}
