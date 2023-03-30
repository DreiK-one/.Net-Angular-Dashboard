using Data.Entities;


namespace Core.Interfaces
{
    public interface IServerService
    {
        Task<Server> GetServerById(int id);
        Task<List<Server>> GetServers();
        Task<int> UpdateServerStatus(Server server, string status);
    }
}
