using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Attend.Controllers
{
	[Area("Attend")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
