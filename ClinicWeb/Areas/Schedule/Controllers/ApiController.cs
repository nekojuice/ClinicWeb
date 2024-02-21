//using ClinicWeb.Models;
//using AspNetCore;
using ClinicWeb.Areas.Schedule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
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


        public IActionResult SelectedDoctorName() //匯入醫師名稱到下拉選單
        {
            var doctornames = _context.MemberEmployeeList
                .Where(d => d.EmpType == "醫生" && d.Quit == true)
                .Select(c => c.Name);
            return Json(doctornames);
        }

        [Route("{area}/{controller}/{action}/{year?}/{month?}")]
        public IActionResult ShowThismonthSchedule(string year, string month) //匯出當月醫師班表
        {
            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.Year.ToString();
            }
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.Month.ToString("D2");
            }

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
            //.OrderBy(d=>d.日期);
            return Json(ThismonthSchedule);
        }

        //匯出月份
        public IActionResult GetMonth()
        {
            var month = _context.ScheduleClinicInfo.Select(d => d.Date.Substring(0, 7)).Distinct(); //2023/12
            return Json(month);

        }

        //匯出班表時段
        public IActionResult GetShifts()
        {
            var month = _context.ScheduleClinicInfo
                .Select(d => new
                {
                    d.ClinicTime.ClinicTimeId,
                    d.ClinicTime.ClinicShifts
                })
                .OrderBy(o => o.ClinicTimeId).Distinct();

            return Json(month);

        }

        //匯出科別
        public IActionResult GetDepartment()
        {
            var department = _context.ScheduleClinicInfo
                .Select(d => d.Doctor.Department).Distinct();


            return Json(department);

        }

        //nkj20240221 interrupt
        [Route("{area}/{controller}/{action}/{departmentName}")]
        [HttpGet]
        public IActionResult Get_WeekSchedule(string departmentName)
        {
            var result = _context.ScheduleClinicSchedule
                .Where(x => x.Doctor.Department == departmentName)
                .GroupBy(x => new { x.Week })
                .Select(x =>
                    new
                    {
                        星期 = x.Key,
                        排班 = x.GroupBy(x => x.Time.ClinicShifts).Select(x => new
                        {
                            時段 = x.Key,
                            診間 = new
                            {
                                doctor = x.Select(x => x.Doctor.Name),
                                room = x.Select(x => x.Room.Name)
                            }
                        })
                    }
                );

            return Json(result);
        }
        //fetch("/Schedule/Api/Get_WeekSchedule/小兒科")
    }
}
