using ClinicWeb.Models;
using ClinicWeb.Models.MBSatisfactionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
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
                .Where(m => m.CaseId == Case_ID /*1*/)
                .Select(x => new
                {
                    mRid = x.MrId,
                    就診日期 = x.ClinicList.Clinic.Date,
                    醫師 = x.ClinicList.Clinic.Doctor.Name,
                    時段 = x.ClinicList.Clinic.ClinicTime.Time,
                    科別 = x.ClinicList.Clinic.Doctor.Department,
                    診間 = x.ClinicList.Clinic.ClincRoom.Name,
                    PatientSatisfaction  = x.PatientSatisfaction,
                    DocSatisfaction = x.DocSatisfaction,
                    ClinicSatisfaction = x.ClinicSatisfaction,
                    SysSatisfaction = x.SysSatisfaction

                });
            return Json(MedicalRecords);
        }

        [HttpPost]
        public IActionResult Get_Review([FromBody] MedicalRecordsVM review)
        {

            //讀出更新的資料
            CasesMedicalRecords? record = _context.CasesMedicalRecords.Find(review.MrId);

            //更新資料
            if (record != null)
            {
                record.PatientSatisfaction = review.PatientSatisfaction;
                record.DocSatisfaction = review.DocSatisfaction;
                record.ClinicSatisfaction = review.ClinicSatisfaction;
                record.SysSatisfaction = review.SysSatisfaction;

                   
            }

            _context.SaveChanges();
            return Ok();


        }


    }
}
