using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Drugs.Controllers
{
	[Area("Drugs")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
