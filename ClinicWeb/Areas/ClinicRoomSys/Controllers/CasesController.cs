using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;

namespace ClinicWeb.Areas.ClinicRoomSys.Controllers
{
    [Area("ClinicRoomSys")]

    public class CasesController : Controller
    {

        private readonly ClinicSysContext _context;
        public CasesController(ClinicSysContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpPost]
        public JsonResult GM(string id) //GETMainCase
        {
            return Json(_context.CasesMainCase
                .Include(x => x.Member)
                .Where(x => x.MemberId == Convert.ToInt32(id))
                .Select(x => new
                {
                    CasesID = x.CaseId,
                    MemberNumber = x.Member.MemberNumber,
                    NationalId = x.Member.NationalId,
                    Name = x.Member.Name,
                    Gender = x.Member.Gender ? "男" : "女",
                    FirstVisitDay = x.FirstvisitDate.ToString("yyyy-MM-dd"),
                    BirthDate = x.Member.BirthDate.ToString("yyyy-MM-dd"),
                    BloodType = x.Member.BloodType,
                    Height = x.Height,
                    Weight = x.Weight,
                    PastHistory = x.PastHistory,
                    AllergyRecord = x.AllergyRecord,

                })
                .FirstOrDefault()
                );
        }
        [HttpPost]
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
                    ReportDate = x.ReportDate.ToString("yyyy-MM-dd"),
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
                    PrescriptionID= x.PrescriptionId,
                    PrescriptionDate = x.PrescriptionDate.ToString("yyyy-MM-dd"),
                    Dispensing = x.Dispensing,
                })
                );
        }
    }
}
