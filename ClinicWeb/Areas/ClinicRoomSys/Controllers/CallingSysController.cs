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
        //testdata docId=4 20240201 中午
        [HttpPost]
        [Route("{area}/{controller}/{action}")]
        public IActionResult Get_CallingList([FromBody] CallingPanelViewModel vm)
        {
            Console.WriteLine(vm.doctorId.ToString());
            Console.WriteLine(vm.date);
            Console.WriteLine(vm.shiftId.ToString());
            return Json("");
        }
    }
}
