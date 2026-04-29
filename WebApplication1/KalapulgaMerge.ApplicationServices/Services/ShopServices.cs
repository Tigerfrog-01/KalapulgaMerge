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
            .Select(x => new ShopItemDTO(
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.Type.ToString(),
                x.ImageUrl
            ))
            .ToListAsync();
    }
}
