using ClinicWeb.Areas.Schedule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Controllers
{
	public class FClinicScheduleController : Controller
    {


        public IActionResult FClinicSchedule()
		{
			return View();
		}



    }

}
