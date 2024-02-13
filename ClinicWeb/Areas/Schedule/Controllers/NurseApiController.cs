using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Schedule.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Schedule.Controllers
{
    [Area("Schedule")]//路由設定123
    public class NurseApiController : Controller
    {

        private readonly ClinicSysContext _context;
        public NurseApiController(ClinicSysContext context)
        {
            _context = context;

        }

        public IActionResult SelectedNurseName() //匯入護士名稱到下拉選單
        {
            var nursenames = _context.MemberEmployeeList
                .Where(d => d.EmpType == "護士" && d.Quit == true)
                .Select(c => c.Name);
            return Json(nursenames);
        }

        [Route("{area}/{controller}/{action}/{year?}/{month?}")]
        public IActionResult ShowNurseSchedule(string year, string month) //匯出護士班表
        {
            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.Year.ToString();
            }
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.Month.ToString("D2");
            }

            var ShowNurseSchedule = _context.ScheduleNurseSchedule.Where(d => d.Clinic.Date.StartsWith($"{year}/{month}"))
                .Select(x => new
                {
                    //id = x.ClinicInfoId,
                    日期 = x.Clinic.Date,
                    護士 = x.Emp.Name,
                    醫師 = x.Clinic.Doctor.Name,
                    時段 = x.Clinic.ClinicTime.ClinicShifts,
                    科別 = x.Clinic.Doctor.Department,
                    診間 = x.Clinic.ClincRoom.Name,
                    掛號上限 = x.Clinic.RegistrationLimit
                });
            //.OrderBy(d=>d.日期);
            return Json(ShowNurseSchedule);



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



    }
}
