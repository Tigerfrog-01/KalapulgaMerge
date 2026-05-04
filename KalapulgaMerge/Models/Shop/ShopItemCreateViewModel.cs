using KalapulgaMerge.Core.Dto;

namespace KalapulgaMerge.Models.Shop
{
    public class ShopItemCreateViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty; // "Hat", "Glasses", "Color"

        public List<IFormFile>? Files { get; set; }
        public IEnumerable<FileToApiDTO>? FileToApiDTOs { get; set; } = new List<FileToApiDTO>();
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
    }
}