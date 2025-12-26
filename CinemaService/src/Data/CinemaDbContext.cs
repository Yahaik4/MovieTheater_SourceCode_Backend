using CinemaService.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<PriceRule> PriceRules { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        // NEW
        public DbSet<FoodDrink> FoodDrinks { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }

        public DbSet<DailyRevenueReport> DailyRevenueReports { get; set; }
        public DbSet<MovieRevenueReport> MovieRevenueReports { get; set; }
        public DbSet<FoodDrinkRevenueReport> FoodDrinkRevenueReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ROOM
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

            // SEAT
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

            // SHOWTIME
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

            // SHOWTIME SEAT
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

            // ========== FoodDrink ==========
            modelBuilder.Entity<FoodDrink>(entity =>
            {
                entity.ToTable("FoodDrink");

                entity.Property(e => e.Id)
                      .HasColumnType("uuid");

                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Type)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Size)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Price)
                      .HasColumnType("numeric(10,2)")
                      .IsRequired();
            });

            // ========== BookingItem ==========
            modelBuilder.Entity<BookingItem>(entity =>
            {
                entity.ToTable("Booking_Items");

                entity.Property(e => e.Id)
                      .HasColumnType("uuid");

                entity.Property(e => e.BookingId)
                      .HasColumnType("uuid")
                      .IsRequired();

                entity.Property(e => e.ItemId)
                      .HasColumnType("uuid")
                      .IsRequired();

                entity.Property(e => e.Quantity)
                      .IsRequired();

                entity.Property(e => e.UnitPrice)
                      .HasColumnType("numeric(10,2)")
                      .IsRequired();

                entity.Property(e => e.TotalPrice)
                      .HasColumnType("numeric(10,2)")
                      .IsRequired();

                entity.HasOne(e => e.FoodDrink)
                      .WithMany(f => f.BookingItems)
                      .HasForeignKey(e => e.ItemId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Booking)
                      .WithMany(b => b.BookingItems)
                      .HasForeignKey(e => e.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // BOOKING
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasOne(b => b.Showtime)
                      .WithMany(st => st.Bookings)
                      .HasForeignKey(b => b.ShowtimeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(b => b.BookingSeats)
                      .HasColumnType("jsonb");

                entity.HasOne(b => b.Promotion)
                      .WithMany()
                      .HasForeignKey(b => b.PromotionId)
                      .IsRequired(false);
            });

            modelBuilder.Entity<PriceRule>(entity =>
            {
                entity.HasOne(pr => pr.CustomerType)
                      .WithMany(ct => ct.PriceRules)
                      .HasForeignKey(pr => pr.CustomerTypeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.HasIndex(p => p.Code)
                      .IsUnique();

                entity.Property(p => p.Code)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(p => p.StartDate)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();

                entity.Property(p => p.EndDate)
                      .HasColumnType("timestamp without time zone")
                      .IsRequired();
            });
        }
    }
}
