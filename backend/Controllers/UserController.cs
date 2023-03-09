using backend.Data;
using backend.Data.Entities;
using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;

        public UserController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthDto authDto)
        {
            if (authDto == null)
            {
                return BadRequest();
;           }

            var user = await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.UserName == authDto.UserName && u.PasswordHash == authDto.PasswordHash);

            if (user == null)
            {
                return NotFound( new { Message = "User Not Found!" });
            }

            return Ok(new { Message = "Login Success!"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userObject)
        {
            if (userObject == null)
            {
                return BadRequest();
            }

            await _context.Users.AddAsync(userObject);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User Registered!" });
        }
    }
}

