using Core.DTOs;
using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;


namespace Domain.Services
{
    public class UserService : IUserService
    {
        private readonly ApiContext _context;
        public UserService(ApiContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetUserByUsernameWithIncludes(AuthDto authDto)
        {
            try
            {
                var user = await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == authDto.Username.ToUpperInvariant());

                if (user == null)
                {
                    throw new NullReferenceException();
                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());

                return user;
            }
            catch (Exception)
            {
                throw;
            }         
        }

        public async Task<int> CreateUser(User user)
        {
            try
            {
                if (user != null)
                {
                    await _context.Users.AddAsync(user);
                    return await _context.SaveChangesAsync();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            try
            {
                return await _context.Users
                .AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public async Task<bool> CheckUserNameExists(string username)
        {
            try
            {
                return await _context.Users
                .AnyAsync(u => u.NormalizedUserName == username.ToUpperInvariant());
            }
            catch (Exception)
            {
                throw;
            }        
        }

        public string CheckPasswordStrength(string password)
        {
            try
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
            catch (Exception)
            {
                throw;
            } 
        }
    }
}
