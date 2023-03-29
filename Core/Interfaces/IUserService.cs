using Core.DTOs;
using Data.Entities;


namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByUsernameWithIncludes(AuthDto authDto);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<int> CreateUser(RegisterDto registerDto);
        Task<int> UpdateResetPropertiesByEmailToken(User user, string emailToken);
        Task<int> SetNewPassword(User user, string password);
        Task<bool> CheckUserNameExists(string username);
        Task<bool> CheckEmailExists(string email);
        string CheckPasswordStrength(string password);  
    }
}
