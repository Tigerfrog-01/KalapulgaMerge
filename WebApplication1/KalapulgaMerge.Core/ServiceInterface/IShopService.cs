using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Core.Dto;

namespace KalapulgaMerge.Core.ServiceInterface;

public interface IShopService
{

    Task<IEnumerable<ShopItemDTO>> GetCatalogAsync();
    Task<ShopItem> Create(ShopItemDTO dto);
}
