using Core.DTOs;
using Data.Entities;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByUsernameWithIncludes(AuthDto authDto);
        Task<User> GetUserByEmail(string email);
        Task<int> CreateUser(User user);

        Task<bool> CheckUserNameExists(string username);
        Task<bool> CheckEmailExists(string email);
        string CheckPasswordStrength(string password);  
    }
}
