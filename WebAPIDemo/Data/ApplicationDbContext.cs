using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Models;

namespace WebAPIDemo.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions options):base(options)
    {

    }

    public DbSet<Shirt> Shirts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<Shirt>().HasData(
            new Shirt { ShirtId = 1, Brand = "H&M", Color = "Blue", Size = 6, Gender = "women", Price = 10.99 },
            new Shirt { ShirtId = 2, Brand = "H&M", Color = "Red", Size = 8, Gender = "men", Price = 12.99 },
            new Shirt { ShirtId = 3, Brand = "AE", Color = "Green", Size = 10, Gender = "women", Price = 14.99 },
            new Shirt { ShirtId = 4, Brand = "AE", Color = "Yellow", Size = 12, Gender = "men", Price = 16.99 }
        );
    }
}