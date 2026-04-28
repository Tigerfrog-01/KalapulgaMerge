using KalapulgaMerge.Models;
using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers
{
    public class ShopController : Controller
    {
        private List<ShopItemViewModel> GetShopItems() => new List<ShopItemViewModel> {
            new ShopItemViewModel { Id = 1, Name = "Kõrvad", Price = 670, ImagePath = "/lib/defaultassets/image/shop1.png" },
            new ShopItemViewModel { Id = 2, Name = "Müts",   Price = 430, ImagePath = "/lib/defaultassets/image/shop2.png" },
        };

        private int GetBalance()
        {
            if (TempData.ContainsKey("balance"))
            {
                TempData.Keep("balance");
                return (int)TempData["balance"];
            }
            return 1200;
        }

        private List<int> GetOwnedItems()
        {
            if (TempData.ContainsKey("owned"))
            {
                TempData.Keep("owned");
                return ((int[])TempData["owned"]).ToList();
            }
            return new List<int> { 2 };
        }

        public IActionResult Index()
        {
            var model = new ShopViewModel
            {
                UserBalance = GetBalance(),
                OwnedItemIds = GetOwnedItems(),
                Items = GetShopItems()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Buy(int itemId)
        {
            var items = GetShopItems();
            var item = items.FirstOrDefault(i => i.Id == itemId);
            var owned = GetOwnedItems();
            var balance = GetBalance();

            if (item == null || owned.Contains(itemId) || balance < item.Price)
                return RedirectToAction("Index");

            owned.Add(itemId);
            TempData["balance"] = balance - item.Price;
            TempData["owned"] = owned.ToArray();

            return RedirectToAction("Index");
        }
    } 
}