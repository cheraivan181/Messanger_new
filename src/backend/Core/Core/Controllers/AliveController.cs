using Core.ApiResponses.Alive;
using Core.ApiResponses.Base;
using Microsoft.AspNetCore.Mvc;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliveController : ControllerBase
    {
        private readonly ILogger _logger;
        
        public AliveController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AliveController>();
        }

        /// <summary>
        /// Validate and create Disburse. Should be called before Loan created.
        /// </summary>
        /// <param name="request">The <see cref="CreateDisburseRequest"/> SNN and Value as required. 
        /// The Value should contain sum body and percentage of Loan</param>
        /// <returns>The <see cref="CreateDisburseResponse"/> contained RecordId which should be used in <see cref="UpdateWithLoanIdRequest"/> request.
        /// Also response contains rest of monthly and yearly amounts.
        /// </returns>
        /// 
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public IActionResult IsAlive()
        {
            _logger.LogDebug("ok");

            var result = new IsAliveResponse()
            {
                IsAlive = true,
                Date = DateTime.Now
            };

            var response = ResponseBuilder<IsAliveResponse>.Build(result);
            return Ok(response);
        }
    }
}
