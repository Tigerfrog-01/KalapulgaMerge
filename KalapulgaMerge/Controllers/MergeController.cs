using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KalapulgaMerge.Controllers
{
    public class MergeController : Controller
    {
        private const string DefaultUnlocked = "[\"stick\",\"head-none\",\"body-none\"]";
        private const string DefaultEquipment = "{\"colour\":\"stick\",\"head\":\"head-none\",\"body\":\"body-none\"}";
        private readonly KalapulkDbContext _context;

        public MergeController(KalapulkDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!TryGetCurrentUserId(out _))
            {
                return RedirectToAction("Login", "Accounts");
            }

            ViewBag.PlayerName = CurrentPlayerName();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> State()
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var playerName = CurrentPlayerName();

            try
            {
                var state = await _context.MergePlayerStates.FirstOrDefaultAsync(x => x.UserAccountId == userId);

                if (state == null)
                {
                    return Json(DefaultState(playerName));
                }

                return Json(new
                {
                    playerName = state.PlayerName,
                    coins = state.Coins,
                    unlockedItems = state.UnlockedItemsJson,
                    activeEquipment = state.ActiveEquipmentJson,
                    theme = state.Theme
                });
            }
            catch
            {
                return Json(DefaultState(playerName));
            }
        }

        [HttpPost]
        public async Task<IActionResult> State([FromBody] MergeStateRequest request)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var playerName = CurrentPlayerName();

            try
            {
                var state = await _context.MergePlayerStates.FirstOrDefaultAsync(x => x.UserAccountId == userId);

                if (state == null)
                {
                    state = new MergePlayerState { UserAccountId = userId };
                    _context.MergePlayerStates.Add(state);
                }

                state.PlayerName = playerName;
                state.Coins = request.Coins;
                state.UnlockedItemsJson = string.IsNullOrWhiteSpace(request.UnlockedItems) ? DefaultUnlocked : request.UnlockedItems;
                state.ActiveEquipmentJson = string.IsNullOrWhiteSpace(request.ActiveEquipment) ? DefaultEquipment : request.ActiveEquipment;
                state.Theme = string.IsNullOrWhiteSpace(request.Theme) ? "original" : request.Theme;
                state.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Json(new { saved = true });
            }
            catch
            {
                return Json(new { saved = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Score([FromBody] MergeScoreRequest request)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var playerName = CurrentPlayerName();
            var mode = string.IsNullOrWhiteSpace(request.Mode) ? "normal" : request.Mode;

            try
            {
                var score = await _context.MergeScores.FirstOrDefaultAsync(x => x.UserAccountId == userId && x.Mode == mode);

                if (score == null)
                {
                    score = new MergeScore
                    {
                        UserAccountId = userId,
                        PlayerName = playerName,
                        Mode = mode,
                        Score = request.Score
                    };

                    _context.MergeScores.Add(score);
                }
                else
                {
                    score.PlayerName = playerName;

                    if (request.Score > score.Score)
                    {
                        score.Score = request.Score;
                        score.CreatedAt = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();
                return Json(new { saved = true });
            }
            catch
            {
                return Json(new { saved = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Leaderboard()
        {
            try
            {
                var scores = await _context.MergeScores
                    .OrderByDescending(x => x.Score)
                    .ThenBy(x => x.CreatedAt)
                    .Take(10)
                    .Select(x => new { name = x.PlayerName, score = x.Score })
                    .ToListAsync();

                return Json(scores);
            }
            catch
            {
                return Json(Array.Empty<object>());
            }
        }

        private static object DefaultState(string playerName)
        {
            return new
            {
                playerName,
                coins = 0,
                unlockedItems = DefaultUnlocked,
                activeEquipment = DefaultEquipment,
                theme = "original"
            };
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            var userIdText = HttpContext.Session.GetString("UserId");
            return int.TryParse(userIdText, out userId);
        }

        private string CurrentPlayerName()
        {
            var name = HttpContext.Session.GetString("UserName");

            if (!string.IsNullOrWhiteSpace(name))
            {
                return name.Trim();
            }

            var email = HttpContext.Session.GetString("UserEmail");
            return string.IsNullOrWhiteSpace(email) ? "Player" : email.Trim();
        }
    }

    public class MergeStateRequest
    {
        public int Coins { get; set; }
        public string UnlockedItems { get; set; } = string.Empty;
        public string ActiveEquipment { get; set; } = string.Empty;
        public string Theme { get; set; } = "original";
    }

    public class MergeScoreRequest
    {
        public int Score { get; set; }
        public string Mode { get; set; } = "normal";
    }
}
