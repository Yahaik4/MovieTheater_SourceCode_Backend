using CinemaService.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CinemaService.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<ShowtimeSeat> ShowtimeSeats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasOne(r => r.Cinema)
                    .WithMany(c => c.Rooms)
                    .HasForeignKey(r => r.CinemaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.RoomType)
                    .WithMany(rt => rt.Rooms)
                    .HasForeignKey(r => r.RoomTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasOne(s => s.SeatType)
                    .WithMany(st => st.Seats)
                    .HasForeignKey(s => s.SeatTypeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Room)
                    .WithMany(r => r.Seats)
                    .HasForeignKey(s => s.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Showtime>(entity =>
            {
                entity.HasOne(st => st.Room)
                    .WithMany(r => r.Showtimes)
                    .HasForeignKey(st => st.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(st => st.ShowtimeSeats)
                    .WithOne(sts => sts.Showtime)
                    .HasForeignKey(sts => sts.ShowTimeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ShowtimeSeat>(entity =>
            {
                entity.HasOne(sts => sts.Seat)
                    .WithMany(s => s.ShowtimeSeats)
                    .HasForeignKey(sts => sts.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sts => sts.Booking)
                    .WithMany(b => b.ShowtimeSeats)
                    .HasForeignKey(sts => sts.BookingId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasOne(b => b.Showtime)
                      .WithMany(st => st.Bookings)
                      .HasForeignKey(b => b.ShowtimeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Booking>()
                        .Property(b => b.BookingSeats)
                        .HasColumnType("jsonb");
        }
    }
}
