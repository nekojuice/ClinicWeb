using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
	public class ClientPageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult shop()
		{
			return View();
		}
		public IActionResult about()
		{
			return View();
		}
		public IActionResult services()
		{
			return View();
		}
		public IActionResult blog()
		{
			return View();
		}
		public IActionResult contact()
		{
			return View();
		}
        public IActionResult Login()
        {
            return View("~/Views/ClientPage/Login/Login.cshtml");
        }
        public IActionResult Example()
        {
            return View();
        }
		public IActionResult Essence() 
		{
			return View();
		}

    }
}
