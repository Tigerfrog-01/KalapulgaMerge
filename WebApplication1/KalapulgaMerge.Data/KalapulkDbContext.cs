using KalapulgaMerge.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalapulgaMerge.Data
{
    public class KalapulkDbContext : DbContext
    {
        public KalapulkDbContext(DbContextOptions<KalapulkDbContext> options) : base(options) { }
        public DbSet<FileToApi> FilesToApi { get; set; }

        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<CaseItem> Cases { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<MergePlayerState> MergePlayerStates { get; set; }
        public DbSet<MergeScore> MergeScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Type).HasConversion<int>();
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ProfilePicPath).HasMaxLength(300);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2");
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<MergePlayerState>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserAccountId).IsRequired();
                entity.Property(e => e.PlayerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UnlockedItemsJson).IsRequired();
                entity.Property(e => e.ActiveEquipmentJson).IsRequired();
                entity.Property(e => e.Theme).IsRequired().HasMaxLength(40);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");
                entity.HasIndex(e => e.UserAccountId).IsUnique();
            });

            modelBuilder.Entity<MergeScore>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserAccountId).IsRequired();
                entity.Property(e => e.PlayerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Mode).IsRequired().HasMaxLength(40);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2");
                entity.HasIndex(e => new { e.UserAccountId, e.Mode }).IsUnique();
            });
        }
    }
}
