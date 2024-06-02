using Entities;
using Microsoft.EntityFrameworkCore;

namespace Config
{
  public class Database : DbContext
  {
    public DbSet<Client> Clients { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseNpgsql("Host=localhost;Database=database;Username=user;Password=example");
    }
  }
}