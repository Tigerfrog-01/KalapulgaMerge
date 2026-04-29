using KalapulgaMerge.ApplicationServices.Services;
using KalapulgaMerge.Core.Dto;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Models;
using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers;

public class ShopController : Controller
{
    private readonly IShopService _shopService;
    private readonly IPreviewService _previewService;

    public ShopController(IShopService shopService, IPreviewService previewService)
    {
        _shopService = shopService;
        _previewService = previewService;
    }


    //  Kataloogi kuvamine
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
    //Preview
    [HttpGet]
    public async Task<IActionResult> Preview(int itemId)
    {
        var result = await _previewService.GetPreviewAsync(new PreviewRequestDto(itemId));

        if (!result.Success)
            return NotFound(new { result.Message });

        return Json(result);
    }
}
