namespace KalapulgaMerge.Core.Domain;

public class ShopItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ShopItemType Type { get; set; }
    public bool IsAvailable { get; set; } = true;



}
public enum ShopItemType
{
    Hat = 0,
    Glasses = 1,
    Color = 2
}
