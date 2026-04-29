namespace KalapulgaMerge.Models;

public class ShopItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Type { get; set; } = string.Empty;   // "Hat" = 0| "Glasses" = 1 | "Color" = 2
    public string ImageUrl { get; set; } = string.Empty;
}
