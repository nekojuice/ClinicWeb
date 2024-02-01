using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Schedule.Controllers
{
	[Area("Schedule")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
