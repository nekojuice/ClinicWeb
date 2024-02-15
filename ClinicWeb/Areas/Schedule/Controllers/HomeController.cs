using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Schedule.Controllers
{
	[Area("Schedule")]
	public class HomeController : Controller
	{
		public IActionResult DoctorSchedule()
		{
			return View();
		}

        public IActionResult NurseSchedule()
        {
            return View();
        }
    }
}
