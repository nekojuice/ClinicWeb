using ClinicWeb.Areas.Drugs.Controllers;
using ClinicWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class MBRecordInfoController : Controller
    {
        private readonly ClinicSysContext _context;
        public MBRecordInfoController(ClinicSysContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Get_MemberName()
        {
            var user = HttpContext.User;
            var MemberName = user.Claims.FirstOrDefault(c => c.Type == "MemberName")?.Value;
            if(!MemberName.IsNullOrEmpty())
            {
                return Json(MemberName);
            }
            
            return View();
        }
        public IActionResult Get_Memberdata()
        {
            var user = HttpContext.User;
            var MemberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

            if (!MemberID.ToString().IsNullOrEmpty())
            {
                return Json(_context.CasesMainCase
                    .Where(x => x.MemberId == Convert.ToInt32(MemberID))
                    .FirstOrDefault(x => x.CaseId != null)
                    );
            }
            return View();
        }
        
        public JsonResult GRD(string id) //GETRECORD
        {
            return Json(_context.CasesMedicalRecords
                .Where(x => x.CaseId == Convert.ToInt32(id))
                .Select(x => new
                {
                    RecordID = x.MrId,
                    BloodPresure = x.Bp,
                    Pulse = x.Pulse,
                    BodyTemparture = x.Bt,
                    ChiefComplaint = x.Cc,
                    Disposal = x.Disposal,
                    Prescribe = x.Prescribe,
                })
                );
        }
        [HttpPost]
        public JsonResult GRT(string id) //GETREPORT
        {
            return Json(_context.CasesTestReport
                .Where(x => x.CaseId == Convert.ToInt32(id))
                .Select(x => new
                {
                    ReportID = x.ReportId,
                    TestName = x.TestName,
                    TestDate = x.TestDate.ToString("yyyy-MM-dd"),
                    ReportDate = x.ReportDate.HasValue ? x.ReportDate.Value.ToString("yyyy-MM-dd") : null,
                    Result = x.Result,
                })
                );
        }
        [HttpPost]
        public JsonResult GP(string id) //GETPRESCRIPTION
        {
            return Json(_context.CasesPrescription
                .Where(x => x.CaseId == Convert.ToInt32(id))
                .Select(x => new
                {
                    PrescriptionID = x.PrescriptionId,
                    PrescriptionDate = x.PrescriptionDate.ToString("yyyy-MM-dd"),
                    Dispensing = x.Dispensing,
                })
                );
        }

        [HttpPost]
        public JsonResult GPL(string id) //GETPRESCRIPTION
        {
            return Json(_context.CasesPrescriptionlist
                .Include(x => x.Drug)
                .Where(x => x.PrescriptionId == Convert.ToInt32(id))
                .Select(x => new
                {
                    DrugId = x.DrugId,
                    Name = x.Drug.FDrugName,
                    Days =x.Days,
                    total=x.TotalAmount,
                })
                );
        }

    }
}
