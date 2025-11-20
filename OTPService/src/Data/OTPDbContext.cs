using Microsoft.EntityFrameworkCore;
using OTPService.Infrastructure.EF.Models;

namespace OTPService.Data
{
    public class OTPDbContext : DbContext
    {
        public OTPDbContext(DbContextOptions<OTPDbContext> options) : base(options)
        {
        }
        public DbSet<OTP> OTPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OTP>()
                .HasIndex(o => o.UserId)
                .IsUnique();
        }
    }
}
