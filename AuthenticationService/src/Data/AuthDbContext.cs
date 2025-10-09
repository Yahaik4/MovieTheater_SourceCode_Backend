using Microsoft.EntityFrameworkCore;
using src.Infrastructure.EF.Models;

namespace src.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId).HasName("PK_Users");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(s => s.SessionId).HasName("PK_Sessions");

                entity.HasOne(s => s.User)
                    .WithMany(u => u.Sessions)
                    .HasForeignKey(s => s.UserId)
                    .HasConstraintName("FK_Sessions_Users_UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
