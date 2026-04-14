using Microsoft.AspNetCore.Mvc;
using KalapulgaMerge.Models;

namespace KalapulgaMerge.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Leaderboard()
        {
            var topPlayers = new List<LeaderboardViewModel>
            {
                new LeaderboardViewModel{Username = "Mängija1", HighestLevel = 42},
                new LeaderboardViewModel{Username = "Mängija2", HighestLevel = 38},
                new LeaderboardViewModel{Username = "Mängija3", HighestLevel = 61},
            };

            var sorted = topPlayers
                .OrderByDescending(p => p.HighestLevel)
                .ToList();

            return View(sorted);

        }

    }
}