using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApiEF.Models.Data
{
    public partial class TestDbContext : DbContext
    {
        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artikal> Artikals { get; set; } = null!;
        public virtual DbSet<Komitent> Komitents { get; set; } = null!;
        public virtual DbSet<Racun> Racuns { get; set; } = null!;
        public virtual DbSet<RacunStavka> RacunStavkas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artikal>(entity =>
            {
                entity.ToTable("Artikal");

                entity.Property(e => e.NazivArtikla)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Komitent>(entity =>
            {
                entity.ToTable("Komitent");

                entity.Property(e => e.NazivKomitenta)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Racun>(entity =>
            {
                entity.ToTable("Racun");

                entity.Property(e => e.BrojDokumenta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Datum).HasColumnType("date");

                entity.Property(e => e.Total).HasColumnType("decimal(10, 3)");

                entity.HasOne(d => d.Komitent)
                    .WithMany(p => p.Racuns)
                    .HasForeignKey(d => d.KomitentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KomitentId");
            });

            modelBuilder.Entity<RacunStavka>(entity =>
            {
                entity.ToTable("RacunStavka");

                entity.Property(e => e.Cena).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Kolicina).HasColumnType("decimal(10, 3)");

                entity.HasOne(d => d.Artikal)
                    .WithMany(p => p.RacunStavkas)
                    .HasForeignKey(d => d.ArtikalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArtikalId");

                entity.HasOne(d => d.Racun)
                    .WithMany(p => p.RacunStavkas)
                    .HasForeignKey(d => d.RacunId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RacunId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer("Data Source=BANKOMW;Initial Catalog=TestDb;User ID=sa;Password=password;Integrated Security=False;");
            base.OnConfiguring(optionBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
