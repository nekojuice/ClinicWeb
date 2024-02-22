using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
	public class ClientPageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult services()
		{
			return View();
		}
	}
}
