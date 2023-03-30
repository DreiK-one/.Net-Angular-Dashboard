using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Domain.Services
{
    public class ServerService : IServerService
    {
        private readonly ApiContext _context;

        public ServerService(ApiContext context)
        {
            _context = context;
        }

        public async Task<Server> GetServerById(int id)
        {
            try
            {
                var server = await _context.Servers
                    .FirstOrDefaultAsync(s => s.Id == id);

                return server;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Server>> GetServers()
        {
            try
            {
                var servers = await _context.Servers
                    .OrderBy(c => c.Id)
                    .ToListAsync();

                return servers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateServerStatus(Server server, string status)
        {
            try
            {
                if (status == "activate")
                {
                    server.IsOnline = true;
                }

                if (status == "deactivate")
                {
                    server.IsOnline = false;
                }

                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
