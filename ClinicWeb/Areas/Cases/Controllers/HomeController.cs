using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Cases.Controllers
{
	[Area("Cases")]
	public class HomeController : Controller
	{
		private ClinicSysContext _context;
		public HomeController(ClinicSysContext context) { _context = context; }
		public IActionResult Index()
		{
			var result = (from tSchedule in _context.ScheduleClinicInfo select tSchedule.Date.Substring(0, 7)).Distinct();

			ViewBag.Date = new SelectList(_context.ScheduleClinicInfo
				.Select(tSchedule => tSchedule.Date.Substring(0, 7))
				.Distinct());

			ViewBag.Department = new SelectList(_context.MemberEmployeeList
				.Select(x => x.Department)
				.Distinct());

			ViewBag.ClinicShifts = new SelectList(_context.ScheduleClinicTime.Select(x => x.ClinicShifts));

			return View();
		}
		[Route("{area}/{controller}/{action}")]
		public JsonResult maindata()
		{
			return Json(_context.CasesMainCase
				.Include(x => x.Member)
				.Select(x => new
				{
					id=x.CaseId,
					姓名=x.Member.Name,
					身分證字號=x.Member.NationalId,
					性別=x.Member.Gender,
					//id = x.ClinicInfoId,
					//日期 = x.Date,
					//時段 = x.ClinicTime.ClinicShifts,
					//科別 = x.Doctor.Department,
					//醫師名稱 = x.Doctor.Name,
					//上限人數 = x.RegistrationLimit,
					//預約人數 = x.ApptClinicList.Count,
				}));
		}
		public IActionResult main()
        {
            return View();
        }
	}
}
