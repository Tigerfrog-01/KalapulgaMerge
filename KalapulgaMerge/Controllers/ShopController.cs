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
        try
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
        catch
        {
            ViewBag.ShopNotice = "Pood kasutab hetkel vaikimisi esemeid, sest andmebaasi kataloogi ei saanud laadida.";
            return View(GetFallbackShopItems());
        }
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
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id < 0)
        {
            var fallbackItem = GetFallbackShopItems().FirstOrDefault(x => x.Id == id);
            if (fallbackItem == null)
            {
                return NotFound();
            }

            return View(fallbackItem);
        }

        var dto = await _shopService.GetShopItemById(id);

        if (dto == null)
        {
            return NotFound();
        }

        var vm = new ShopItemViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Type = dto.Type,
            Images = dto.Images.Select(f => new ImageViewModel
            {
                ImageID = f.ImageID,
                FilePath = f.FilePath,
                ShopItemID = f.ShopItemID
            }).ToList()
        };

        return View(vm);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _shopService.GetShopItemById(id);
        if (dto == null) return NotFound();

        var vm = new ShopItemEditViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Type = dto.Type,
            ExistingImages = dto.Images.Select(f => new ImageViewModel
            {
                ImageID = f.ImageID,
                FilePath = f.FilePath,
                ShopItemID = f.ShopItemID
            }).ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ShopItemEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var dto = new ShopItemDTO
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            Type = vm.Type,
            Files = vm.Files
        };

        var domain = await _shopService.Edit(dto);

        if (domain == null)
        {
            return NotFound();
        }

        if (vm.Files != null && vm.Files.Any())
        {
            _filesServices.FilesToApi(dto, domain);
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpGet] 
    public async Task<IActionResult> RemoveImage(Guid imageId, int shopItemId)
    {
        var dto = new FileToApiDTO { ImageID = imageId };
        var image = await _filesServices.RemoveImageFromApi(dto);

        if (image == null)
        {
            return RedirectToAction(nameof(Edit), new { id = shopItemId });
        }

        return RedirectToAction(nameof(Edit), new { id = shopItemId });
    }
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var dto = await _shopService.GetShopItemById(id);
        if (dto == null) return NotFound();

        var vm = new ShopItemDeleteViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Type = dto.Type,
            Price = dto.Price,
            MainImagePath = dto.Images.FirstOrDefault()?.FilePath
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dto = await _shopService.GetShopItemById(id);

        if (dto != null)
        {
            // 1. Kustutame failid kettalt ja FilesToApi tabelist
            foreach (var img in dto.Images)
            {
                await _filesServices.RemoveImageFromApi(img);
            }

            // 2. Kustutame eseme ShopItems tabelist
            await _shopService.Delete(id);
        }

        return RedirectToAction(nameof(Index));
    }

    private static List<ShopItemViewModel> GetFallbackShopItems()
    {
        return new List<ShopItemViewModel>
        {
            new()
            {
                Id = -1,
                Name = "Classic Fish Stick",
                Description = "Vaikimisi Merge tegelase alus.",
                Price = 0,
                Type = "Color",
                Images = new List<ImageViewModel>
                {
                    new() { FilePath = "lib/defaultassets/image/fishstick_base.png" }
                }
            },
            new()
            {
                Id = -2,
                Name = "Shop Hat",
                Description = "Kerge kosmeetiline ese Merge tegelasele.",
                Price = 25,
                Type = "Hat",
                Images = new List<ImageViewModel>
                {
                    new() { FilePath = "lib/defaultassets/image/shop1.png" }
                }
            },
            new()
            {
                Id = -3,
                Name = "Bright Glasses",
                Description = "Lisab mängule rohkem iseloomu.",
                Price = 50,
                Type = "Glasses",
                Images = new List<ImageViewModel>
                {
                    new() { FilePath = "lib/defaultassets/image/shop2.png" }
                }
            },
            new()
            {
                Id = -4,
                Name = "Golden Style",
                Description = "Silmapaistev valik suuremate skooride jaoks.",
                Price = 100,
                Type = "Color",
                Images = new List<ImageViewModel>
                {
                    new() { FilePath = "lib/defaultassets/image/shop3.png" }
                }
            }
        };
    }
}

