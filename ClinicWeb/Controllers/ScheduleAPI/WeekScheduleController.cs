using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Controllers.ScheduleAPI
{
    public class WeekScheduleController : Controller
    {
        private readonly ClinicSysContext _context;
        public WeekScheduleController(ClinicSysContext context)
        {
            _context = context;

        }
        [Route("{area}/{controller}/{action}/{departmentName}")]
        [HttpGet]
        public IActionResult Get_WeekSchedule(string departmentName)
        {
            //撈取資料到物件
            var result = _context.ScheduleClinicSchedule
                .Where(x => x.Doctor.Department == departmentName)
                .GroupBy(x => x.Week)
                .Select(x =>
                    new
                    {
                        Week = x.Key,
                        ClinicShifts = x.GroupBy(x => x.Time.ClinicShifts).Select(x =>
                        new
                        {
                            ClinicShift = x.Key,
                            Details = x.Select(x => new
                            {
                                doctor = x.Doctor.Name,
                                room = x.Room.Name
                            })
                        })
                    }
                );
            //將物件重組成動態json key
            var rebuildFormat = result.ToDictionary(
                w => w.Week, w => (object)w.ClinicShifts.ToDictionary(
                    w => w.ClinicShift, w => (object)w.Details
                ));

            return Json(rebuildFormat);
        }
        //用法: fetch("/Schedule/Api/Get_WeekSchedule/小兒科")
    }
}
