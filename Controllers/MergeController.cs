using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers
{
    public class MergeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
