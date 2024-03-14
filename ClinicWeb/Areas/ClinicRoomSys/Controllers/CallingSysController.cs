using ClinicWeb.Areas.Appointment.Models;
using ClinicWeb.Areas.ClinicRoomSys.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace ClinicWeb.Areas.ClinicRoomSys.Controllers
{
    /// <summary>
    /// 叫號機專用控制器
    /// </summary>
    [Area("ClinicRoomSys")]
    public class CallingSysController : Controller
    {
        private readonly ClinicSysContext _context;
        public CallingSysController(ClinicSysContext context)
        {
            _context = context;
        }

        //testdata docId=4 20240201 中午
        [HttpPost]
        [Route("{area}/{controller}/{action}")]
        public IActionResult Get_CallingList([FromBody] CallingPanelViewModel vm)
        {
            //var user = HttpContext.User;
            //var EmpIdCookie = user.Claims.FirstOrDefault(c => c.Type == "EmpId")?.Value;

            //[debug] 擷取傳入的值
            //Console.WriteLine($"doctor: { vm.doctorId}, date: {vm.date}, shiftId: {vm.shiftId}");
            return Json(_context.ApptClinicList
                .Where(x => x.Clinic.DoctorId == vm.doctorId
                && x.Clinic.Date == vm.date
                && x.Clinic.ClinicTimeId == vm.shiftId)
                .Select(x => new
                {
                    member_id = x.MemberId,
                    clinicListId = x.ClinicListId,
                    status_id = x.PatientStateId,
                    診號 = x.ClinicNumber,
                    姓名 = x.Member.Name,
                    性別 = x.Member.Gender ? "男" : "女",
                    狀態 = x.PatientState.PatientStateName
                })
                );
        }

        [HttpPost]
        public IActionResult Get_EmpId()
        {
            var user = HttpContext.User;
            var EmpIdCookie = user.Claims.FirstOrDefault(c => c.Type == "EmpId")?.Value ?? "";

            return Content(EmpIdCookie);
        }

        [HttpPost]
        public IActionResult Get_EmpInfo([FromBody] CallingPanelViewModel vm)
        {
            if (vm.doctorId == null || vm.date == null)
            {
                return NotFound();
            }

            return Json(_context.ScheduleClinicInfo.Where(x => x.DoctorId == Convert.ToInt32(vm.doctorId) && x.Date == vm.date)
                .Select(x => new
                {
                    doctorId = x.DoctorId,
                    department = x.Doctor.Department,
                    room = x.ClincRoom.Name,
                    doctorName = x.Doctor.Name
                }).FirstOrDefault());
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult View_CallingPage()
        {
            return View("~/Areas/ClinicRoomSys/Views/CallingPage/View_CallingPage.cshtml");
        }

        //更改跳號序
        public IActionResult Put_JunpState(string clinicInfoId, string stateId)
        {
            return Ok();
        }

        //更改病患報到狀態
        [HttpPost]
        [Route("{area}/{controller}/{action}/{clinicListId}/{stateId}")]
        public async Task<IActionResult> Put_PatientState(string clinicListId, string stateId)
        {
            Console.WriteLine($"{clinicListId}, {stateId}");
            var target = _context.ApptClinicList.Where(x => x.ClinicListId == Convert.ToInt32(clinicListId)).FirstOrDefault();
            if (target == null)
            {
                return NotFound();
            }
            target.PatientStateId = Convert.ToInt32(stateId);
            await _context.SaveChangesAsync();

            return Json(_context.ApptClinicList.Where(x=>x.ClinicListId == Convert.ToInt32(clinicListId)).Select(x=> new
            {
                member_id = x.MemberId,
                clinicListId = x.ClinicListId,
                status_id = x.PatientStateId,
                診號 = x.ClinicNumber,
                姓名 = x.Member.Name,
                性別 = x.Member.Gender ? "男" : "女",
                狀態 = x.PatientState.PatientStateName
            }
            ).FirstOrDefault());
        }
    }
}
