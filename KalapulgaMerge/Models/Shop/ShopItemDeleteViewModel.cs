namespace KalapulgaMerge.Models.Shop
{
    public class ShopItemDeleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }

        public string? MainImagePath { get; set; }
    }
}