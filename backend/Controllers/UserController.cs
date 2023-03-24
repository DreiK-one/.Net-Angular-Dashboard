using backend.Data;
using backend.Data.Entities;
using backend.Domain.DTOs;
using backend.Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


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
                .FirstOrDefaultAsync(u => u.NormalizedUserName == authDto.Username.ToUpperInvariant());

            if (user == null)
            {
                return NotFound( new { Message = "User not found!" });
            }

            if (!PasswordHasher.VerifyPassword(authDto.Password, user.PasswordHash))
            {
                return BadRequest( new { Message = "Password is incorrect"});
            }

            user.Token = CreateJwtToken(user);

            return Ok(new 
            { 
                Token = user.Token,
                Message = "Login success!"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest();
            }

            if (await CheckUserNameExistsAsync(registerDto.Username))
            {
                return BadRequest(new { Message = "Username already exist!" });
            }

            if (await CheckEmailExistsAsync(registerDto.Email))
            {
                return BadRequest(new { Message = "Email already exist!" });
            }

            var pass = CheckPasswordStrength(registerDto.Password);
            if (!string.IsNullOrEmpty(pass))
            {
                return BadRequest(new { Message = pass });
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
                RoleId = role.Id
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User Registered!" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        private async Task<bool> CheckUserNameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.NormalizedUserName == username.ToUpperInvariant());
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
        }

        private string CheckPasswordStrength(string password)
        {
            var stringBuilder = new StringBuilder();

            if (password.Length < 8)
            {
                stringBuilder.Append($"Minimum password length should be 8 {Environment.NewLine}");
            }

            if (!Regex.IsMatch(password, "[a-z]") && !Regex.IsMatch(password, "[A-Z]") && !Regex.IsMatch(password, "[0-9]"))
            {
                stringBuilder.Append($"Password should be Alphanumeric {Environment.NewLine}");
            }

            if (!Regex.IsMatch(password, "[<,>,@,?,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,`,\\,.,/,~,',-,=]"))
            {
                stringBuilder.Append($"Password should contain special chars {Environment.NewLine}");
            }

            return stringBuilder.ToString();
        }

        private string CreateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("thisIsSecretKey......");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _context.Users
                .Any(u => u.RefreshToken == refreshToken);

            if (tokenInUser)
            {
                return CreateRefreshToken();
            }

            return refreshToken;
        }
    }
}

