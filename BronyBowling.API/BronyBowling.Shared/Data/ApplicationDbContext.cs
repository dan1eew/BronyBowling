using BronyBowling.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BronyBowling.Shared.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<BowlingCenter> BowlingCenters => Set<BowlingCenter>();
    public DbSet<Tariff> Tariffs => Set<Tariff>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // ---------- USERS ----------
        b.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.UserId);
            e.Property(x => x.UserId).HasColumnName("user_id");

            e.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(11).IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
            e.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        b.Entity<Admin>(e =>
        {
            e.ToTable("admins");
            e.HasKey(x => x.AdminId);
            e.Property(x => x.AdminId).HasColumnName("admin_id");
            e.Property(x => x.Login).HasColumnName("login").IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        // ---------- CENTERS ----------
        b.Entity<BowlingCenter>(e =>
        {
            e.ToTable("bowling_centers");
            e.HasKey(x => x.CenterId);
            e.HasOne(x => x.Tariff)
             .WithOne(t => t.Center)
             .HasForeignKey<Tariff>(t => t.CenterId);

            e.Property(x => x.CenterId).HasColumnName("center_id").IsRequired();
            e.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
            e.Property(x => x.City).HasColumnName("city").HasMaxLength(150).IsRequired();
            e.Property(x => x.Street).HasColumnName("street").HasMaxLength(150).IsRequired();
            e.Property(x => x.House).HasColumnName("house").HasMaxLength(20).IsRequired();

            e.Property(x => x.WeekdayOpen).HasColumnName("weekday_open").IsRequired();
            e.Property(x => x.WeekdayClose).HasColumnName("weekday_close").IsRequired();
            e.Property(x => x.WeekendOpen).HasColumnName("weekend_open").IsRequired();
            e.Property(x => x.WeekendClose).HasColumnName("weekend_close").IsRequired();

            e.Property(x => x.LanesCount).HasColumnName("lanes_count").IsRequired();
            e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        });

        // ---------- TARIFFS ----------
        b.Entity<Tariff>(e =>
        {
            e.ToTable("tariffs");
            e.HasKey(x => x.TariffId);
            e.HasOne(x => x.Center).WithOne(c => c.Tariff).HasForeignKey<Tariff>(x => x.CenterId)
             .OnDelete(DeleteBehavior.Cascade);

            e.Property(x => x.TariffId).HasColumnName("tariff_id");
            e.Property(x => x.CenterId).HasColumnName("center_id");
            e.Property(x => x.WeekdayPrice).HasColumnName("weekday_price").HasColumnType("numeric(10,2)");
            e.Property(x => x.WeekendPrice).HasColumnName("weekend_price").HasColumnType("numeric(10,2)");
        });

        b.Entity<Booking>(e =>
        {
            e.ToTable("bookings");
            e.HasKey(x => x.BookingId);

            e.Property(x => x.BookingId)
                .HasColumnName("booking_id");

            e.Property(x => x.CenterId)
                .HasColumnName("center_id")
                .IsRequired();

            e.Property(x => x.LaneNumber)
                .HasColumnName("lane_number")
                .IsRequired();

            e.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            e.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .IsRequired();

            e.Property(x => x.GuestName)
                .HasColumnName("guest_name")
                .HasMaxLength(150);

            e.Property(x => x.GuestPhone)
                .HasColumnName("guest_phone")
                .HasMaxLength(20);

            e.Property(x => x.BookingCode)
                .HasColumnName("booking_code")
                .HasMaxLength(4)
                .IsRequired();

            e.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(30)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.Property(x => x.UserId)
                .HasColumnName("user_id");

            e.HasOne(x => x.Center)
                .WithMany()
                .HasForeignKey(x => x.CenterId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        //// ---------- PAYMENTS ----------
        //b.Entity<Payment>(e =>
        //{
        //    e.ToTable("payments");
        //    e.HasKey(x => x.PaymentId);

        //    e.Property(x => x.Amount).HasColumnType("numeric(10,2)");
        //});
    }
}
