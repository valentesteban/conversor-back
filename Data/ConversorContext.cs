using conversor.Entities;
using Microsoft.EntityFrameworkCore;

namespace conversor.Data;

public class ConversorContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Plan> Plan { get; set; }
    public DbSet<Coin> Coin { get; set; }
    public DbSet<CoinExchange> CoinExchange { get; set; }
    public DbSet<Auth> Auth { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=conversor.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>()
            .HasMany(e => e.Users)
            .WithOne(e => e.Plan)
            .HasForeignKey("PlanId")
            .IsRequired();

        modelBuilder.Entity<Auth>()
            .HasOne(e => e.User)
            .WithOne(e => e.Auth)
            .HasForeignKey<User>(e => e.AuthId);

        modelBuilder.Entity<Coin>().HasData(
            new Coin
            {
                Id = 1,
                Name = "Euro",
                Code = "EUR",
                Value = 1.2,
            },
            new Coin
            {
                Id = 2,
                Name = "Peso Argentino",
                Code = "ARS",
                Value = 0.50,

            },
            new Coin
            {
                Id = 3,
                Name = "Peso Colombiano",
                Code = "COP",
                Value = 0.10,
            });

        modelBuilder.Entity<Plan>().HasData(
            new Plan
            {
                Id = 1,
                Name = "Free",
                Price = 0,
                Limit = 10
            }, new Plan
            {
                Id = 2,
                Name = "Trial",
                Price = 10,
                Limit = 100
            },
            new Plan
            {
                Id = 3,
                Name = "Pro",
                Price = 20,
                Limit = -1
            });

        modelBuilder.Entity<Auth>().HasData(
            new Auth
            {
                Id = 1,
                Password = "admin",
                Role = "admin",
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                AuthId = 1,
                PlanId = 1,
            });
    }
}