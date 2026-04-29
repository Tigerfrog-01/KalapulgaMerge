using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Models;
using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers;

public class ShopController : Controller
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    // GET /Shop
    // Task 1 – Kataloogi kuvamine
    public async Task<IActionResult> Index()
    {
        var items = await _shopService.GetCatalogAsync();

        var viewModels = items.Select(x => new ShopItemViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            Type = x.Type,
            ImageUrl = x.ImageUrl
        });

        return View(viewModels);
    }
}
