using Microsoft.EntityFrameworkCore;
using serviceLogin.API.Models;

namespace serviceLogin.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users_table");

            entity.HasKey(x => x.UserId);

            entity.Property(x => x.PhoneNumber)
                .HasMaxLength(11)
                .IsRequired();

            entity.HasIndex(x => x.PhoneNumber)
                .IsUnique();

            entity.Property(x => x.PasswordHash)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
