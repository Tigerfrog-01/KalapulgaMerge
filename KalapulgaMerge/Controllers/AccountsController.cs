using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KalapulgaMerge.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly KalapulkDbContext _context;

        public AccountsController(IWebHostEnvironment env, KalapulkDbContext context)
        {
            _env = env;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.UserEmail = user.Email;
            ViewBag.UserName = user.Name;
            ViewBag.ProfilePic = user.ProfilePicPath;

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            name = string.IsNullOrWhiteSpace(name) ? email?.Split('@').FirstOrDefault() ?? "User" : name.Trim();
            email = email?.Trim() ?? "";
            password ??= "";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required";
                return View();
            }

            try
            {
                if (await _context.UserAccounts.AnyAsync(u => u.Email == email))
                {
                    ViewBag.Error = "Account already exists";
                    return View();
                }

                var user = new UserAccount
                {
                    Name = name,
                    Email = email,
                    Password = password
                };

                _context.UserAccounts.Add(user);
                await _context.SaveChangesAsync();
            }
            catch
            {
                ViewBag.Error = "Database is not available";
                return View();
            }

            TempData["Success"] = "Registered successfully, please login";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (IsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            email = email?.Trim() ?? "";
            password ??= "";

            UserAccount? user = null;

            try
            {
                if (email == "admin" || email == "admin@admin")
                {
                    user = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Name == "admin" && u.Email == "admin@admin");
                }

                user ??= await _context.UserAccounts.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    user = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Name == email);
                }
            }
            catch
            {
                ViewBag.Error = "Database is not available";
                return View();
            }

            if (user == null)
            {
                ViewBag.Error = "Account does not exist";
                return View();
            }

            if (user.Password != password)
            {
                ViewBag.Error = "Wrong password";
                return View();
            }

            SaveSession(user);
            return RedirectToAction("Index");
        }

        public IActionResult SwitchAdmin()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Users(string search)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            var query = _context.UserAccounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.Name.Contains(search));
            }

            ViewBag.Search = search;

            try
            {
                var users = await query.OrderBy(u => u.Name).ToListAsync();
                return View(users);
            }
            catch
            {
                ViewBag.Error = "Database is not available";
                return View(new List<UserAccount>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            UserAccount? user;

            try
            {
                user = await _context.UserAccounts.FindAsync(id);
            }
            catch
            {
                return RedirectToAction("Users");
            }

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, string name, string email, string password)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            UserAccount? user;

            try
            {
                user = await _context.UserAccounts.FindAsync(id);
            }
            catch
            {
                ViewBag.Error = "Database is not available";
                return View(new UserAccount { Id = id, Name = name ?? "", Email = email ?? "" });
            }

            if (user == null)
            {
                return NotFound();
            }

            name = name?.Trim() ?? "";
            email = email?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Name and email are required";
                return View(user);
            }

            bool emailTaken;

            try
            {
                emailTaken = await _context.UserAccounts.AnyAsync(u => u.Email == email && u.Id != id);
            }
            catch
            {
                ViewBag.Error = "Database is not available";
                return View(user);
            }

            if (emailTaken)
            {
                ViewBag.Error = "Email already exists";
                return View(user);
            }

            user.Name = name;
            user.Email = email;

            if (!string.IsNullOrWhiteSpace(password))
            {
                user.Password = password;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                ViewBag.Error = "Database is not available";
                return View(user);
            }

            if (HttpContext.Session.GetString("UserId") == user.Id.ToString())
            {
                SaveSession(user);
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login");
            }

            UserAccount? user;

            try
            {
                user = await _context.UserAccounts.FindAsync(id);
            }
            catch
            {
                return RedirectToAction("Users");
            }

            if (user == null)
            {
                return RedirectToAction("Users");
            }

            _context.UserAccounts.Remove(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return RedirectToAction("Users");
            }

            if (HttpContext.Session.GetString("UserId") == user.Id.ToString())
            {
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPicture(IFormFile photo)
        {
            var user = await GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream);

                user.ProfilePicPath = "/uploads/" + fileName;
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("UserProfilePic", user.ProfilePicPath);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private async Task<UserAccount?> GetCurrentUser()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (!int.TryParse(userIdText, out var userId))
            {
                return null;
            }

            try
            {
                return await _context.UserAccounts.FindAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        private void SaveSession(UserAccount user)
        {
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("IsAdmin", IsAdminAccount(user) ? "true" : "false");

            if (!string.IsNullOrWhiteSpace(user.ProfilePicPath))
            {
                HttpContext.Session.SetString("UserProfilePic", user.ProfilePicPath);
            }
            else
            {
                HttpContext.Session.Remove("UserProfilePic");
            }
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserName") == "admin" && HttpContext.Session.GetString("UserEmail") == "admin@admin";
        }

        private bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserId"));
        }

        private static bool IsAdminAccount(UserAccount user)
        {
            return user.Name == "admin" && user.Email == "admin@admin" && user.Password == "admin";
        }
    }
}
