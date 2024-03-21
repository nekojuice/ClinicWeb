using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Room.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
