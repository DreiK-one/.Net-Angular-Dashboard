using Core.DTOs;
using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApiContext _context;
        private readonly IConfiguration _configuration;

        public TokenService(ApiContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenDto> CreateTokenForAuthenticatedUser(User user)
        {
            user.Token = CreateJwtToken(user);
            user.RefreshToken = CreateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

            await _context.SaveChangesAsync();

            var token = new TokenDto
            {
                AccessToken = user.Token,
                RefreshToken = user.RefreshToken
            };

            return token;
        }

        public async Task<TokenDto> CreateRefreshTokenForAuthenticatedUser(User user)
        {
            try
            {
                user.RefreshToken = CreateRefreshToken();

                await _context.SaveChangesAsync();

                var refreshToken = new TokenDto
                {
                    AccessToken = CreateJwtToken(user),
                    RefreshToken = user.RefreshToken
                };

                return refreshToken;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateRandomToken()
        {
            try
            {
                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                return Convert.ToBase64String(tokenBytes);
            }
            catch (Exception)
            {
                throw;
            }       
        }

        public ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
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
    }
}
