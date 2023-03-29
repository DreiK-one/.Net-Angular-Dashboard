using Core.DTOs;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IEmailService emailService, 
            IUserService userService,
            ITokenService tokenService)
        {
            _emailService = emailService;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthDto authDto)
        {
            try
            {
                if (authDto == null)
                {
                    return BadRequest();
                    ;
                }

                var user = await _userService.GetUserByUsernameWithIncludes(authDto);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found!" });
                }

                if (!PasswordHasher.VerifyPassword(authDto.Password, user.PasswordHash))
                {
                    return BadRequest(new { Message = "Password is incorrect" });
                }

                var token = await _tokenService.CreateTokenForAuthenticatedUser(user);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }  
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
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

                await _userService.CreateUser(registerDto);

                return Ok(new { Message = "User Registered!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }       
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }        
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
        {
            try
            {
                if (tokenDto == null)
                {
                    return BadRequest("Invalid client request");
                }

                var principal = _tokenService.GetPrincipleFromExpiredToken(tokenDto.AccessToken);

                var user = await _userService.GetUserByUsername(principal.Identity.Name);

                if (user is null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest("Invalid request");
                }

                var refreshToken = await _tokenService
                    .CreateRefreshTokenForAuthenticatedUser(user);

                return Ok(refreshToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }        
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            try
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

                var emailToken = _tokenService.GenerateRandomToken();

                await _userService.UpdateResetPropertiesByEmailToken(user, emailToken);

                _emailService.SendEmail(email, emailToken);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Email sent!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }        
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
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

                if (user.ResetPasswordToken != resetPasswordDto.EmailToken || user.ResetPasswordExpiry < DateTime.Now)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Invalid reset link!"
                    });
                }

                await _userService.SetNewPassword(user, resetPasswordDto.NewPassword);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Password reset successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }          
        }
    }
}

