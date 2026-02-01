using serviceLogin.API.Models;
using Microsoft.EntityFrameworkCore;

namespace serviceLogin.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users_table");

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .HasColumnName("UserId");

            entity.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(11)
                .IsRequired();

            entity.Property(e => e.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(e => e.FullName)
                .HasColumnName("FullName")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.BirthDate)
                .HasColumnName("BirthDate")
                .IsRequired();

            entity.Property(e => e.CreateAt)
                .HasColumnName("CreatedAt")
                .IsRequired();
        });
    }
}