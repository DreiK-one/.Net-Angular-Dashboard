using Data.Entities;
using System.Security.Claims;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateJwtToken(User user);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipleFromExpiredToken(string token);
    }
}
