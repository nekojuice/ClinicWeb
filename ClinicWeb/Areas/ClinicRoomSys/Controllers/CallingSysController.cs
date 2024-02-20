using ClinicWeb.Areas.Appointment.Models;
using ClinicWeb.Areas.ClinicRoomSys.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            //[debug] 擷取傳入的值
            //Console.WriteLine($"doctor: { vm.doctorId}, date: {vm.date}, shiftId: {vm.shiftId}");
            return Json(_context.ApptClinicList
                .Where(x => x.Clinic.DoctorId == vm.doctorId
                && x.Clinic.Date == vm.date
                && x.Clinic.ClinicTimeId == vm.shiftId)
                .Select(x=>new {
					member_id = x.MemberId,
					status_id = x.PatientStateId,
					診號 = x.ClinicNumber,
					姓名 = x.Member.Name,
					性別 = x.Member.Gender? "男":"女",
					狀態 = x.PatientState.PatientStateName
				})
                );
        }
    }
}
