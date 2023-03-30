using API.Controllers.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IServerService _serverService;

        public ServerController(IServerService serverService)
        {
            _serverService = serverService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var servers = await _serverService.GetServers();

                return Ok(servers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }      
        }

        [HttpGet("{id}", Name = "GetServer")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var server = await _serverService.GetServerById(id);

                if (server == null)
                {
                    return BadRequest();
                }

                return Ok(server);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ServerMessage message)
        {
            try
            {
                var server = await _serverService.GetServerById(id);

                if (server == null)
                {
                    return NotFound();
                }

                await _serverService.UpdateServerStatus(server, message.Payload);

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }           
        }
    }
}
