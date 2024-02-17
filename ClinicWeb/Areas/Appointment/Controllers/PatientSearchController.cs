﻿using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Appointment.Controllers
{
    [Area("Appointment")]
    public class PatientSearchController : Controller
    {
        private readonly ClinicSysContext _context;
        public PatientSearchController(ClinicSysContext context) { this._context = context; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("{area}/{controller}/{action}/{memberId}/{monthBefore}")]
        public IActionResult PatientApptTable(string memberId, string monthBefore)
        {
            // int, bool
            //不包含歷史資料，從此月開始 (預設)
            if (!Convert.ToBoolean(monthBefore))
            {
                string thisMonth = DateTime.Now.ToString("yyyy/mm");
                return Json(_context.ApptClinicList
                .Where(x => x.MemberId == Convert.ToInt32(memberId)
                && x.Clinic.Date.CompareTo(thisMonth) >= 0)
                .Include(x => x.Clinic.Doctor)
                .Include(x => x.Clinic.ClincRoom)
                .Include(x => x.Clinic.ClinicTime)
                .Select(x => new
                {
					clinicAppt_id = x.ClinicListId,
                    日期 = x.Clinic.Date,
                    時段 = x.Clinic.ClinicTime.ClinicShifts,
                    科別 = x.Clinic.Doctor.Department,
                    醫師名稱 = x.Clinic.Doctor.Name,
                    診號 = x.ClinicNumber,
                    退掛 = x.IsCancelled ? "是" : "否",
                    看診狀態 = x.PatientState.PatientStateName
                })
                );
            }
            //包含歷史資料
            return Json(_context.ApptClinicList
                .Where(x => x.MemberId == Convert.ToInt32(memberId))
                .Include(x => x.Clinic.Doctor)
                .Include(x => x.Clinic.ClincRoom)
                .Include(x => x.Clinic.ClinicTime)
                .Select(x => new
                {
					clinicAppt_id = x.ClinicListId,
                    日期 = x.Clinic.Date,
                    時段 = x.Clinic.ClinicTime.ClinicShifts,
                    科別 = x.Clinic.Doctor.Department,
                    醫師名稱 = x.Clinic.Doctor.Name,
                    診號 = x.ClinicNumber,
                    退掛 = x.IsCancelled ? "是" : "否",
                    看診狀態 = x.PatientState.PatientStateName
                })
                );
        }

		public JsonResult GET_ApptRecordOne(string id)
		{
			return Json(_context.ApptClinicList
				.Where(x => x.ClinicListId == Convert.ToInt32(id))
				.Select(x => new
				{
					clinicAppt_id = x.ClinicListId,
					member_id = x.MemberId,
					會員號碼 = x.Member.MemberNumber,
					診號 = x.ClinicNumber,
					姓名 = x.Member.Name,
					生日 = x.Member.BirthDate.ToString("yyyy/MM/dd"),
					性別 = x.Member.Gender ? "男" : "女",
					血型 = x.Member.BloodType,
					身分證字號 = x.Member.NationalId,
					退掛 = x.IsCancelled ? "是" : "否",
					看診狀態 = x.PatientState.PatientStateName
				})
				.FirstOrDefault()
				);
		}
	}
}
