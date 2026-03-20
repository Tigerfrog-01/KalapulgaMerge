using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers
{
    public class GameAdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
