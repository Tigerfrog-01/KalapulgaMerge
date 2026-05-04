namespace KalapulgaMerge.Core.Dto;

public class ShopItemDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Type { get; set; }
    public string ImageUrl { get; set; }
}
