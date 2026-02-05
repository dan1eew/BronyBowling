using BronyBowling.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BronyBowling.Shared.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder b)
    {
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
    }
}
