using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class TemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
