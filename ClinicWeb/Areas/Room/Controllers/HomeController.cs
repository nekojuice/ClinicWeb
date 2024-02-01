using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Room.Controllers
{
	[Area("Room")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
