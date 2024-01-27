using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
