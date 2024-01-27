using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            return View();
        }

        [Route("{area}/{controller}/{action}/{month}")]
        public IActionResult ClinicInfoPartial(string month)
        {
            var result = from x in _context.ScheduleClinicInfo
                         where x.Date.StartsWith(month)
                         select x;
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return PartialView("_ClinicInfoPartial", result);
            }
        }
    }
}
