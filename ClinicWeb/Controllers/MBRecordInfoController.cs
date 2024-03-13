using ClinicWeb.Areas.Drugs.Controllers;
using ClinicWeb.Models;
using ClinicWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace ClinicWeb.Controllers
{
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
        public IActionResult DrugDetails()
        {
            int? drugId = 2;

            var viewModel = GetData(drugId);
            return View("DrugDetails",(DrugDetailsViewModel)viewModel);
        }
        //ViewModel
        public DrugDetailsViewModel GetData(int? drugId)
        {
            // 取
            var dataFromDb=drugId.HasValue
                ?_context.PharmacyTMedicinesList.Where(d=>d.FIdDrug==drugId).ToList()
                :_context.PharmacyTMedicinesList.ToList();


            var viewModel = new DrugDetailsViewModel
            {
                DrugId = drugId,
                DrugCode = dataFromDb.Select(d => d.FDrugCode).FirstOrDefault(),
                GenericName = dataFromDb.Select(d => d.FGenericName).FirstOrDefault(),
                TradeName = dataFromDb.Select(d => d.FTradeName).FirstOrDefault(),
                DrugName = dataFromDb.Select(d => d.FDrugName).FirstOrDefault()
            };
            return viewModel;
        }


    }
}
