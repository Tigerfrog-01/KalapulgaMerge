using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
