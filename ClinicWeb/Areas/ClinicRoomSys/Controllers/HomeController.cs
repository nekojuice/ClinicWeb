using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.ClinicRoomSys.Controllers
{
	/// <summary>
	/// 回傳UI頁面專用控制器
	/// </summary>
	[Area("ClinicRoomSys")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult CallingSound() 
		{
			return File("~/msit155e/audio/callingSound.mp3", "audio/mp3");
        }
    }
}
