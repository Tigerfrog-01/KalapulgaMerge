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
            if (!TryGetCurrentUserId(out _))
            {
                return Unauthorized();
            }

            var playerName = CurrentPlayerName();
            var saveKey = CurrentAccountKey();

            try
            {
                var state = await FindPlayerState(saveKey, playerName);

                if (state == null)
                {
                    return Json(DefaultState(playerName));
                }

                return Json(new
                {
                    playerName,
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
            if (!TryGetCurrentUserId(out _))
            {
                return Unauthorized();
            }

            var playerName = CurrentPlayerName();
            var saveKey = CurrentAccountKey();

            try
            {
                var state = await FindPlayerState(saveKey, playerName);

                if (state == null)
                {
                    state = new MergePlayerState { PlayerName = saveKey };
                    _context.MergePlayerStates.Add(state);
                }

                state.PlayerName = saveKey;
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
        public async Task<IActionResult> Coins([FromBody] MergeCoinsRequest request)
        {
            if (!TryGetCurrentUserId(out _))
            {
                return Unauthorized();
            }

            var playerName = CurrentPlayerName();
            var saveKey = CurrentAccountKey();

            try
            {
                var state = await FindPlayerState(saveKey, playerName);

                if (state == null)
                {
                    state = new MergePlayerState
                    {
                        PlayerName = saveKey,
                        UnlockedItemsJson = DefaultUnlocked,
                        ActiveEquipmentJson = DefaultEquipment,
                        Theme = "original"
                    };
                    _context.MergePlayerStates.Add(state);
                }

                state.PlayerName = saveKey;
                state.Coins = request.Coins < 0 ? 0 : request.Coins;
                state.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Json(new { saved = true, coins = state.Coins });
            }
            catch
            {
                return Json(new { saved = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Score([FromBody] MergeScoreRequest request)
        {
            if (!TryGetCurrentUserId(out _))
            {
                return Unauthorized();
            }

            var saveKey = CurrentAccountKey();
            var mode = string.IsNullOrWhiteSpace(request.Mode) ? "normal" : request.Mode;

            try
            {
                var score = await _context.MergeScores.FirstOrDefaultAsync(x => x.PlayerName == saveKey && x.Mode == mode);

                if (score == null)
                {
                    score = new MergeScore
                    {
                        PlayerName = saveKey,
                        Mode = mode,
                        Score = request.Score
                    };

                    _context.MergeScores.Add(score);
                }
                else
                {
                    score.PlayerName = saveKey;

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
                    .ToListAsync();

                var userIds = scores
                    .Select(x => int.TryParse(x.PlayerName, out var id) ? id : 0)
                    .Where(x => x > 0)
                    .Distinct()
                    .ToList();

                var names = await _context.UserAccounts
                    .Where(x => userIds.Contains(x.Id))
                    .ToDictionaryAsync(x => x.Id, x => x.Name);

                return Json(scores.Select(x => new { name = ScoreName(x.PlayerName, names), score = x.Score }));
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

        private async Task<MergePlayerState?> FindPlayerState(string saveKey, string playerName)
        {
            var state = await _context.MergePlayerStates.FirstOrDefaultAsync(x => x.PlayerName == saveKey);

            if (state == null && playerName != saveKey)
            {
                state = await _context.MergePlayerStates.FirstOrDefaultAsync(x => x.PlayerName == playerName);
            }

            return state;
        }

        private static string ScoreName(string key, Dictionary<int, string> names)
        {
            if (int.TryParse(key, out var id) && names.TryGetValue(id, out var name))
            {
                return name;
            }

            return key;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            var userIdText = HttpContext.Session.GetString("UserId");
            return int.TryParse(userIdText, out userId);
        }

        private string CurrentAccountKey()
        {
            var userIdText = HttpContext.Session.GetString("UserId");
            return string.IsNullOrWhiteSpace(userIdText) ? CurrentPlayerName() : userIdText.Trim();
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

    public class MergeCoinsRequest
    {
        public int Coins { get; set; }
    }

    public class MergeScoreRequest
    {
        public int Score { get; set; }
        public string Mode { get; set; } = "normal";
    }
}
