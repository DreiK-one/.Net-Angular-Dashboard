using backend.Data;
using backend.Data.Entities;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApiContext _context;

        public RoleController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _context.Roles.OrderBy(c => c.Id);

            return Ok(data);
        }
    }
}

