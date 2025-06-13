using Microsoft.AspNetCore.Mvc;

namespace Tasco.Gateway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Status = "Gateway is running", Timestamp = DateTime.UtcNow });
        }
    }
}