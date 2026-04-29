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

                // Seed – testandmed
                entity.HasData(
                    new ShopItem { Id = 1, Name = "Punane müts", Type = ShopItemType.Hat, Price = 500, ImageUrl = "lib/defaultassets/image/shop1.png", IsAvailable = true },
                    new ShopItem { Id = 2, Name = "Sinine müts", Type = ShopItemType.Hat, Price = 500, ImageUrl = "lib/defaultassets/image/shop2.png", IsAvailable = true },
                    new ShopItem { Id = 3, Name = "Ümarad prillid", Type = ShopItemType.Glasses, Price = 750, ImageUrl = "lib/defaultassets/image/shop3.png", IsAvailable = true },
                    new ShopItem { Id = 4, Name = "Päikeseprillid", Type = ShopItemType.Glasses, Price = 750, ImageUrl = "lib/defaultassets/image/shop4.png", IsAvailable = true },
                    new ShopItem { Id = 5, Name = "Kuldne värv", Type = ShopItemType.Color, Price = 1000, ImageUrl = "", IsAvailable = true },
                    new ShopItem { Id = 6, Name = "Hõbedane värv", Type = ShopItemType.Color, Price = 1000, ImageUrl = "", IsAvailable = true }
                );
            });
        }
    }
}

