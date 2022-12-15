using backend.Controllers.Helpers;
using backend.Data;
using backend.Data.Entities;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly ApiContext _context;

        public ServerController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = _context.Servers.OrderBy(c => c.Id).ToList();

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetServer")]
        public IActionResult Get(int id)
        {
            var response = _context.Customers.Find(id);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Message(int id, [FromBody] ServerMessage message)
        {
            var server = _context.Servers.Find(id);

            if (server == null)
            {
                return NotFound();
            }


            // Refactor: move into a service
            if (message.Payload == "activate")
            {
                server.IsOnline = true;
            }

            if (message.Payload == "deactivate")
            {
                server.IsOnline = false;
            }

            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}
