using Microsoft.EntityFrameworkCore;
using BloxBin.Models;

namespace BloxBin.Data;

public class BloxBinContext : DbContext
{
    public BloxBinContext(DbContextOptions<BloxBinContext> options) : base(options) { }

    public DbSet<Bin> Bins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ViewKey> ViewKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Bins)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}