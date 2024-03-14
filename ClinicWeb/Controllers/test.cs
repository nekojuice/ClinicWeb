using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
