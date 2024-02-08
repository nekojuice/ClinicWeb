using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Appointment.Controllers
{
    [Area("Appointment")]
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

            ViewBag.Department = new SelectList(_context.MemberEmployeeList
                .Select(x => x.Department)
                .Distinct());

            ViewBag.ClinicShifts = new SelectList(_context.ScheduleClinicTime.Select(x => x.ClinicShifts));

            return View();
        }

        //[Route("{area}/{controller}/{action}/{year}/{month}")]
        //public IActionResult ClinicInfo(string year, string month)
        //{
        //    var apptableMonth = from x in _context.ScheduleClinicInfo
        //                        where x.Date.StartsWith($"{year}/{month}")
        //                        select x;
        //    if (apptableMonth == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return PartialView("_ClinicInfoPartial", apptableMonth);
        //    }
        //}

        [Route("{area}/{controller}/{action}/{year}/{month}")]
        //[HttpPost]
        public JsonResult ClinicInfo(string year, string month)
        {
            return Json(_context.ScheduleClinicInfo
                .Where(x => x.Date.StartsWith($"{year}/{month}"))
                .Include(x => x.Doctor)
                .Include(x => x.ClincRoom)
                .Include(x => x.ClinicTime)
                .Include(x => x.ApptClinicList)
                .Select(x => new
                {
                    id = x.ClinicInfoId,
                    日期 = x.Date,
                    時段 = x.ClinicTime.ClinicShifts,
                    科別 = x.Doctor.Department,
                    醫師名稱 = x.Doctor.Name,
                    上限人數 = x.RegistrationLimit,
                    預約人數 = x.ApptClinicList.Count,
                }));
        }

        [Route("{area}/{controller}/{action}/{id}")]
        //[HttpPost]
        public JsonResult ApptRecord(string id)
        {
            return Json(_context.ApptClinicList
                .Where(x => x.ClinicId == Convert.ToInt32(id))
                .Include(x => x.Member)
                .Select(x => new
                {
                    clinic_id = x.ClinicId,
                    member_id = x.MemberId,
                    診號 = x.ClinicNumber,
                    姓名 = x.Member.Name,
                    生日 = x.Member.BirthDate.ToString("yyyy/MM/dd"),
                    性別 = x.Member.Gender ? "男" : "女",
                    身分證字號 = x.Member.NationalId,
                    退掛 = x.IsCancelled ? "是" : "否",
                    看診狀態 = x.PatientState.PatientStateName
                }));
        }

        [Route("{area}/{controller}/{action}/{nationalId}")]
        //[HttpPost]
        public JsonResult MemberSnap(string nationalId)
        {
            return Json(_context.MemberMemberList
                .Where(x => x.NationalId.Contains(nationalId))
                .Select(x => new
                {
                    id = x.MemberId,
                    身分證字號 = x.NationalId,
                    姓名 = x.Name,
                    性別 = x.Gender ? "男" : "女",
                    生日 = x.BirthDate.ToString("yyyy-MM-dd")
                })
                .Take(5)
                );
        }

        [HttpPost]
        public JsonResult MemberData(string id)
        {
            return Json(_context.MemberMemberList
                .Where(x => x.MemberId == Convert.ToInt32(id))
                .Select(x => new
                {
                    id = x.MemberId,
                    MemberNumber = x.MemberNumber,
                    NationalId = x.NationalId,
                    Name = x.Name,
                    Gender = x.Gender ? "男" : "女",
                    BirthDate = x.BirthDate.ToString("yyyy-MM-dd"),
                    BloodType = x.BloodType,
                    ContactAddress = x.ContactAddress,
                    Phone = x.Phone,
                    MemEmail = x.MemEmail,
                    IceName = x.IceName,
                    IceNumber = x.IceNumber
                })
                );
        }
        [Route("{area}/{controller}/{action}/{clinicId}/{memberId}/{isVIP}")]
        [HttpPost]
        public IActionResult AddAppt(string clinicId, string memberId, string isVIP)
        {
            bool isDuplicate = _context.ApptClinicList
                .Where(x => x.ClinicId == Convert.ToInt32(clinicId) && x.MemberId == Convert.ToInt32(memberId))
                .Any();
            if (!isDuplicate)
            {
                int maxClinicNumber = Convert.ToBoolean(isVIP) ? -1 : 0;

                try
                {
                    maxClinicNumber += _context.ApptClinicList
                                    .Where(x => x.IsVip == Convert.ToBoolean(isVIP) && x.ClinicId == Convert.ToInt32(clinicId))
                                    .Select(x => x.ClinicNumber)
                                    .Max();
                }
                catch (Exception)
                {
                }



                Console.WriteLine($"clinicId={clinicId} memberId={memberId} isVIP={isVIP}");
                ApptClinicList newappt = new ApptClinicList
                {
                    ClinicId = Convert.ToInt32(clinicId),
                    MemberId = Convert.ToInt32(memberId),
                    IsVip = Convert.ToBoolean(isVIP),
                    ClinicNumber = maxClinicNumber + 2,
                    PatientStateId = 8,
                    IsCancelled = false
                };

                _context.ApptClinicList.Add(newappt);
                _context.SaveChanges();
            }


            return Content(isDuplicate.ToString());
        }

        public IActionResult test1()
        {
            return View();
        }
    }
}
