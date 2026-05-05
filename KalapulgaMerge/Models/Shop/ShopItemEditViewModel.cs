namespace KalapulgaMerge.Models.Shop
{
    public class ShopItemEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public List<ImageViewModel> ExistingImages { get; set; } = new();
        public List<IFormFile>? Files { get; set; }
    }
}
