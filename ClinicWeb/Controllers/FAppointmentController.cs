using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    public class FAppointmentController : Controller
    {
        private readonly ClinicSysContext _context;
        public FAppointmentController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult FAppointment()
        {
            return View();
        }

        /// <summary>
        /// 依科別撈取所有醫師
        /// </summary>
        /// <param name="deptName">科別 (string)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{controller}/{action}/{deptName}")]
        public IActionResult Get_DeptDoctorInfo(string deptName)
        {
            return Json(_context.MemberEmployeeList
                .Where(x => x.Department == deptName)
                .Select(x => new
                {
                    empId = x.EmpId,
                    docName = x.Name,
                    empPhoto = x.EmpPhoto
                })
                .Distinct()
                );
        }

        /// <summary>
        /// 依醫師id撈取 該時間點 的掛號資訊
        /// </summary>
        /// <param name="doctorId">醫師id (string)</param>
        /// <param name="year">年 (string)</param>
        /// <param name="month">月 (string)</param>
        /// <param name="day">日 (string)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{controller}/{action}/{doctorId}/{year}/{month}/{day}")]
        public IActionResult Get_ClinicApptInfo(string doctorId, string year, string month, string day)
        {
            DateTime start = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            DateTime end = start.AddDays(6);

            return Json(_context.ScheduleClinicInfo
                .Where(x => x.DoctorId == Convert.ToInt32(doctorId)
                && x.Date.CompareTo(start.ToString("yyyy/MM/dd")) >= 0
                && x.Date.CompareTo(end.ToString("yyyy/MM/dd")) <= 0)
                .Select(x => new
                {
                    clinicInfoId = x.ClinicInfoId,
                    date = x.Date,
                    shift = x.ClinicTime.ClinicShifts,
                    count = x.ApptClinicList.Count(),
                    limit = x.RegistrationLimit,
                })
                );
        }

        /// <summary>
        /// 撈取使用者登入資訊
        /// </summary>
        /// <returns>jsonobject</returns>
        [Authorize(AuthenticationSchemes = "Angular")]
        public IActionResult Get_MemberStatus()
        {
            var user = HttpContext.User;

            var memberNumber = user.Claims.FirstOrDefault(c => c.Type == "MemberNumber")?.Value;
            var memberName = user.Claims.FirstOrDefault(c => c.Type == "MemberName")?.Value;
            var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

            if (memberID == null)
            {
                return NotFound();
            }
            var resultObject = new
            {
                MemberNumber = memberNumber,
                MemberName = memberName,
                MemberID = memberID
            };
            return Json(resultObject);
        }

        //[Authorize(Policy = "frontendpolicy")]
        public IActionResult Add_NewAppt()
        {
            var user = HttpContext.User;
            if (user == null)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
