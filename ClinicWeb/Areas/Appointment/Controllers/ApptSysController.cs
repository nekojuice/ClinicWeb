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

        public JsonResult MemberData(string id)
        {
            return Json(_context.MemberMemberList
                .Where(x => x.MemberId == Convert.ToInt32(id))
                .Select(x => new
                {
                    id = x.MemberId,
                    會員號碼 = x.MemberNumber,
                    身分證字號 = x.NationalId,
                    姓名 = x.Name,
                    性別 = x.Gender ? "男" : "女",
                    生日 = x.BirthDate.ToString("yyyy-MM-dd"),
                    血型 = x.BloodType,
                    聯絡地址 = x.ContactAddress,
                    連絡電話 = x.Phone,
                    電子郵件 = x.MemEmail,
                    緊急聯絡人 = x.IceName,
                    緊急電話 = x.IceNumber
                })
                );
        }

        public IActionResult test1()
        {
            return View();
        }
    }
}
