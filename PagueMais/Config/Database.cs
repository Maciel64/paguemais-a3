using Entities;
using Microsoft.EntityFrameworkCore;

namespace Config
{
  public class Database : DbContext
  {
    public DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseNpgsql("Host=localhost;Database=database;Username=user;Password=example");
    }
  }
}