using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Appointment.Controllers
{
	[Area("Appointment")]
	public class ArrivalController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
