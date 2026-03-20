
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
       
            return View(Index);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


    }
}


     
     
