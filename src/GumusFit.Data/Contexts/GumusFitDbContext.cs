using Microsoft.EntityFrameworkCore;
using GumusFit.Domain.Entities;

namespace GumusFit.Data.Contexts;

public class GumusFitDbContext : DbContext
{
    public GumusFitDbContext(DbContextOptions<GumusFitDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<CalorieEntry> CalorieEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<CalorieEntry>()
            .HasOne(c => c.User)
            .WithMany(u => u.CalorieEntries)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
