using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicWeb.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class ApptSysController : Controller
    {
        private ClinicSysContext _context;
        public ApptSysController(ClinicSysContext context) { _context = context; }

        public IActionResult Index()
        {
            var result = (from tSchedule in _context.ScheduleClinicInfo select tSchedule.Date.Substring(0, 7)).Distinct();

            ViewBag.Date = new SelectList(_context.ScheduleClinicInfo
                .Select(tSchedule => tSchedule.Date.Substring(0, 7))
                .Distinct());
			//_context.ScheduleClinicInfo

			//.GroupBy(x => x.Date)

			//.Select(x => new
			//{
			//    日期資料 = x.First().Date.Substring(0, 7),
			//    日期 = x.First().Date.Substring(0, 7),
			//}), "日期資料", "日期");
			ViewBag.Department = new SelectList(_context.MemberEmployeeList
				.Select(x => x.Department)
				.Distinct());

			return View();
        }

        [Route("{area}/{controller}/{action}/{year}/{month}")]
        public IActionResult ClinicInfo(string year, string month)
        {
            var apptableMonth = from x in _context.ScheduleClinicInfo
                         where x.Date.StartsWith($"{year}/{month}")
                         select x;
			if (apptableMonth == null)
            {
                return NotFound();
            }
            else
            {
                return PartialView("_ClinicInfoPartial", apptableMonth);
            }
        }

        [Route("{area}/{controller}/{action}/{year}/{month}")]
        //[HttpPost]
        public JsonResult Jsontest(string year, string month)
        {
            return Json(_context.ScheduleClinicInfo
                .Where(x => x.Date.StartsWith($"{year}/{month}"))
                .Include(x => x.Doctor)
                .Include(x => x.ClincRoom)
                .Include(x => x.ClinicTime)
                .Include(x => x.ApptClinicList)
                .Select(x => new
                {
                    id = x.ClinicInfoId,
                    日期 = x.Date,
                    時段 = x.ClinicTime.ClinicShifts,
                    科別 = x.Doctor.Department,
                    醫師名稱 = x.Doctor.Name,
                    上限人數 = x.RegistrationLimit,
                    預約人數 = x.ApptClinicList.Count,
                }));
        }
    }
}
