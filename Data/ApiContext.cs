using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        //ADD NEW TABLES AND THEN ADD NEW MIGRATIONS !!!

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}
