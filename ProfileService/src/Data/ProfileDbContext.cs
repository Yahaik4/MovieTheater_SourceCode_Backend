using Microsoft.EntityFrameworkCore;
using src.Infrastructure.EF.Models;

namespace src.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
