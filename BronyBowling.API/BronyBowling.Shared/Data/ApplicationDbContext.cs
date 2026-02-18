using BronyBowling.Shared.Models;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace BronyBowling.Shared.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<BowlingCenter> BowlingCenters => Set<BowlingCenter>();
    public DbSet<BowlingLane> BowlingLanes => Set<BowlingLane>();
    public DbSet<Tariff> Tariffs => Set<Tariff>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // ---------- USERS ----------
        b.Entity<User>(e =>
        {
            e.ToTable("users");

            e.HasKey(x => x.UserId);

            e.Property(x => x.UserId)
                .HasColumnName("user_id");

            e.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            e.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .IsRequired();

            e.Property(x => x.LastName)
                .HasColumnName("last_name")
                .IsRequired();

            e.Property(x => x.MiddleName)
                .HasColumnName("middle_name");

            e.Property(x => x.BirthDate)
                .HasColumnName("birth_date");

            e.Property(x => x.City)
                .HasColumnName("city");

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at");
        });

        // ---------- ADMINS ----------
        b.Entity<Admin>(e =>
        {
            e.ToTable("admins");

            e.HasKey(x => x.AdminId);

            e.Property(x => x.AdminId)
                .HasColumnName("admin_id");

            e.Property(x => x.Login)
                .HasColumnName("login")
                .IsRequired();

            e.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            e.HasIndex(x => x.Login)
                .IsUnique();
        });

        // ---------- BOWLING CENTERS ----------
        b.Entity<BowlingCenter>(e =>
        {
            e.ToTable("bowling_centers");

            e.HasKey(x => x.BowlingCenterId);

            e.Property(x => x.BowlingCenterId)
                .HasColumnName("center_id");

            e.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            e.Property(x => x.City)
                .HasColumnName("city")
                .IsRequired();

            e.Property(x => x.Street)
                .HasColumnName("street")
                .IsRequired();

            e.Property(x => x.House)
                .HasColumnName("house")
                .IsRequired();

            e.Property(x => x.WorkingHours)
                .HasColumnName("work_hours")
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active");
        });

        // ---------- BOWLING LANES ----------
        b.Entity<BowlingLane>(e =>
        {
            e.ToTable("bowling_lanes");

            e.HasKey(x => x.LaneId);

            e.Property(x => x.LaneId)
                .HasColumnName("lane_id");

            e.Property(x => x.BowlingCenterId)
                .HasColumnName("center_id")
                .IsRequired();

            e.Property(x => x.Number)
                .HasColumnName("number")
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active");

            e.HasOne(x => x.Center)
                .WithMany(c => c.Lanes)
                .HasForeignKey(x => x.BowlingCenterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- TARIFFS ----------
        b.Entity<Tariff>(e =>
        {
            e.ToTable("tariffs");

            e.HasKey(x => x.TariffId);

            e.Property(x => x.TariffId)
                .HasColumnName("tariff_id");

            e.Property(x => x.BowlingLaneId)
                .HasColumnName("lane_id")
                .IsRequired();

            e.Property(x => x.DayOfWeek)
                .HasColumnName("day_of_week")
                .IsRequired();

            e.Property(x => x.PricePerHour)
                .HasColumnName("price_per_hour")
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active");

            e.HasOne(x => x.Lane)
                .WithMany(l => l.Tariffs)
                .HasForeignKey(x => x.BowlingLaneId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- BOOKINGS ----------
        b.Entity<Booking>(e =>
        {
            e.ToTable("bookings");

            e.HasKey(x => x.BookingId);

            e.Property(x => x.BookingId)
                .HasColumnName("booking_id");

            e.Property(x => x.BowlingLaneId)
                .HasColumnName("lane_id")
                .IsRequired();

            e.Property(x => x.UserId)
                .HasColumnName("user_id");

            e.Property(x => x.GuestFullName)
                .HasColumnName("guest_full_name");

            e.Property(x => x.GuestPhone)
                .HasColumnName("guest_phone");

            e.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            e.Property(x => x.TimeRange)
                .HasColumnName("time_range")
                .HasColumnType("tstzrange");

            e.HasOne(x => x.Lane)
                .WithMany()
                .HasForeignKey(x => x.BowlingLaneId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
