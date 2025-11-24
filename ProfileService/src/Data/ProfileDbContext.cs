using Microsoft.EntityFrameworkCore;
using ProfileService.Infrastructure.EF.Models;

namespace ProfileService.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
    }
}
