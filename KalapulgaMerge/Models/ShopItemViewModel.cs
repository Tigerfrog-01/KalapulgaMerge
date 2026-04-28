namespace KalapulgaMerge.Models
{
    public class ShopItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
    }

    public class ShopViewModel
    {
        public List<ShopItemViewModel> Items { get; set; }
        public int UserBalance { get; set; }
        public List<int> OwnedItemIds { get; set; }
    }
}