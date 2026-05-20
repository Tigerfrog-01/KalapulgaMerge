using System.Diagnostics;
using KalapulgaMerge.Data;
using KalapulgaMerge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KalapulgaMerge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KalapulkDbContext _context;

        public HomeController(ILogger<HomeController> logger, KalapulkDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Coins = 0;
            ViewBag.IsLoggedIn = false;
            ViewBag.PlayerName = "Guest";
            ViewBag.ProfilePic = null;

            var userIdText = HttpContext.Session.GetString("UserId");

            if (int.TryParse(userIdText, out var userId))
            {
                ViewBag.IsLoggedIn = true;
                ViewBag.PlayerName = HttpContext.Session.GetString("UserName") ?? "Player";
                ViewBag.ProfilePic = HttpContext.Session.GetString("UserProfilePic");

                try
                {
                    var state = await _context.MergePlayerStates.FirstOrDefaultAsync(x => x.UserAccountId == userId);
                    ViewBag.Coins = state?.Coins ?? 0;
                }
                catch
                {
                    ViewBag.Coins = 0;
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
