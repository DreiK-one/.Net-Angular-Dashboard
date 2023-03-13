using backend.Data;
using backend.Data.Entities;
using backend.Domain.DTOs;
using backend.Domain.Helpers;
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
                .FirstOrDefaultAsync(u => u.UserName == authDto.Username && u.PasswordHash == authDto.Password);

            if (user == null)
            {
                return NotFound( new { Message = "User Not Found!" });
            }

            return Ok(new { Message = "Login Success!"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest();
            }

            var role = _context.Roles.FirstOrDefault(r => r.Id == 2);

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                NormalizedEmail = registerDto.Email.ToUpperInvariant(),
                UserName = registerDto.Username,
                NormalizedUserName = registerDto.Username.ToUpperInvariant(),
                Token = "",
                PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
                Role = new Role { Id = role.Id, Name = role.Name }
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User Registered!" });
        }
    }
}

