using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Controllers
{
    public class MBSatisfactionController : Controller
    { 
        private readonly ClinicSysContext _context;
        public MBSatisfactionController(ClinicSysContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }


        /// 撈取使用者登入資訊
        public IActionResult Get_MemberStatus()
        {
            var user = HttpContext.User;
            var resultObject = new
            {
                MemberNumber = user.Claims.FirstOrDefault(c => c.Type == "MemberNumber")?.Value,
                MemberName = user.Claims.FirstOrDefault(c => c.Type == "MemberName")?.Value,
                MemberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value
            };
            return Json(resultObject);
        }

        //撈出使用者就診紀錄
        [HttpPost]
        public IActionResult Get_MedicalRecords()
        {
            var user = HttpContext.User;
            var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;
            var Case_ID = _context.CasesMainCase
                .Where(m => m.MemberId == Convert.ToInt32(memberID))
                .Select(m => m.CaseId)
                .FirstOrDefault();

            var MedicalRecords = _context.CasesMedicalRecords
                .Include(m => m.ClinicList.Clinic)
                .Where(m => m.CaseId == 1 /*Case_ID*/)
                .Select(x => new
                {
                    recordid = x.MrId,
                    就診日期 = x.ClinicList.Clinic.Date,
                    醫師 = x.ClinicList.Clinic.Doctor.Name,
                    時段 = x.ClinicList.Clinic.ClinicTime.Time,
                    科別 = x.ClinicList.Clinic.Doctor.Department,
                    診間 = x.ClinicList.Clinic.ClincRoom.Name,

                });
            return Json(MedicalRecords);
        }

    }
}
