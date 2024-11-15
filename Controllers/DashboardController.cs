using Microsoft.AspNetCore.Mvc;

namespace MockPruebaTecnica.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
