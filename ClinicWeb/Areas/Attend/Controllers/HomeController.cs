using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Attend.Controllers
{
	[Area("Attend")]
	public class HomeController : Controller
	{
		private ClinicSysContext _context;
		public HomeController(ClinicSysContext context) { _context = context; }
		public IActionResult Index()
		{
			var result = _context.AttendanceTAttendance.ToList();
			ViewBag.result = result;
			return View();
		}
		[Route("{area}/{controller}/{action}/{id}")]
		public JsonResult check(string id)
		{
			return Json(_context.AttendanceTAttendance
				.Where(x => x.FEmployeeId == Convert.ToInt32(id))
				.Select(x => new
				{

					上班時間 = x.FCheckInTime,
					下班時間 = x.FCheckOutTime,
					上班日期 = x.FWorkDate,
					上班狀態 = x.FAttendanceCis,
					下班狀態 = x.FAttendanceCos
				}));
		}
	}
}
