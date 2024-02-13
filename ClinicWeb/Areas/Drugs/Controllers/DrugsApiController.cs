
//using ClinicWeb.Models;
using ClinicWeb.Areas.Drugs.Models;
using Microsoft.AspNetCore.Mvc;

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

        //讀取DB劑型資訊傳回JSON
        //GET:Drugs/DrugsApi/TypeInfo
        public JsonResult TypeInfo() {
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
    }
}
