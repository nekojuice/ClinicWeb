using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Appointment.Controllers
{
    /// <summary>
    /// 掛號管理控制器
    /// </summary>
    /// <author>nekojuice</author>
    [Area("Appointment")]
    public class ApptSysController : Controller
    {
        private ClinicSysContext _context;
        public ApptSysController(ClinicSysContext context) { _context = context; }

		/// <summary>
		/// 顯示 掛號管理頁面
		/// </summary>
		/// <returns>[html] Index</returns>
		public IActionResult Index()
        {
            var result = (from tSchedule in _context.ScheduleClinicInfo select tSchedule.Date.Substring(0, 7)).Distinct();

            ViewBag.Date = new SelectList(_context.ScheduleClinicInfo
                .Select(tSchedule => tSchedule.Date.Substring(0, 7))
                .Distinct());

            ViewBag.Department = new SelectList(_context.MemberEmployeeList
                .Select(x => x.Department)
                .Distinct());

            ViewBag.ClinicShifts = new SelectList(_context.ScheduleClinicTime.Select(x => x.ClinicShifts));

            return View();
        }

		//[Route("{area}/{controller}/{action}/{year}/{month}")]
		//public IActionResult ClinicInfo(string year, string month)
		//{
		//    var apptableMonth = from x in _context.ScheduleClinicInfo
		//                        where x.Date.StartsWith($"{year}/{month}")
		//                        select x;
		//    if (apptableMonth == null)
		//    {
		//        return NotFound();
		//    }
		//    else
		//    {
		//        return PartialView("_ClinicInfoPartial", apptableMonth);
		//    }
		//}

		/// <summary>
		/// 以 指定年月 撈取所有已排班的開診資訊
		/// </summary>
		/// <param name="year">指定年</param>
		/// <param name="month">指定月</param>
		/// <returns>[JSON] 該月開診資訊，多筆</returns>
		[Route("{area}/{controller}/{action}/{year}/{month}")]
        [HttpGet]
        public JsonResult GET_ClinicInfoList(string year, string month)
        {
            return Json(_context.ScheduleClinicInfo
                .Where(x => x.Date.StartsWith($"{year}/{month}"))
                .Include(x => x.Doctor)
                .Include(x => x.ClincRoom)
                .Include(x => x.ClinicTime)
                .Include(x => x.ApptClinicList)
                .Select(x => new
                {
                    id = x.ClinicInfoId,
                    日期 = x.Date,
                    時段 = x.ClinicTime.ClinicShifts,
                    科別 = x.Doctor.Department,
                    醫師名稱 = x.Doctor.Name,
                    上限人數 = x.RegistrationLimit,
                    預約人數 = x.ApptClinicList.Count(y=>y.IsCancelled==false)
                }));
        }

		/// <summary>
		/// 撈取該診掛號清單
		/// </summary>
		/// <param name="id">開診資訊id ClinicInfoId (int)</param>
		/// <returns>[JSON] 該診掛號清單，多筆</returns>
		[HttpGet]
		public JsonResult GET_ApptRecordList(string id)
        {
            return Json(_context.ApptClinicList
                .Where(x => x.ClinicId == Convert.ToInt32(id))
                .Include(x => x.Member)
                .Select(x => new
                {
                    clinicAppt_id = x.ClinicListId,
                    member_id = x.MemberId,
                    診號 = x.ClinicNumber,
                    姓名 = x.Member.Name,
                    生日 = x.Member.BirthDate.ToString("yyyy/MM/dd"),
                    性別 = x.Member.Gender ? "男" : "女",
                    身分證字號 = x.Member.NationalId,
                    退掛 = x.IsCancelled ? "是" : "否",
                    看診狀態 = x.PatientState.PatientStateName
                }));
        }

		/// <summary>
		/// 快捷部分搜尋
		/// 簡易版會員資訊
		/// </summary>
		/// <param name="id">會員身分證字號 NationalId (string)</param>
		/// <returns>[JSON] 簡易版會員資訊，最大5筆</returns>
		[HttpGet]
		public JsonResult GET_MemberDataSnap(string id)
        {
            return Json(_context.MemberMemberList
                .Where(x => x.NationalId.Contains(id))
                .Select(x => new
                {
                    id = x.MemberId,
                    身分證字號 = x.NationalId,
                    姓名 = x.Name,
                    性別 = x.Gender ? "男" : "女",
                    生日 = x.BirthDate.ToString("yyyy-MM-dd")
                })
                .Take(5)
                );
        }

		/// <summary>
		/// !!敏感資訊!!
		/// 搜尋詳細會員資料
		/// </summary>
		/// <param name="id">會員id MemberId (int)</param>
		/// <returns>[JSON] 詳細會員資料，1筆</returns>
		[HttpPost]
        public JsonResult GET_MemberData(string id)
        {
            return Json(_context.MemberMemberList
                .Where(x => x.MemberId == Convert.ToInt32(id))
                .Select(x => new
                {
                    id = x.MemberId,
                    MemberNumber = x.MemberNumber,
                    NationalId = x.NationalId,
                    Name = x.Name,
                    Gender = x.Gender ? "男" : "女",
                    BirthDate = x.BirthDate.ToString("yyyy-MM-dd"),
                    BloodType = x.BloodType,
                    ContactAddress = x.ContactAddress,
                    Phone = x.Phone,
                    MemEmail = x.MemEmail,
                    IceName = x.IceName,
                    IceNumber = x.IceNumber
                })
                .FirstOrDefault()
                );
        }

		/// <summary>
		/// 執行掛號
		/// </summary>
		/// <param name="clinicId">開診資訊id ClinicId (int)</param>
		/// <param name="memberId">會員id MemberId (int)</param>
		/// <param name="isVIP">是否優待 IsVip (bool)</param>
		/// <returns>[text] 是否重複掛號 isDuplicate (bool)</returns>
		[Route("{area}/{controller}/{action}/{clinicId}/{memberId}/{isVIP}")]
        [HttpPost]
        public async Task<IActionResult> Add_ApptRecord(string clinicId, string memberId, string isVIP)
        {
            bool isDuplicate = _context.ApptClinicList
                .Where(x => x.ClinicId == Convert.ToInt32(clinicId) && x.MemberId == Convert.ToInt32(memberId))
                .Any();
            //非重複掛號時
            if (!isDuplicate)
            {
                //計算診號邏輯
                //未有已掛號號碼，初始計算值
                int maxClinicNumber = Convert.ToBoolean(isVIP) ? -1 : 0;
                //如果有已掛號號碼，撈取最大值
                try
                {
                    maxClinicNumber = _context.ApptClinicList
                                    .Where(x => x.IsVip == Convert.ToBoolean(isVIP) && x.ClinicId == Convert.ToInt32(clinicId))
                                    .Select(x => x.ClinicNumber)
                                    .Max();
                }
                catch (Exception) { }
                //Console.WriteLine($"clinicId={clinicId} memberId={memberId} isVIP={isVIP}");
                try
                {
                    ApptClinicList newappt = new ApptClinicList
                    {
                        ClinicId = Convert.ToInt32(clinicId),
                        MemberId = Convert.ToInt32(memberId),
                        IsVip = Convert.ToBoolean(isVIP),
                        ClinicNumber = maxClinicNumber + 2,
                        PatientStateId = 8,
                        IsCancelled = false
                    };

                    _context.ApptClinicList.Add(newappt);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Content(isDuplicate.ToString());
        }

		/// <summary>
		/// 1. 用於顯示查詢彈跳視窗
		/// 2. 用於更新掛號紀錄單個row
		/// </summary>
		/// <param name="id">掛號紀錄id ClinicListId (int)</param>
		/// <returns>[JSON] 複合資料: 會員資料</returns>

		[HttpPost]
        public JsonResult GET_ApptRecordOne(string id)
        {
            return Json(_context.ApptClinicList
                .Where(x => x.ClinicListId == Convert.ToInt32(id))
                .Select(x => new
                {
                    clinicAppt_id = x.ClinicListId,
                    member_id = x.MemberId,
                    會員號碼 = x.Member.MemberNumber,
                    診號 = x.ClinicNumber,
                    姓名 = x.Member.Name,
                    生日 = x.Member.BirthDate.ToString("yyyy/MM/dd"),
                    性別 = x.Member.Gender ? "男" : "女",
                    血型 = x.Member.BloodType,
                    身分證字號 = x.Member.NationalId,
                    退掛 = x.IsCancelled ? "是" : "否",
                    看診狀態 = x.PatientState.PatientStateName,

                    日期 = x.Clinic.Date,
                    時段 = x.Clinic.ClinicTime.ClinicShifts,
                    科別 = x.Clinic.Doctor.Department,
                    醫師名稱 = x.Clinic.Doctor.Name
                })
                .FirstOrDefault()
                );
        }

		/// <summary>
		/// 修改掛號紀錄
		/// </summary>
		/// <param name="id">掛號紀錄id ClinicListId (int)</param>
		/// <param name="cancelled">是否退掛 IsCancelled (bool)</param>
		/// <returns>[JSON] 複合資料: 會員資料</returns>
		[Route("{area}/{controller}/{action}/{id}/{cancelled}")]
        [HttpPost]
        public async Task<IActionResult> PUT_ApptRecord_Cancelled(string id, string cancelled)
        {
            try
            {
                var target = _context.ApptClinicList
                    .Where(x => x.ClinicListId == Convert.ToInt32(id))
                    .First();
                target.IsCancelled = Convert.ToBoolean(cancelled);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
            return GET_ApptRecordOne(id);
        }

		/// <summary>
		/// 刪除掛號紀錄
		/// </summary>
		/// <param name="id">掛號紀錄id ClinicListId (int)</param>
		/// <returns>[text] "Success" 或 NotFound()</returns>
		[HttpPost]
        public async Task<IActionResult> DEL_ApptRecordOne(string id)
        {
            try
            {
                var target = _context.ApptClinicList
					.Where(x => x.ClinicListId == Convert.ToInt32(id))
					.First();
                _context.ApptClinicList.Remove(target);
                await _context.SaveChangesAsync();
                return Content("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

		/// <summary>
		/// 更新當診 已掛號人數
		/// 掛號、退掛、刪除紀錄時撈取
		/// </summary>
		/// <param name="id">開診資訊id ClinicInfoId (int)</param>
		/// <returns>[JSON] 預約人數 (int) 或 NotFound()</returns>
		[HttpPost]
        public IActionResult GET_ClinicPatientNumber(string id)
        {
            try
            {
                return Json(_context.ScheduleClinicInfo
                    .Where(x => x.ClinicInfoId == Convert.ToInt32(id))
                    .Select(x => new
                    {
                        預約人數 = x.ApptClinicList.Count(y => y.IsCancelled == false)
                    })
                    .First()
                    );
            }
            catch (Exception)
            {
                return NotFound();
            }

        }
        public IActionResult test1()
        {
            return View();
        }
    }
}
