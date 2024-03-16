using ClinicWeb.Areas.ClinicRoomSys.Models.ViewModels;
using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
                    Gender = (bool)x.Member.Gender ? "男" : "女",
                    FirstVisitDay = x.FirstvisitDate.ToString("yyyy-MM-dd"),
                    BirthDate = x.Member.BirthDate.HasValue ? x.Member.BirthDate.Value.ToString("yyyy-MM-dd") : null,
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
            if (!int.TryParse(id, out int caseId))
            {
                return Json(new { error = "Invalid id parameter" });
            }

            return Json(_context.CasesMedicalRecords
                .Include(x=>x.ClinicList.Clinic)
                .Where(x => x.CaseId == caseId)
                .Select(x => new
                {
                    date = DateTime.Parse(x.ClinicList.Clinic.Date).ToString("yyyy-MM-dd"),
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
                    PrescriptionID= x.PrescriptionId,
                    PrescriptionDate = x.PrescriptionDate.ToString("yyyy-MM-dd"),
                    Dispensing = x.Dispensing,
                })
                );
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCase(int id, [FromBody] CasesMainCase caseUpdateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            var caseToUpdate = await _context.CasesMainCase.FirstOrDefaultAsync(x => x.CaseId == id);
            if (caseToUpdate == null)
            {
                return NotFound(new { success = false, message = "Case not found" });
            }

            caseToUpdate.Height = caseUpdateModel.Height;
            caseToUpdate.Weight = caseUpdateModel.Weight;
            caseToUpdate.PastHistory = caseUpdateModel.PastHistory;
            caseToUpdate.AllergyRecord = caseUpdateModel.AllergyRecord;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Record updated successfully" });
        }

        [HttpPost]
        public IActionResult AddMedicalRecord([FromBody] CasesMedicalRecords record)
        {
            // 在這裡處理表單提交的資料，例如將資料儲存到資料庫中
            _context.CasesMedicalRecords.Add(record);
            _context.SaveChanges();

            // 返回適當的回應
            return Ok();
        }

        [HttpPost]
        public IActionResult AddTestReport([FromBody] CasesTestReport record)
        {
            // 在這裡處理表單提交的資料，例如將資料儲存到資料庫中
            _context.CasesTestReport.Add(record);
            _context.SaveChanges();

            // 返回適當的回應
            return Ok();
        }
    }
 }
