using BronyBowling.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BronyBowling.Shared.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BowlingLane> BowlingLanes => Set<BowlingLane>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // ---------- USERS ----------
        b.Entity<User>(e =>
        {
            e.ToTable("users_table");
            e.HasKey(x => x.UserId);

            e.Property(x => x.PhoneNumber).HasMaxLength(11).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();

            e.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(50).IsRequired();
            e.Property(x => x.MiddleName).HasMaxLength(50);

            e.Property(x => x.City).HasMaxLength(100);
            e.Property(x => x.CreatedAt).IsRequired();
        });

        // ---------- BOWLING LANES ----------
        b.Entity<BowlingLane>(e =>
        {
            e.ToTable("bowling_lanes");
            e.HasKey(x => x.BowlingLaneId);

            e.Property(x => x.Number).IsRequired();
            e.Property(x => x.IsActive).IsRequired();
        });

        // ---------- BOOKINGS ----------
        b.Entity<Booking>(e =>
        {
            e.ToTable("bookings");
            e.HasKey(x => x.BookingId);

            e.Property(x => x.Status)
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .IsRequired();

            e.HasOne(x => x.Lane)
                .WithMany()
                .HasForeignKey(x => x.BowlingLaneId)
                .HasPrincipalKey(x => x.BowlingLaneId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
