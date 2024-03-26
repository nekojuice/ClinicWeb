using ClinicWeb.Areas.Drugs.Controllers;
using ClinicWeb.Models;
using ClinicWeb.ViewModels;
using System.Collections;
using System.Collections.Generic;
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
        private readonly ClinicWeb.Areas.Drugs.Models.ClinicSysContext _drugs;
        public MBRecordInfoController(ClinicSysContext context, Areas.Drugs.Models.ClinicSysContext drugs)
        {
            _context = context;
            _drugs = drugs;
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
                    Days = x.Days,
                    total = x.TotalAmount,
                })
                );
        }

        // 分支:DrugsDetails_20240312
        
        // Hover--> infoBox

        [HttpGet]
        public IActionResult DrugsDetails(int drugId)
        {
            try
            {
                Console.WriteLine($"Received drugId:{drugId}");


                var list = from a in _context.PharmacyTMedicinesList
                           join b in _context.PharmacyTTypeDetails on a.FIdDrug equals b.FIdDrug
                           join c in _context.PharmacyTTypeList on b.FIdTpye equals c.FIdType
                           where a.FIdDrug == drugId
                           select new
                           {
                               ID = a.FIdDrug,
                               DrugCode = a.FDrugCode,
                               GenericName = a.FGenericName,
                               TradeName = a.FTradeName,
                               DrugName = a.FDrugName,
                               PregnancyCategory = a.FPregnancyCategory,
                               Type = c.FType
                           };


                Console.WriteLine($"Return data count:{list.Count()}");
                var firstItem = list.FirstOrDefault();
                Console.WriteLine($"First item in the list: {firstItem}");


                return Json(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request:{ex.Message}");
                return StatusCode(500, "Internal Server Error");

            }

        }
        [HttpGet]
        public IActionResult ClinicalUseDetails(int drugId)
        {
            try
            {
                Console.WriteLine($"Received drugId:{drugId}");

                var CUList = from a in _context.PharmacyTMedicinesList
                             join b in _context.PharmacyTClinicalUseDetails on a.FIdDrug equals b.FIdDrug
                             join c in _context.PharmacyTClinicalUseList on b.FIdClicicalUse equals c.FIdClinicalUse
                             where a.FIdDrug == drugId
                             select new
                             {
                                 藥品名稱 = a.FDrugName,
                                 適應症 = c.FClinicalUse
                             };

                Console.WriteLine($"Return data count:{CUList.Count()}");
                var firstItem = CUList.FirstOrDefault();
                Console.WriteLine($"First item in the list: {firstItem}");


                return Json(CUList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request:{ex.Message}");
                return StatusCode(500, "Internal Server Error");

            }
        }
        [HttpGet]
        public IActionResult SideEffectDetails(int drugId)
        {
            try
            {
                Console.WriteLine($"Received drugId:{drugId}");
                var SEList = from a in _context.PharmacyTMedicinesList
                             join b in _context.PharmacyTSideEffectDetails on a.FIdDrug equals b.FIdDrug
                             join c in _context.PharmacyTSideEffectList on b.FIdSideEffect equals c.FIdSideEffect
                             where a.FIdDrug == drugId
                             select new
                             {
                                 藥品名稱 = a.FDrugName,
                                 副作用 = c.FSideEffect
                             };
                Console.WriteLine($"Return data count:{SEList.Count()}");
                var firstItem = SEList.FirstOrDefault();
                Console.WriteLine($"First item in the list: {firstItem}");


                return Json(SEList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request:{ex.Message}");
                return StatusCode(500, "Internal Server Error");

            }
        }

        // 藥品明細頁面-->DrugDetails.cshtml
        public IActionResult GetPage(int? drugId)
        {
            //int? drugId = 1;

            var viewModel = GetData(drugId);
            return View("DrugDetails", (DrugDetailsViewModel)viewModel);
        }
        
        //ViewModel
        public DrugDetailsViewModel GetData(int? drugId)
        {
            // 取資料放入viewModel中
            var dataFromDb = drugId.HasValue
                ? _drugs.PharmacyTMedicinesList.Where(d => d.FIdDrug == drugId).ToList()
                : _drugs.PharmacyTMedicinesList.ToList();
            
            //不只一筆的適應症及副作用要用List<string>接資料
            if (dataFromDb.Any())
            {

                var viewModel = new DrugDetailsViewModel
                {
                    DrugId = drugId,
                    DrugCode = dataFromDb.Select(d => d.FDrugCode).FirstOrDefault(),
                    GenericName = dataFromDb.Select(d => d.FGenericName).FirstOrDefault(),
                    TradeName = dataFromDb.Select(d => d.FTradeName).FirstOrDefault(),
                    DrugName = dataFromDb.Select(d => d.FDrugName).FirstOrDefault(),
                    DrugDose = dataFromDb.Select(d => d.FDrugDose).FirstOrDefault(),
                    MaxDose = dataFromDb.Select(d => d.FMaxDose).FirstOrDefault(),
                    Precautions = dataFromDb.Select(d => d.FPrecautions).FirstOrDefault(),
                    Warnings = dataFromDb.Select(d => d.FWarnings).FirstOrDefault(),
                    PregnancyCategory = dataFromDb.Select(d => d.FPregnancyCategory).FirstOrDefault(),
                    Storage = dataFromDb.Select(d => d.FStorage).FirstOrDefault(),
                    Supplier = dataFromDb.Select(d => d.FSupplier).FirstOrDefault(),
                    Brand = dataFromDb.Select(d => d.FBrand).FirstOrDefault(),
                    Dosage = dataFromDb.Select(d => d.FDosage).FirstOrDefault(),
                    ClinicalUse=new List<string>(),
                    SideEffect=new List<string>()                                      
                };

                //使用drugId找TypeDetails的TypeId
                var typeId=_drugs.PharmacyTTypeDetails
                    .Where(x=>x.FIdDrug==drugId)
                    .Select(x=>x.FIdTpye)
                    .FirstOrDefault();
                
                //將劑型加入ViewModel中
                if (typeId != default(int))
                {
                    viewModel.DrugType=_drugs.PharmacyTTypeList
                        .Where(y=>y.FIdType==typeId)
                        .Select(y=>y.FType) .FirstOrDefault();
                }

                //使用drugId找ClinicalUseDetails的ClinicalUseId
                var ClinicalUseId = _drugs.PharmacyTClinicalUseDetails
                    .Where(x => x.FIdDrug == drugId)
                    .Select(x => x.FIdClicicalUse)
                    .ToList();
                
                //將適應症加入ViewModel中
                foreach (var c in ClinicalUseId)
                {
                    var clinicalUse=_drugs.PharmacyTClinicalUseList
                        .Where(y=>y.FIdClinicalUse==c)
                        .Select(y=>y.FClinicalUse) .FirstOrDefault();
                    if (!string.IsNullOrEmpty(clinicalUse))
                    {
                        viewModel.ClinicalUse.Add(clinicalUse);
                    }
                }

                //使用drugId找SideEffectDetails的SideEffectId
                var SideEffectId=_drugs.PharmacyTSideEffectDetails
                    .Where(x=>x.FIdDrug==drugId)
                    .Select (x=>x.FIdSideEffect)
                    .ToList ();
                
                foreach (var s in SideEffectId)
                {
                    var sideEffect=_drugs.PharmacyTSideEffectList
                        .Where(y=>y.FIdSideEffect==s)
                        .Select(y=>y.FSideEffect) .FirstOrDefault();
                    if (!string.IsNullOrEmpty(sideEffect))
                    {
                        viewModel.SideEffect.Add(sideEffect);
                    }
                }
                
                return viewModel;
            }
            throw new Exception("");
        }
        // 分支:Drugs_20240321
       
        public async Task<IActionResult> GetPdf(int drugId)
        {
            
            var pdf = await _drugs.PharmacyHealthInformation.Where(x=>x.FIdDrug==drugId&&x.FFileName.Contains("仿單")).FirstOrDefaultAsync();

            if (pdf == null)
            {
                return NotFound();
            }

            // 返回 PDF 文件的二進位數據
            return File(pdf.FFileData, "application/pdf");
        }

        public async Task<IActionResult> GetOtherPdf(int drugId)
        {

            var pdf = await _drugs.PharmacyHealthInformation.Where(x => x.FIdDrug == drugId && x.FFileName.Contains("用藥教育")).FirstOrDefaultAsync();

            if (pdf == null)
            {
                return NotFound();
            }

            // 返回 PDF 文件的二進位數據
            return File(pdf.FFileData, "application/pdf");
        }

        //補上確認是否找到此drugId檔名是否有 仿單
        public async Task<IActionResult> CheckPdfExistence(int drugId)
        {
            var pdf = await _drugs.PharmacyHealthInformation
                .Where(x => x.FIdDrug == drugId && x.FFileName.Contains("仿單")).FirstOrDefaultAsync();
            if (pdf == null)
            {
                //如果找不到仿單，返回不存在
                return Json(new {exists=false});
            }
            else
            {
                //如果找的到訪單，返回存在
                return Json(new {exists=true});
            }
        }

        //補上確認是否找到此drugId檔案是否有 用藥教育
        public async Task<IActionResult> CheckOtherPdfExistence(int drugId)
        {
            var otherPdf = await _drugs.PharmacyHealthInformation
                .Where(x => x.FIdDrug == drugId && x.FFileName.Contains("用藥教育")).FirstOrDefaultAsync();
            if(otherPdf == null)
            {
                //如果找不到用藥教育，返回不存在
                return Json (new { exists = false });
            }
            else
            {
                //如果找的到用藥教育，返回存在
                return Json(new { exists=true});
            }
        }

    }
}

