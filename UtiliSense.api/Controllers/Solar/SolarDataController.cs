using Microsoft.AspNetCore.Mvc;

namespace UtiliSense.api.Controllers.Solar
{
    public class SolarDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
