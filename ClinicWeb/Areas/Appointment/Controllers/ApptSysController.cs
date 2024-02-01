using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Appointment.Controllers
{
	public class ApptSysController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
