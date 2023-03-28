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
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ITokenService _tokenService;

        public UserController(ApiContext context, 
            IConfiguration configuration,
            IEmailService emailService, 
            IUserService userService,
            IRoleService roleService,
            ITokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _userService = userService;
            _roleService = roleService;
            _tokenService = tokenService;
        }
        //refactoring
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthDto authDto)
        {
            if (authDto == null)
            {
                return BadRequest();
;           }

            var user = await _userService.GetUserByUsernameWithIncludes(authDto);

            if (user == null)
            {
                return NotFound( new { Message = "User not found!" });
            }

            if (!PasswordHasher.VerifyPassword(authDto.Password, user.PasswordHash))
            {
                return BadRequest( new { Message = "Password is incorrect"});
            }          

            //refactoring
            user.Token = _tokenService.CreateJwtToken(user);
            var newAccessToken = user.Token;
            var newRefreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

            await _context.SaveChangesAsync();
            //refactoring

            return Ok(new TokenDto
            { 
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        //refactoring
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest();
            }

            if (await _userService.CheckUserNameExists(registerDto.Username))
            {
                return BadRequest(new { Message = "Username already exist!" });
            }

            if (await _userService.CheckEmailExists(registerDto.Email))
            {
                return BadRequest(new { Message = "Email already exist!" });
            }

            var pass = _userService.CheckPasswordStrength(registerDto.Password);

            if (!string.IsNullOrEmpty(pass))
            {
                return BadRequest(new { Message = pass });
            }

            var role = await _roleService.GetUserRole();

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                NormalizedEmail = registerDto.Email.ToUpperInvariant(),
                UserName = registerDto.Username,
                NormalizedUserName = registerDto.Username.ToUpperInvariant(),
                Token = "",
                PasswordHash = PasswordHasher.HashPassword(registerDto.Password), //refactoring
                RoleId = role.Id
            };

            await _userService.CreateUser(newUser);

            return Ok(new { Message = "User Registered!" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        //refactoring
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
        {
            if (tokenDto == null)
            {
                return BadRequest("Invalid client request");
            }

            //refactoring
            var accessToken = tokenDto.AccessToken;
            var refreshToken = tokenDto.RefreshToken;
            var principal = _tokenService.GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            //refactoring

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid request");
            }

            //refactoring
            var newAccessToken = _tokenService.CreateJwtToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;

            await _context.SaveChangesAsync();
            //refactoring

            return Ok(new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        //refactoring
        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _userService.GetUserByEmail(email);

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist!"
                });
            }

            //refactoring
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            var from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //refactoring

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email sent!"
            });
        }
        //refactoring
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _userService.GetUserByEmail(resetPasswordDto.Email);

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

            //refactoring
            user.PasswordHash = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //refactoring

            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfully!"
            });
        }
    }
}

