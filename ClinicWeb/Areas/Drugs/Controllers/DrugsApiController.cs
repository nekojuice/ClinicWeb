
using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Drugs.Controllers
{
    [Area("Drugs")]
    public class DrugsApiController : Controller
    {
       
        private readonly ClinicSysContext _context;
        public DrugsApiController(ClinicSysContext context) { _context = context; }
      
        //GET:Drugs/DrugsApi/DrugsInfo
        public JsonResult DrugsInfo()
        {
            return Json(_context.PharmacyTMedicinesList.Select(x =>new
            {
              id=x.FIdDrug,
              藥品代碼=x.FDrugCode,
              學名=x.FGenericName,
              商品名=x.FTradeName,
              中文名=x.FDrugName,                           
              懷孕藥品分級=x.FPregnancyCategory,                            
              儲存方法=x.FStorage                        
            }));
        }

        //GET:Drugs/DrugsApi/TypeInfo
        public JsonResult TypeInfo() {
            return Json(_context.PharmacyTTypeList.Select(x => new
            {
                id = x.FIdType,
                劑型代碼=x.FTypeCode,
                劑型名稱=x.FType
            }));
        }
    }
}
