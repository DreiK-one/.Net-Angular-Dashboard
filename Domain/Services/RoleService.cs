using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApiContext _context;
        public RoleService(ApiContext context)
        {
            _context = context;
        }

        public async Task<Role> GetUserRole()
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == 2);

                if (role == null)
                {
                    throw new NullReferenceException();
                }

                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
