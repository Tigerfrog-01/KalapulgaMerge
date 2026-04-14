using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers
{
    public class KeepMeSecure : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
