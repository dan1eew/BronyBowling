using BronyBowling.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BronyBowling.Shared.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BowlingLane> BowlingLanes => Set<BowlingLane>();
    public DbSet<BowlingCenter> BowlingCenters => Set<BowlingCenter>();
    public DbSet<Tariff> Tariffs => Set<Tariff>();
    public DbSet<Admin> Admins => Set<Admin>();
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
                .HasMaxLength(11)
                .IsRequired();

            e.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(256)
                .IsRequired();

            e.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50)
                .IsRequired();

            e.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(50)
                .IsRequired();

            e.Property(x => x.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(50);

            e.Property(x => x.BirthDate)
                .HasColumnName("birth_date");

            e.Property(x => x.City)
                .HasColumnName("city")
                .HasMaxLength(100);

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        });

        // ---------- LANES ----------
        b.Entity<BowlingLane>(e =>
        {
            e.ToTable("bowling_lanes");

            e.HasKey(x => x.BowlingLaneId);

            e.Property(x => x.BowlingLaneId)
                .HasColumnName("lane_id");

            e.Property(x => x.Number)
                .HasColumnName("number")
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();
        });

        // ---------- BOOKINGS ----------
        b.Entity<Booking>(e =>
        {
            e.ToTable("bookings");

            e.HasKey(x => x.BookingId);

            e.Property(x => x.BookingId)
                .HasColumnName("booking_id");

            e.Property(x => x.UserId)
                .HasColumnName("user_id");

            e.Property(x => x.GuestFullName)
                .HasColumnName("guest_full_name");

            e.Property(x => x.GuestPhone)
                .HasColumnName("guest_phone");

            e.Property(x => x.TimeRange) .HasColumnType("tstzrange");

            e.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");

            e.HasOne(x => x.Lane)
                     .WithMany()
                     .HasForeignKey(x => x.BowlingLaneId)
                     .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- BOWLING_CENTER ----------
        b.Entity<BowlingCenter>(e =>
        {
            e.ToTable("bowling_centers");

            e.HasKey(x => x.BowlingCenterId);

            e.Property(x => x.BowlingCenterId)
                .HasColumnName("bowling_center_id");

            e.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.City)
                .HasColumnName("city")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.Street)
                .HasColumnName("street")
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.House)
                .HasColumnName("house")
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.WorkingHours)
                .HasColumnName("working_hours")
                .HasMaxLength(50)
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();
        });

        // ---------- BOWLING_LANE ----------
        b.Entity<BowlingLane>(e =>
        {
            e.ToTable("bowling_lanes");

            e.HasKey(x => x.BowlingLaneId);

            e.Property(x => x.BowlingLaneId)
                .HasColumnName("bowling_lane_id");

            e.Property(x => x.BowlingCenterId)
                .HasColumnName("bowling_center_id");

            e.Property(x => x.Number)
                .HasColumnName("number")
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

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
                .HasColumnName("bowling_lane_id");

            e.Property(x => x.DayOfWeek)
                .HasColumnName("day_of_week")
                .IsRequired();

            e.Property(x => x.PricePerHour)
                .HasColumnName("price_per_hour")
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            e.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            e.HasOne(x => x.Lane)
                .WithMany(l => l.Tariffs)
                .HasForeignKey(x => x.BowlingLaneId)
                .OnDelete(DeleteBehavior.Cascade);
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
                .HasMaxLength(50)
                .IsRequired();

            e.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(256)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            e.HasIndex(x => x.Login)
                .IsUnique();
        });
    }
}
