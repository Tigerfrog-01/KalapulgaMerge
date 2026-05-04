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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Type).HasConversion<int>();

                //////// Seed – testandmed
                //////entity.HasData(
                //////    new ShopItem { Id = 1, Name = "Punane müts", Type = ShopItemType.Hat, Price = 500, ImageUrl = "lib/defaultassets/image/shop1.png", IsAvailable = true },
                new ShopItem { Id = 2, Name = "Sinine müts", Type = ShopItemType.Hat, Price = 500 };
            }
                 
                );
            }
        }
    }


