using Core.DTOs;
using Data.Entities;
using System.Security.Claims;


namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateTokenForAuthenticatedUser(User user);
        Task<TokenDto> CreateRefreshTokenForAuthenticatedUser(User user);
        string GenerateRandomToken();
        ClaimsPrincipal GetPrincipleFromExpiredToken(string token);     
    }
}
