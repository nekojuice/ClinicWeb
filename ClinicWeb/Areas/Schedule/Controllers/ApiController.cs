//using ClinicWeb.Models;
using ClinicWeb.Areas.Schedule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Schedule.Controllers
{
    [Area("Schedule")]//路由設定
    public class ApiController : Controller
    {

       
        private readonly ClinicSysContext _context;
        public ApiController(ClinicSysContext context)
        {
            _context = context;

        }


        public IActionResult SelectedDoctorName()
        {
            var doctornames = _context.MemberEmployeeList.Where(d => d.EmpType == "醫生").Select(c => c.Name);
            return Json(doctornames);
        }

        
        public IActionResult ShowThismonthSchedule(string year, string month)
        {
            var ThismonthSchedule = _context.ScheduleClinicInfo.Where(d => d.Date.StartsWith($"{year}/{month}"))
                .Select(x => new
                {
                    //id = x.ClinicInfoId,
                    日期 = x.Date,
                    醫師 = x.Doctor.Name,
                    時段 = x.ClinicTime.ClinicShifts,
                    科別 = x.Doctor.Department,
                    診間 = x.ClincRoom.Name,
                    掛號上限 = x.RegistrationLimit
                });
            return Json(ThismonthSchedule); 
        }

    }
}
