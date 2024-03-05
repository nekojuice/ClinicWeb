using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class MBAppointmentInfoController : Controller
    {
        private readonly ClinicSysContext _context;

        public MBAppointmentInfoController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 撈取使用者登入資訊
        /// </summary>
        /// <returns>jsonobject</returns>
        public IActionResult Get_MemberStatus()
        {
            var user = HttpContext.User;
            var resultObject = new
            {
                MemberNumber = user.Claims.FirstOrDefault(c => c.Type == "MemberNumber")?.Value,
                MemberName = user.Claims.FirstOrDefault(c => c.Type == "MemberName")?.Value,
                MemberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value
            };
            return Json(resultObject);
        }

        /// <summary>
        /// 撈取該使用者 當前日期以後 的掛號資訊
        /// </summary>
        /// <param name="vm">json物件 成員: today (DateTime)</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Get_MemberAppt([FromBody] DateViewModel vm)
        {
            Console.WriteLine(vm.today.ToString("yyyy/MM/dd"));
            var user = HttpContext.User;
            var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

            return Json(_context.ApptClinicList
                .Where(x => x.MemberId == Convert.ToInt32(memberID) 
                && x.Clinic.Date.CompareTo(vm.today.ToString("yyyy/MM/dd")) >= 0
                && x.IsCancelled == false)
                .Select(x => new
                {
                    id = x.ClinicListId,
                    日期 = x.Clinic.Date,
                    時段 = x.Clinic.ClinicTime.ClinicShifts,
                    科別 = x.Clinic.Doctor.Department,
                    醫師名稱 = x.Clinic.Doctor.Name,
                    診間 = x.Clinic.ClincRoom.Name,
                    診號 = x.ClinicNumber,
                    看診狀態 = x.PatientState.PatientStateName,
                })
                );
        }

        /// <summary>
        /// 退掛指定掛號清單id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Put_CancelMemberAppt(string id)
        {
            try
            {
                var target = _context.ApptClinicList.Where(x => x.ClinicListId == Convert.ToInt32(id)).FirstOrDefault();
                if (target == null) { return NotFound(); }
                else
                {
                    target.IsCancelled = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[偵錯] db update error!!!!");
                return BadRequest();
            }
            Console.WriteLine("[偵錯] else error!!!!");
            return Ok();
        }
    }

    //接收日期用viewmodel
    public class DateViewModel
    {
        public DateTime today { get; set; }
    }
}
