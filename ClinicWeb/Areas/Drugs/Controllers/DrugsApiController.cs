
//using ClinicWeb.Models;
using ClinicWeb.Areas.Drugs.Models;
using ClinicWeb.Areas.Drugs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ClinicWeb.Areas.Drugs.ViewModels.DrugDetailsViewModel;

namespace ClinicWeb.Areas.Drugs.Controllers
{
    [Area("Drugs")]
    public class DrugsApiController : Controller
    {
        //引入記錄日誌
        private readonly ILogger<DrugsApiController> _logger;

        private readonly ClinicSysContext _context;
        public DrugsApiController(ClinicSysContext context, ILogger<DrugsApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //讀取DB藥品部分資訊(只有主表PharmacyTMedicinesList)傳回JSON
        //GET:Drugs/DrugsApi/DrugsInfo
        public JsonResult DrugsInfo()
        {
            return Json(_context.PharmacyTMedicinesList.Select(x => new
            {
                id = x.FIdDrug,
                藥品代碼 = x.FDrugCode,
                學名 = x.FGenericName,
                商品名 = x.FTradeName,
                中文名 = x.FDrugName,
                懷孕藥品分級 = x.FPregnancyCategory,
                儲存方法 = x.FStorage
            }));
        }

        //讀取DB藥品資訊含所有明細表內容傳回JSON
        //GET:Drugs/DrugsApi/DrugDatas
        public JsonResult DrugDatas()
        {
            var drugDatas = _context.PharmacyTMedicinesList
                .Include(x => x.PharmacyTTypeDetails)
                .ThenInclude(y => y.FIdTpyeNavigation)
                .Include(x => x.PharmacyTClinicalUseDetails)
                .ThenInclude(y => y.FIdClicicalUseNavigation)
                .Include(x => x.PharmacyTSideEffectDetails)
                .ThenInclude(y => y.FIdSideEffectNavigation)
                .Select(x => new
                {
                    id = x.FIdDrug,
                    藥品代碼 = x.FDrugCode,
                    學名 = x.FGenericName,
                    商品名 = x.FTradeName,
                    中文名 = x.FDrugName,
                    懷孕藥品分級 = x.FPregnancyCategory,
                    儲存方法 = x.FStorage,
                    常用劑量 = x.FDrugDose,
                    最大劑量 = x.FMaxDose,
                    用法 = x.FDosage,
                    禁忌 = x.FPrecautions,
                    注意事項 = x.FWarnings,
                    藥商 = x.FSupplier,
                    廠牌 = x.FBrand,
                    劑型 = x.PharmacyTTypeDetails.Select(d => d.FIdTpyeNavigation.FType).FirstOrDefault(),
                    適應症 = x.PharmacyTClinicalUseDetails.Select(d => d.FIdClicicalUseNavigation.FClinicalUse),
                    副作用 = x.PharmacyTSideEffectDetails.Select(d => d.FIdSideEffectNavigation.FSideEffect)
                });
            return Json(drugDatas);
        }


        //新增藥品

        //新增Modal上 傳入劑型清單 使用  TypeInfo()--->select option
        //新增Modal上 傳入適應症與副作用清單 使用 ClinicalUseInfo()、SideEffectInfo --->checkbox

        #region       
        //[HttpPost]
        //public IActionResult DrugCreate(PharmacyTMedicinesList medicine)
        //{
        //    _context.PharmacyTMedicinesList.Add(medicine);
        //    _context.SaveChanges();
        //    return View("~/Areas/Drugs/Views/Home/index.cshtml");
        //}
        #endregion

        [HttpPost]
        public IActionResult MedicineCreate(PharmacyTMedicinesList medicine, PharmacyTTypeDetails typeDetails, List<int> FIdClicicalUse, List<int> FIdSideEffect)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //先存藥品主表可取得fDrugId
                    _context.PharmacyTMedicinesList.Add(medicine);
                    _context.SaveChanges();

                    //存此藥品劑型種類
                    if (typeDetails != null)
                    {
                        typeDetails.FIdDrug = medicine.FIdDrug;
                        _context.PharmacyTTypeDetails.Add(typeDetails);

                    }

                    //存此藥品適應症明細表
                    if (FIdClicicalUse != null && FIdClicicalUse.Any())
                    {
                        foreach (var id in FIdClicicalUse)
                        {
                            var clinicalUseDetails = new PharmacyTClinicalUseDetails { FIdDrug = medicine.FIdDrug, FIdClicicalUse = id };
                            _context.PharmacyTClinicalUseDetails.Add(clinicalUseDetails);
                        }
                    }

                    //存此藥品副作用明細表
                    if (FIdSideEffect != null && FIdSideEffect.Any())
                    {
                        foreach (var id in FIdSideEffect)
                        {
                            var sideEffectDetails = new PharmacyTSideEffectDetails { FIdDrug = medicine.FIdDrug, FIdSideEffect = id };
                            _context.PharmacyTSideEffectDetails.Add(sideEffectDetails);
                        }
                    }                    

                    _context.SaveChanges();
                    transaction.Commit();
                    return View("~/Areas/Drugs/Views/Home/index.cshtml");

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "An error occurred while creating medicine.");
                    return RedirectToAction("Error", "Home", new { area = "" });
                    throw;
                }
            }
        }


        //public async Task<IActionResult> DrugDetails()
        //{
        //    var List=await _context.PharmacyTClinicalUseList.ToListAsync();

        //    var viewModel = new DrugDetailsViewModel
        //    {
        //        ClinicalUseLists = List.Select(x => new DrugDetailsViewModel.ClinicalUseList
        //        {
        //            ClinicalUseId = x.FIdClinicalUse,
        //            ClinicalUseCode = x.FClinicalUseCode,
        //            ClinicalUseName = x.FClinicalUse
        //        }).ToList()
        //    };
        //    return PartialView("_DrugDetailsPartial", viewModel);
        //}


        //------------------------------------------------------------------------------------------------

        //讀取DB劑型資訊傳回JSON
        //GET:Drugs/DrugsApi/TypeInfo
        public JsonResult TypeInfo()
        {
            return Json(_context.PharmacyTTypeList.Select(x => new
            {
                id = x.FIdType,
                劑型代碼 = x.FTypeCode,
                劑型名稱 = x.FType
            }));
        }

        //讀取DB適應症資訊傳回JSON
        //GET:Drugs/DrugsApi/ClinicalUseInfo       
        public JsonResult ClinicalUseInfo()
        {
            return Json(_context.PharmacyTClinicalUseList.Select(x => new
            {
                id = x.FIdClinicalUse,
                適應症代碼 = x.FClinicalUseCode,
                適應症名稱 = x.FClinicalUse
            }));
        }

        //讀取DB副作用資訊傳回JSON
        //GET:Drugs/DrugsApi/SideEffectInfo
        public JsonResult SideEffectInfo()
        {
            return Json(_context.PharmacyTSideEffectList.Select(x => new
            {
                id = x.FIdSideEffect,
                副作用代碼 = x.FSideEffectCode,
                副作用名稱 = x.FSideEffect
            }));
        }


        //新增適應症資料
        [HttpPost]
        public IActionResult ClinicalUseCreate(PharmacyTClinicalUseList clinicalUseList)
        {
            //return Content("測試API是否有連到前端");
            //var maxClinicalUseId = _context.PharmacyTClinicalUseList.Max(c => c.FIdClinicalUse);
            //var nextClinicalUseId = maxClinicalUseId + 1;
            //clinicalUseList.FIdClinicalUse = nextClinicalUseId;
            _context.PharmacyTClinicalUseList.Add(clinicalUseList);
            _context.SaveChanges();

            return View("~/Areas/Drugs/Views/Home/ClinicalUse.cshtml");
        }

        //修改適應症資料-->給modal讀單一一筆資料
        [HttpGet]
        // [Route("GetClinicalUse/{id}")]
        public async Task<IActionResult> EditClinicalUse(int? id)
        {
            if (id == null || _context.PharmacyTClinicalUseList == null)
            {
                return NotFound();
            }

            var pharmacyTClinicalUseList = await _context.PharmacyTClinicalUseList.FindAsync(id);
            if (pharmacyTClinicalUseList == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Drugs/Views/Partial/_ClinicalUseEditPartial.cshtml", pharmacyTClinicalUseList);
        }
        //Update回DB
        //[Bind("FIdClinicalUse,FClinicalUseCode,FClinicalUse")]
        [HttpPost]
        //[ValidateAntiForgeryToken] 加了這個會BadResqust

        public async Task<IActionResult> EditClinicalUse(PharmacyTClinicalUseList pharmacyTClinicalUseList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacyTClinicalUseList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PharmacyTClinicalUseListExists(pharmacyTClinicalUseList.FIdClinicalUse))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine($"Update failed: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Update failed: {ex.Message}");
                }

            }
            return View("~/Areas/Drugs/Views/Home/ClinicalUse.cshtml");
            // return View(pharmacyTClinicalUseList);
        }

        private bool PharmacyTClinicalUseListExists(int id)
        {
            return (_context.PharmacyTClinicalUseList?.Any(e => e.FIdClinicalUse == id)).GetValueOrDefault();
        }

    }
}
