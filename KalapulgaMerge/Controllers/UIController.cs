using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers
{
    public class UIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
