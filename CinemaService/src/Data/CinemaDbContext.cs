using Microsoft.EntityFrameworkCore;
using src.Infrastructure.EF.Models;

namespace src.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<SeatLayout> SeatLayouts { get; set; }
        public DbSet<SeatLayoutDetail> SeatLayoutDetail { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<ColumnSkip> ColumnSkips { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasOne(r => r.Cinema)
                    .WithMany(c => c.Rooms)
                    .HasForeignKey(r => r.CinemaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.SeatLayout)
                    .WithMany(s => s.Rooms)
                    .HasForeignKey(r => r.LayoutId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SeatLayoutDetail>(entity =>
            {
                entity.HasOne(s => s.SeatLayout)
                    .WithMany(sl => sl.SeatLayoutDetails)
                    .HasForeignKey(s => s.SeatLayoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.SeatType)
                    .WithMany(st => st.SeatLayoutDetails)
                    .HasForeignKey(s => s.SeatTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            });

            modelBuilder.Entity<ColumnSkip>(entity =>
            {
                entity.HasOne(c => c.SeatLayout)
                    .WithMany(sl => sl.ColumnSkips)
                    .HasForeignKey(c => c.SeatLayoutId)
                    .OnDelete(DeleteBehavior.Cascade);
            });



        }
    }
}
