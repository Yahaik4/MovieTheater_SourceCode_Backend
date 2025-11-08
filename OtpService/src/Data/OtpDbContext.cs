using System;
using Microsoft.EntityFrameworkCore;
using src.Infrastructure.EF.Models;

public class OtpDbContext : DbContext
{
    public DbSet<Otp> Otps { get; set; }

    public OtpDbContext(DbContextOptions<OtpDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).HasMaxLength(20).IsRequired();
            entity.Property(e => e.ExpiryAt).IsRequired();
        });
    }
}