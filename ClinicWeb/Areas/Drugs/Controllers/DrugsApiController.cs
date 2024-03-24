
//using ClinicWeb.Models;
using ClinicWeb.Areas.Drugs.Models;
using ClinicWeb.Areas.Drugs.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using static ClinicWeb.Areas.Drugs.ViewModels.DrugDetailsViewModel;

namespace ClinicWeb.Areas.Drugs.Controllers
{
    [Area("Drugs")]
    public class DrugsApiController : Controller
    {
        //引入 可取得伺服器上的實際路徑的介面
        private readonly IWebHostEnvironment _webHostEnvironment;
        //引入記錄日誌
        private readonly ILogger<DrugsApiController> _logger;

        private readonly ClinicSysContext _context;
        public DrugsApiController(ClinicSysContext context, ILogger<DrugsApiController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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

        //讀取藥品圖片 (顯示資料庫二進位圖片)
        //GET::Drugs/DrugsApi/ShowPicture/id
        [HttpGet]
        public async Task<FileResult> ShowPicture(int id)
        {
            PharmacyTMedicinesList? pic = await _context.PharmacyTMedicinesList.FindAsync(id);
            byte[]? Content = pic?.FApperance;
            return File(Content, "image/jpeg");

        }
        //新增藥品，一次性新增Medicine、TypeDetial、CliniaclUseDetail、SideEffectDetail共4張表

        //新增的Modal上 傳入劑型清單 使用  TypeInfo()--->select option
        //新增的Modal上 傳入適應症與副作用清單 使用 ClinicalUseInfo()、SideEffectInfo() --->checkbox

        #region  
        //測試只新增單一藥品主表
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

        //修改藥品資料--->給modal讀取單一一筆資料 ViewModel
        [HttpGet]
        public async Task<IActionResult> EditDrug(int? id)
        {
            //id = 1; 測試DrugId=1，安寶錠帶入資料是否正確
            if (id == null || _context.PharmacyTMedicinesList == null)
            {
                return NotFound();
            }
            //先找到主表 MedicineList的資料-->拿 FIdDrug
            var MedicineList = await _context.PharmacyTMedicinesList.FindAsync(id);
            if (MedicineList == null)
            {
                return NotFound();
            }

            // 查其他資料表的內容

            //藥品劑型只有是一對一 DrugId-->IdType 記錄在TypeDetail
            var TypeDetails = await _context.PharmacyTTypeDetails.FirstOrDefaultAsync(x => x.FIdDrug == MedicineList.FIdDrug);

            var TypeList = await _context.PharmacyTTypeList.ToListAsync();

            //-------------藥品適應症----------------------

            //step1:藥品適應症是一對多 一筆DrugId-->多筆IdClinicalUse 記錄在ClinicalUseDetails
            var ClinicalUseDetails = await _context.PharmacyTClinicalUseDetails.Where(x => x.FIdDrug == MedicineList.FIdDrug).ToListAsync();

            //step2:藥品適應症清單，開啟畫面時，使用for迴圈生成所有的適應症清單 --> 之前直接使用Detials
            var ClinicalUseList = await _context.PharmacyTClinicalUseList.ToListAsync();

            //step3:確認DB是有適應症清單(有內容)，避免生成畫面出現參考錯誤(NullReferenceException)--> Index 就會直接壞掉
            Console.WriteLine("PharmacyTClinicalUseList Count: " + ClinicalUseList.Count);

            //----------------------------------------------

            //-------------藥品副作用----------------------

            //step1:藥品副作用是一對多 一筆DrugId-->多筆IdSideEffect 記錄在SideEffectDetails
            var SideEffectDetails = await _context.PharmacyTSideEffectDetails.Where(x => x.FIdDrug == MedicineList.FIdDrug).ToListAsync();

            //step2:藥品副作用清單，開啟畫面時，使用for迴圈生成所有的副作用清單
            var SideEffectList = await _context.PharmacyTSideEffectList.ToListAsync();

            //step3:確認DB是有副作用清單(有內容)，避免生成畫面出現參考錯誤(NullReferenceException)--> Index 就會直接壞掉
            Console.WriteLine("PharmaceTSideEffectList Count: " + SideEffectList.Count);

            EditDrugViewModel viewModel;

            viewModel = new EditDrugViewModel
            {
                PharmacyTMedicinesList = MedicineList,
                PharmacyTTypeDetails = TypeDetails,
                PharmacyTTypeList = TypeList,
                PharmacyTClinicalUseList = ClinicalUseList,
                PharmacyTClinicalUseDetails = ClinicalUseDetails,
                PharmacyTSideEffectDetails = SideEffectDetails,
                PharmacyTSideEffectLists = SideEffectList
            };

            return PartialView("~/Areas/Drugs/Views/Partial/_DrugEditPartial.cshtml", viewModel);
        }
        //修改主表可以動了 還有部分未完成-->已完成
        [HttpPost]
        public async Task<IActionResult> EditDrug(int id, [FromForm] EditDrugViewModel viewModel, List<int> FIdClicicalUse, List<int> FIdSideEffect)
        {
            try
            {
                //先找編輯的主表

                var updateData = _context.PharmacyTMedicinesList.Find(id);
                if (updateData != null)
                {
                    updateData.FIdDrug = id;
                    updateData.FDrugCode = viewModel.PharmacyTMedicinesList.FDrugCode;
                    updateData.FGenericName = viewModel.PharmacyTMedicinesList.FGenericName;
                    updateData.FTradeName = viewModel.PharmacyTMedicinesList.FTradeName;
                    updateData.FDrugName = viewModel.PharmacyTMedicinesList.FDrugName;
                    updateData.FPregnancyCategory = viewModel.PharmacyTMedicinesList.FPregnancyCategory;
                    updateData.FDrugDose = viewModel.PharmacyTMedicinesList.FDrugDose;
                    updateData.FMaxDose = viewModel.PharmacyTMedicinesList.FMaxDose;
                    updateData.FDosage = viewModel.PharmacyTMedicinesList.FDosage;
                    updateData.FPrecautions = viewModel.PharmacyTMedicinesList.FPrecautions;
                    updateData.FWarnings = viewModel.PharmacyTMedicinesList.FWarnings;
                    updateData.FStorage = viewModel.PharmacyTMedicinesList.FStorage;
                    updateData.FSupplier = viewModel.PharmacyTMedicinesList.FSupplier;
                    updateData.FBrand = viewModel.PharmacyTMedicinesList.FBrand;
                    await _context.SaveChangesAsync();

                    var updateDataTypeDetail = _context.PharmacyTTypeDetails.FirstOrDefault(x => x.FIdDrug == updateData.FIdDrug);
                    if (updateDataTypeDetail != null)
                    {
                        updateDataTypeDetail.FIdTpye = viewModel.PharmacyTTypeDetails.FIdTpye;
                    }
                    await _context.SaveChangesAsync();

                    var updateDataClinicalDetail = await _context.PharmacyTClinicalUseDetails.Where(x => x.FIdDrug == updateData.FIdDrug).ToListAsync();
                    if (updateDataClinicalDetail.Count > 0)
                    {
                        _context.PharmacyTClinicalUseDetails.RemoveRange(updateDataClinicalDetail);
                    }
                    if (FIdClicicalUse != null && FIdClicicalUse.Any())
                    {
                        foreach (var clinicalUseId in FIdClicicalUse)
                        {
                            var clinicalUseDetails = new PharmacyTClinicalUseDetails { FIdDrug = updateData.FIdDrug, FIdClicicalUse = clinicalUseId };
                            _context.PharmacyTClinicalUseDetails.Add(clinicalUseDetails);
                        }
                    }

                    ////放棄使用EditDrugViewModel viewModel 前端傳值回來viewModel.PharmacyTClinicalUseList都是null
                    ////foreach (var clinicalUseId in viewModel.PharmacyTClinicalUseList)
                    ////{
                    ////    var newClinicalUseDetail = new PharmacyTClinicalUseDetails
                    ////    {
                    ////        FIdDrug = updateData.FIdDrug,
                    ////        FIdClicicalUse = clinicalUseId.FIdClinicalUse
                    ////    };
                    ////    _context.PharmacyTClinicalUseDetails.Add(newClinicalUseDetail);
                    ////}
                    var updateDataSideEffectDetail = await _context.PharmacyTSideEffectDetails.Where(x => x.FIdDrug == updateData.FIdDrug).ToListAsync();
                    if (updateDataSideEffectDetail.Count > 0)
                    {
                        _context.PharmacyTSideEffectDetails.RemoveRange(updateDataSideEffectDetail);

                    }
                    if (FIdSideEffect != null && FIdSideEffect.Any())
                    {
                        foreach (var sideEffectId in FIdSideEffect)
                        {
                            var sideEffectDetials = new PharmacyTSideEffectDetails { FIdDrug = updateData.FIdDrug, FIdSideEffect = sideEffectId };
                            _context.PharmacyTSideEffectDetails.Add(sideEffectDetials);
                        }
                    }
                    ////同上原因
                    ////foreach(var sideEffectId in viewModel.PharmacyTSideEffectDetails)
                    ////{
                    ////    var newSideEffectDetail = new PharmacyTSideEffectDetails
                    ////    {
                    ////        FIdDrug = updateData.FIdDrug,
                    ////        FIdSideEffect = sideEffectId.FIdSideEffect
                    ////    };
                    ////    _context.PharmacyTSideEffectDetails.Add(newSideEffectDetail);
                    ////}
                    await _context.SaveChangesAsync();

                }
                else
                {
                    Console.WriteLine("NotFound");
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            return View("~/Areas/Drugs/Views/Home/Index.cshtml");
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

        //刪除選中的藥品資訊

        public async Task<IActionResult> DeleteDrug(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //先刪除劑型明細表 drugId=id
                    var typeDetials = _context.PharmacyTTypeDetails.FirstOrDefault(x => x.FIdDrug == id);
                    if (typeDetials != null)
                    {
                        _context.PharmacyTTypeDetails.Remove(typeDetials);
                    }
                    //刪除適應症明細表 drugId=id
                    var clinicalUseDetails = _context.PharmacyTClinicalUseDetails.Where(x => x.FIdDrug == id);
                    if (clinicalUseDetails.Any())
                    {
                        _context.PharmacyTClinicalUseDetails.RemoveRange(clinicalUseDetails);
                    }
                    //刪除副作用明細表 drugId=id
                    var sideEffectDetails = _context.PharmacyTSideEffectDetails.Where(x => x.FIdDrug == id);
                    if (sideEffectDetails.Any())
                    {
                        _context.PharmacyTSideEffectDetails.RemoveRange(sideEffectDetails);
                    }
                    await _context.SaveChangesAsync();

                    //最後才能刪除藥品主表
                    var medicine = _context.PharmacyTMedicinesList.FirstOrDefault(x => x.FIdDrug == id);
                    if (medicine == null)
                    {
                        return NotFound();
                    }
                    _context.PharmacyTMedicinesList.Remove(medicine);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return View("~/Areas/Drugs/Views/Home/index.cshtml");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "An error occurred while deleting medicine.");
                    return RedirectToAction("Error", "Home", new { area = "" });
                    throw;
                }
            }
        }

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

        //------------------------------------------------------------------------------------------------

        //新增劑型資料

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

        //刪除選中的適應症清單

        public async Task<IActionResult> DeleteClinicalUse(int id)
        {
            var pharmacyTClinicalUseList = await _context.PharmacyTClinicalUseList.FindAsync(id);
            if (pharmacyTClinicalUseList == null)
            {
                return NotFound();
            }
            _context.PharmacyTClinicalUseList.Remove(pharmacyTClinicalUseList);
            await _context.SaveChangesAsync();
            return View("~/Areas/Drugs/Views/Home/ClinicalUse.cshtml");
        }

        //新增副作用資料
        [HttpPost]
        public async Task<IActionResult> SideEffectCreate(PharmacyTSideEffectList sideEffectList)
        {
            _context.PharmacyTSideEffectList.Add(sideEffectList);
            await _context.SaveChangesAsync();
            return View("~/Areas/Drugs/Views/Home/SideEffect.cshtml");
        }

        //修改副作用資料-->給modal讀單一一筆資料
        [HttpGet]
        public async Task<IActionResult> EditSideEffect(int? id)
        {
            if (id == null || _context.PharmacyTMedicinesList == null)
            {
                return NotFound();
            }
            var pharmacyTSideEffectList = await _context.PharmacyTSideEffectList.FindAsync(id);
            if (pharmacyTSideEffectList == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Drugs/Views/Partial/_SideEffectEditPartial.cshtml", pharmacyTSideEffectList);
        }

        //update回DB
        public async Task<IActionResult> EditSideEffect(PharmacyTSideEffectList pharmacyTSideEffectList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacyTSideEffectList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PharmacyTSideEffectListExists(pharmacyTSideEffectList.FIdSideEffect))
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
            return View("~/Areas/Drugs/Views/Home/SideEffect.cshtml");
        }
        private bool PharmacyTSideEffectListExists(int id)
        {
            return (_context.PharmacyTSideEffectList?.Any(e => e.FIdSideEffect == id)).GetValueOrDefault();
        }

        //刪除選中的副作用清單
        public async Task<IActionResult>DeleteSideEffect(int id)
        {
            var pharmacyTSideEffectList=await _context.PharmacyTSideEffectList.FindAsync(id);
            if (pharmacyTSideEffectList == null)
            {
                return NotFound() ;
            }
            _context.PharmacyTSideEffectList.Remove(pharmacyTSideEffectList);
            await _context.SaveChangesAsync();
            return View("~/Areas/Drugs/Views/Home/SideEffect.cshtml");
        }

        //---------------------------------仿單----------------------------------------------------

        //仿單上傳
        [HttpPost]
        public async Task<IActionResult> DrugFiles(PharmacyHealthInformation drugUpLoad, List<IFormFile> files,int FIdDrug)
        {
            //檔案傳到根目錄的嘗試
            try
            {
                if (files == null || files.Count == 0)
                {
                    // 如果沒有文件被上傳，可能需要返回相應的錯誤訊息或採取其他操作
                    return BadRequest("No files uploaded.");
                }

                //允許多檔上傳
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        //創建一個新的 PharmacyHealthInformation();
                        var newDrugUpload = new PharmacyHealthInformation();

                        //開啟的檔名=存進資料庫檔名
                        
                        string  fileName = file.FileName;
                        
                        //存至wwwroot
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "msit155e", "uploads", fileName);
                        using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                        {
                            file?.CopyTo(fileStream);
                        }
                        
                        // 設置新的 PharmacyHealthInformation 物件的屬性值
                        newDrugUpload.FFileName = fileName;
                        newDrugUpload.FFilePath = uploadPath;
                        newDrugUpload.FIdDrug = FIdDrug;

                        ////新增至資料庫

                        //drugUpLoad.FFileName = fileName;
                        //drugUpLoad.FFilePath = uploadPath;

                        //轉成二進位
                        byte[]? pdfByte = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            file?.CopyTo(memoryStream);
                            pdfByte = memoryStream.ToArray();
                        }
                        newDrugUpload.FFileData = pdfByte;
                       // drugUpLoad.FFileData = pdfByte;
                        _context.PharmacyHealthInformation.Add(newDrugUpload);
                       
                    }
                }
               
                await _context.SaveChangesAsync();
                return View("~/Areas/Drugs/Views/Home/Index2.cshtml");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload failed: {ex.Message}");
            }

            return BadRequest();
        }
        public JsonResult DrugsFileInfo()
        {
            return Json(_context.PharmacyHealthInformation.Select(x => new
            {
                id = x.FIdDrug,
                藥品代碼 = x.FIdDrugNavigation.FDrugCode,
                藥品名稱 = x.FIdDrugNavigation.FDrugName,
                檔案名稱 = x.FFileName
            }));
        }

    }
}
