
//using ClinicWeb.Models;
using ClinicWeb.Areas.Drugs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Drugs.Controllers
{
    [Area("Drugs")]
    public class DrugsApiController : Controller
    {

        private readonly ClinicSysContext _context;
        public DrugsApiController(ClinicSysContext context) { _context = context; }

        //讀取DB藥品資訊傳回JSON
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
        //public JsonResult DrugsDetail(int id) 
        //{
        //    return Json(_context.PharmacyTMedicinesList
        //        .Where(x=>x.FIdDrug==id)
        //        .Include(x=>x.PharmacyTTypeDetails)                
        //        .Select(x=>new 
        //        {
        //            識別碼 = x.FIdDrug,
        //            劑型=x.PharmacyTTypeDetails

        //        }));
                    
        //}

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

        //讀取劑型明細：編輯
        //public IActionResult Details(int? id)
        //{
        //    if(id == null || _context.PharmacyTTypeList==null) 
        //    {
        //        return NotFound();
        //    }
        //    var Type = _context.PharmacyTTypeList.FirstOrDefault(m=>m.FIdType==id);
        //    if(Type == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(Type);
        //}

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

            return View("~/Areas/Drugs/Views/ClinicalUse.cshtml");
        }

        //修改適應症資料
       
        public async Task<IActionResult> ClinicalUseEdit(int? clinicaluseId)
        {
            if (clinicaluseId == null || _context.PharmacyTClinicalUseList==null)
            {
                return NotFound();
            }

            var pharmacyTClinicalUseList = await _context.PharmacyTClinicalUseList.FindAsync(clinicaluseId);
            if (pharmacyTClinicalUseList == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Drugs/Views/Partial/__ClinicalUseEditPartial.cshtml", pharmacyTClinicalUseList);
        }

    }
}
