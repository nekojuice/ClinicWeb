using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Shop.Controllers
{
	[Area("Shop")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
