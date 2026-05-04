using KalapulgaMerge.ApplicationServices.Services;
using KalapulgaMerge.Core.Dto;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Data;
using KalapulgaMerge.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KalapulgaMerge.Controllers;

public class ShopController : Controller
{
    private readonly IShopService _shopService;
    private readonly IFilesServices _filesServices;
    private readonly KalapulkDbContext _context;

    public ShopController(IShopService shopService, IFilesServices filesServices, KalapulkDbContext context)
    {
        _shopService = shopService;
        _filesServices = filesServices;
        _context = context;
    }

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
            Images = x.Images.Select(f => new ImageViewModel
            {
                ImageID = f.ImageID,
                FilePath = f.FilePath,
                ShopItemID = f.ShopItemID
            }).ToList()
        });

        return View(viewModels);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShopItemCreateViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var dto = new ShopItemDTO
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                Type = vm.Type,
                Files = vm.Files 
            };

            var domain = await _shopService.Create(dto);

            if (domain != null)
            {
                _filesServices.FilesToApi(dto, domain);
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }
}

