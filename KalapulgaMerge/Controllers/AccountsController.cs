using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Filminurk.Controllers
{
    public class AccountsController : Controller
    {
        private static List<UserAccount> _users = new List<UserAccount>();

        public class UserAccount
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public IActionResult Index()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login");
            ViewBag.UserEmail = userEmail;
            return View();
        }

        [HttpGet] public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string email, string password)
        {
            
            if (_users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Account already exists!";
                return View();
            }

            _users.Add(new UserAccount { Email = email, Password = password });

            
            TempData["Success"] = "Registered successfully! Please login";
            return RedirectToAction("Login");
        }

        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ViewBag.Error = "Account does not exist";
                return View();
            }

            if (user.Password != password)
            {
                ViewBag.Error = "Wrong password!";
                return View();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            return RedirectToAction("Index");
        }
    }
}