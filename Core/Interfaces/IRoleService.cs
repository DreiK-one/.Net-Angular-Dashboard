using Data.Entities;

namespace Core.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetUserRole();
    }
}
