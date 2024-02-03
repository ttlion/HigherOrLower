﻿using HigherOrLower.Entities;
using Microsoft.EntityFrameworkCore;

namespace HigherOrLower.Infrastructure.Database
{
    public class HigherOrLowerDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameCard> GameCards { get; set; }

        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=HigherOrLower;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCard(modelBuilder);
            ConfigureGame(modelBuilder);
            ConfigureGameCard(modelBuilder);
            ConfigurePlayer(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureCard(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<Card>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Card>()
                .Property(x => x.Value)
                .IsRequired();
        }

        private static void ConfigureGame(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<Game>()
                .Property(x => x.DisplayId)
                .IsRequired();

            modelBuilder.Entity<Game>()
                .Property(x => x.IsFinished)
                .IsRequired();
        }

        private static void ConfigureGameCard(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCard>()
                .HasKey(x => new { x.GameId, x.CardId });

            modelBuilder.Entity<GameCard>()
                .HasOne(typeof(Game))
                .WithMany()
                .HasForeignKey(nameof(GameCard.GameId));

            modelBuilder.Entity<GameCard>()
                .HasOne(typeof(Card))
                .WithMany()
                .HasForeignKey(nameof(GameCard.CardId));

            modelBuilder.Entity<GameCard>()
                .Property(x => x.DrawOrder)
                .IsRequired();
        }

        private static void ConfigurePlayer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<Player>()
                .HasOne(typeof(Game))
                .WithMany()
                .HasForeignKey(nameof(Player.GameId));

            modelBuilder.Entity<Player>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Player>()
                .Property(x => x.Score)
                .IsRequired();

            modelBuilder.Entity<Player>()
                .Property(x => x.OrderInGame)
                .IsRequired();

            modelBuilder.Entity<Player>()
                .Property(x => x.IsCurrentMove)
                .IsRequired();
        }
    }
}