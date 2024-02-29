using ClinicWeb.Areas.Schedule.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Schedule.Controllers
{
    [Area("Schedule")]//路由設定123
    public class ClinicInfoController : Controller
    {
        private readonly ClinicSysContext _context;
        public ClinicInfoController(ClinicSysContext context)
        {
            _context = context;

        }
        public IActionResult ShowWeekSchedule() //匯出周表
        {

            var WeekSchedule = _context.ScheduleClinicSchedule
                .Select(x => new
                {
                    //id = x.ClinicInfoId,
                    醫師 = x.Doctor.Name,
                    星期 = GetDayOfWeek(x.Week),
                    時段 = x.Time.ClinicShifts,
                    科別 = x.Doctor.Department,
                    診間 = x.Room.Name
                    
                });
            
            return Json(WeekSchedule);
        }

        private static string GetDayOfWeek(int week)
        {
            switch (week)
            {
                case 0:
                    return "星期日";
                case 1:
                    return "星期一";
                case 2:
                    return "星期二";
                case 3:
                    return "星期三";
                case 4:
                    return "星期四";
                case 5:
                    return "星期五";
                case 6:
                    return "星期六";
                default:
                    return "未知";
            }
        }


    }
}
