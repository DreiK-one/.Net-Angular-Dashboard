using backend.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace backend.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Server> Servers { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}
