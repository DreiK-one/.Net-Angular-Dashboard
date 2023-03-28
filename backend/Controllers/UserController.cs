using Core.DTOs;
using Core.Helpers;
using Core.Interfaces;
using Core.Models;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserController(ApiContext context, 
            IConfiguration configuration,
            IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
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
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

            await _context.SaveChangesAsync();

            return Ok(new TokenDto
            { 
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
        {
            if (tokenDto == null)
            {
                return BadRequest("Invalid client request");
            }

            var accessToken = tokenDto.AccessToken;
            var refreshToken = tokenDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid request");
            }

            var newAccessToken = CreateJwtToken(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;

            await _context.SaveChangesAsync();

            return Ok(new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist!"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            var from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email sent!"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.NormalizedEmail == resetPasswordDto.Email.ToUpperInvariant());

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User doesn't exist!"
                });
            }

            var tokenCode = user.ResetPasswordToken;
            var emailTokenExpiry = user.ResetPasswordExpiry;

            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid reset link!"
                });
            }

            user.PasswordHash = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfully!"
            });
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
            var key = Encoding.ASCII.GetBytes(_configuration["SecretKey"]);
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

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["SecretKey"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null 
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is invalid token!");
            }

            return principal;
        }
    }
}

