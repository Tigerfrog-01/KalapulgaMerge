using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Core.Dto;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Data;
using Microsoft.EntityFrameworkCore;

namespace KalapulgaMerge.ApplicationServices.Services;

public class ShopServices : IShopService
{
    private readonly KalapulkDbContext _context;

    public ShopServices(KalapulkDbContext context)
    {
        _context = context;
    }

public async Task<IEnumerable<ShopItemDTO>> GetCatalogAsync()
{
    return await _context.ShopItems
        .Where(x => x.IsAvailable)
        .OrderBy(x => x.Type)
        .ThenBy(x => x.Price)
        .Select(x => new ShopItemDTO // No parentheses here
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            Type = x.Type.ToString(),
            ImageUrl = x.ImageUrl
        })
        .ToListAsync();
}

    public async Task<ShopItem> Create(ShopItemDTO dto)
    {
        var domain = new ShopItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Type = Enum.Parse<ShopItemType>(dto.Type),
            ImageUrl = dto.ImageUrl,
            IsAvailable = true
        };
        _context.ShopItems.Add(domain);
        await _context.SaveChangesAsync();

        return domain;
    }
}
