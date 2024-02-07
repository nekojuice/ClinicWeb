//using ClinicWeb.Models;
using ClinicWeb.Areas.Schedule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicWeb.Areas.Schedule.Controllers
{
    [Area("Schedule")]//路由設定123
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

       
        public IActionResult ShowThismonthSchedule()
        {
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString("D2"); 

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
