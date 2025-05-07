using Microsoft.AspNetCore.Mvc;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
