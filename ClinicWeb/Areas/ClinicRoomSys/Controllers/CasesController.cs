using ClinicWeb.Areas.ClinicRoomSys.Models.ViewModels;
using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        //查詢大隊長
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
        public async Task<JsonResult> GRD(string id) //GETRECORD
        {
            if (!int.TryParse(id, out int caseId))
            {
                return Json(new { error = "Invalid id parameter" });
            }

            return Json(await _context.CasesMedicalRecords
                .Include(x => x.ClinicList.Clinic)
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
                .ToListAsync()
            );
        }
        [HttpPost]
        public async Task<JsonResult> GRT(string id) //GETREPORT
        {
            return await Task.Run(() =>
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
                    .ToList()
                );
            });
        }
        [HttpPost]
        public async Task<JsonResult> GP(string id) //GETPRESCRIPTION
        {
            return await Task.Run(() =>
            {
                return Json(_context.CasesPrescription
                    .Where(x => x.CaseId == Convert.ToInt32(id))
                    .Select(x => new
                    {
                        PrescriptionID = x.PrescriptionId,
                        PrescriptionDate = x.PrescriptionDate.ToString("yyyy-MM-dd"),
                        Dispensing = x.Dispensing,
                    })
                    .ToList()
                );
            });
        }
        [HttpPost]
        public async Task<JsonResult> GPL(string id) //GETPRESCRIPTIONLIST
        {
            return await Task.Run(() =>
            {
                return Json(_context.CasesPrescriptionlist
                    .Include(x => x.Drug)
                    .Where(x => x.PrescriptionId == Convert.ToInt32(id))
                    .Select(x => new
                    {
                        DrugId = x.DrugId,
                        Name = x.Drug.FDrugName,
                        Days = x.Days,
                        TotalAmount = x.TotalAmount,
                    })
                    .ToList()
                );
            });
        }
        //更新大隊長
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
        //新增大隊長
        [HttpPost]
        public async Task<IActionResult> AddMedicalRecord([FromBody] CasesMedicalRecords record)
        {
            // 在這裡處理表單提交的資料，例如將資料儲存到資料庫中
            _context.CasesMedicalRecords.Add(record);
            await _context.SaveChangesAsync();

            // 返回適當的回應
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddTestReport([FromBody] CasesTestReport record)
        {
            // 在這裡處理表單提交的資料，例如將資料儲存到資料庫中
            _context.CasesTestReport.Add(record);
            await _context.SaveChangesAsync();

            // 返回適當的回應
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] CasesPrescription record)
        {
            // 在這裡處理表單提交的資料，例如將資料儲存到資料庫中
            _context.CasesPrescription.Add(record);
            await _context.SaveChangesAsync();
            int? id = _context.CasesPrescription
                  .OrderBy(p => p.PrescriptionId)
                  .LastOrDefault()?.PrescriptionId;
            if (!id.ToString().IsNullOrEmpty())
            {
                return Ok(id);
            }
            else
            {
                // 返回適當的回應
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddPrescriptionL([FromBody] CasesPrescriptionlist record)
        {
            // 在這裡處理表單提交的資料，例如將資料儲存到資料庫中
            _context.CasesPrescriptionlist.Add(record);
            await _context.SaveChangesAsync();

            // 返回適當的回應
            return Ok();
        }

        public async Task<IActionResult> UpdateMR(int id, [FromBody] CasesMedicalRecords UpdateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            var recordToUpdate = await _context.CasesMedicalRecords.FirstOrDefaultAsync(x => x.MrId == id);
            if (recordToUpdate == null)
            {
                return NotFound(new { success = false, message = "Case not found" });
            }

            //recordToUpdate.Height = UpdateModel.Height;
            //recordToUpdate.Weight = UpdateModel.Weight;
            //recordToUpdate.PastHistory = UpdateModel.PastHistory;
            //recordToUpdate.AllergyRecord = UpdateModel.AllergyRecord;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Record updated successfully" });
        }

        public async Task<IActionResult> GetDrugList()
        {
            var drugList = await _context.PharmacyTMedicinesList
                .Select(x => new
                {
                    DrugId = x.FIdDrug,
                    Name = x.FDrugName,
                })
                .ToListAsync();
            return Json(drugList);
        }

        //刪除大隊長

        [HttpPost]
        public async Task<IActionResult> DMedicalRecord(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }
            var idInt = Convert.ToInt32(id);
            var entity = _context.CasesMedicalRecords.FirstOrDefault(e => e.MrId == idInt);
            if (entity != null)
            {
                _context.CasesMedicalRecords.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return Ok("ok");
        }
        [HttpPost]
        public async Task<IActionResult> DTestReport(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }
            var idInt = Convert.ToInt32(id);
            var entity = _context.CasesTestReport.FirstOrDefault(e => e.ReportId == idInt);
            if (entity != null)
            {
                _context.CasesTestReport.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DPrescription(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }
            var idInt = Convert.ToInt32(id);
            var entity = _context.CasesPrescription.FirstOrDefault(e => e.PrescriptionId == idInt);
            if (entity != null)
            {
                _context.CasesPrescription.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> DPrescriptionL(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }
            var idInt = Convert.ToInt32(id);
            var entities = _context.CasesPrescriptionlist.Where(e => e.PrescriptionId == idInt).ToList();
            if (entities != null && entities.Count > 0)
            {
                _context.CasesPrescriptionlist.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
 }
