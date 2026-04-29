namespace KalapulgaMerge.Core.Dto;

public record ShopItemDTO(
    int Id,
    string Name,
    string Description,
    decimal Price,
    string Type,        // "Hat"  "Glasses"  "Color"
    string ImageUrl
);
