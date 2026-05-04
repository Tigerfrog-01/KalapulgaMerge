using KalapulgaMerge.Models.Shop;

namespace KalapulgaMerge.Models.Shop
{
    public class ShopItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public List<IFormFile>? Files { get; set; }

        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
    }


}

