using ClinicWeb.Areas.Appointment.Models;
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
    }
}
