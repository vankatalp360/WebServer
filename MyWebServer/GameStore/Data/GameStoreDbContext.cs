using System.Security.Authentication.ExtendedProtection;
using Microsoft.EntityFrameworkCore;
using MyWebServer.GameStore.Data.Models;

namespace MyWebServer.GameStore.Data
{
    public class GameStoreDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                .UseSqlServer(@"Server=LAPTOP-OB32IJPD\SQLEXPRESS;Database=GameStore;Integrated Security=True");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderGame> OrderGames { get; set; }
        public DbSet<UserGame> UserGames { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserGame>().HasKey(ug => new {ug.UserId, ug.GameId});
            builder.Entity<OrderGame>().HasKey(og => new {og.OrderId, og.GameId});

            builder
                .Entity<User>()
                .HasAlternateKey(u => u.Email);

            builder
                .Entity<UserGame>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.Games)
                .HasForeignKey(ug => ug.UserId);

            builder
                .Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany(g => g.Users)
                .HasForeignKey(ug => ug.GameId);

            builder
                .Entity<OrderGame>()
                .HasOne(og => og.Game)
                .WithMany(u => u.Orders)
                .HasForeignKey(og => og.GameId);

            builder
                .Entity<OrderGame>()
                .HasOne(og => og.Order)
                .WithMany(u => u.Games)
                .HasForeignKey(og => og.OrderId);
        }
    }
}