using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }    
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalitycs()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
