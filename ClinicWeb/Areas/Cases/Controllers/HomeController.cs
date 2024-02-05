using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Cases.Controllers
{
	[Area("Cases")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
        public IActionResult main()
        {
            return View();
        }
    }
}
