using Microsoft.EntityFrameworkCore;
using serviceProfile.API.Models;

namespace serviceProfile.API.Data;
public class ProfileDbContext(DbContextOptions<ProfileDbContext> options)
           : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users_table");
            entity.HasKey(x => x.UserId);
        });
    }
}

