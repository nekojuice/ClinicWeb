using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.ClinicRoomSys.Controllers
{
	/// <summary>
	/// 叫號機專用控制器
	/// </summary>
	[Area("ClinicRoomSys")]
	public class CallingSysController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
