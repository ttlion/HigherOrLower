﻿// <auto-generated />
using System;
using HigherOrLower.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HigherOrLower.Migrations
{
    [DbContext(typeof(HigherOrLowerDbContext))]
    partial class HigherOrLowerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HigherOrLower.Entities.Cards.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("HigherOrLower.Entities.Games.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DisplayId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("HigherOrLower.Entities.Games.GameCard", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("DrawOrder")
                        .HasColumnType("int");

                    b.HasKey("GameId", "CardId");

                    b.HasIndex("CardId");

                    b.ToTable("GameCards");
                });

            modelBuilder.Entity("HigherOrLower.Entities.Games.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IsCurrentMove")
                        .HasColumnType("int");

                    b.Property<int>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("int");

                    b.Property<int>("OrderInGame")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("HigherOrLower.Entities.Games.GameCard", b =>
                {
                    b.HasOne("HigherOrLower.Entities.Cards.Card", null)
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HigherOrLower.Entities.Games.Game", null)
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HigherOrLower.Entities.Games.Player", b =>
                {
                    b.HasOne("HigherOrLower.Entities.Games.Game", null)
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
