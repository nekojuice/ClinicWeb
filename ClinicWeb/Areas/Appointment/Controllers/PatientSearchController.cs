using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Appointment.Controllers
{
	[Area("Appointment")]
	public class PatientSearchController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
