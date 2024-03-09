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
        [Route("{controller}/{action}/{doctorId}/{year}/{month}/{day}/{memId?}")]
        public IActionResult Get_ClinicApptInfo(string doctorId, string year, string month, string day, string? memId)
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
                    count = x.ApptClinicList.Where(y=>y.IsCancelled == false).Count(),
                    limit = x.RegistrationLimit,
                    isAppted = x.ApptClinicList.Any(y => y.MemberId == Convert.ToInt32(memId) && y.IsCancelled == false)
                })
                );
        }

        /// <summary>
        /// 撈取使用者登入資訊
        /// </summary>
        /// <returns>jsonobject</returns>
        //[Authorize(AuthenticationSchemes = "Angular")]
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
        [HttpPost]
        public async Task<IActionResult> Add_NewAppt([FromBody] NewAppt_DataBody vm)
        {
            if (vm.memid == null || vm.apptId == null)
            {
                return BadRequest();
            }


            Console.WriteLine($"{vm.apptId}, {vm.memid}");

            bool isDuplicate = _context.ApptClinicList
                .Where(x => x.ClinicId == vm.apptId && x.MemberId == vm.memid && x.IsCancelled == false)
                .Any();
            //非重複掛號時
            bool isVIP = false;

            if (!isDuplicate)
            {
                //計算診號邏輯
                //未有已掛號號碼，初始計算值
                int maxClinicNumber = Convert.ToBoolean(isVIP) ? -1 : 0;
                //如果有已掛號號碼，撈取最大值
                try
                {
                    maxClinicNumber = _context.ApptClinicList
                                    .Where(x => x.IsVip == Convert.ToBoolean(isVIP) && x.ClinicId == vm.apptId)
                                    .Select(x => x.ClinicNumber)
                                    .Max();
                }
                catch (Exception) { }
                //Console.WriteLine($"clinicId={clinicId} memberId={memberId} isVIP={isVIP}");
                try
                {
                    ApptClinicList newappt = new ApptClinicList
                    {
                        ClinicId = (int)vm.apptId,
                        MemberId = (int)vm.memid,
                        IsVip = Convert.ToBoolean(isVIP),
                        ClinicNumber = maxClinicNumber + 2,
                        PatientStateId = 8,
                        IsCancelled = false
                    };

                    _context.ApptClinicList.Add(newappt);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }

            return Ok();
        }
    }

    public class NewAppt_DataBody
    {
        public int? apptId { get; set; }
        public int? memid { get; set; }
    }
}
