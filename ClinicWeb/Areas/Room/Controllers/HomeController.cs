using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Room.Models;
namespace ClinicWeb.Areas.Room.Controllers
{
	[Area("Room")]
	public class HomeController : Controller
	{
        private readonly ClinicSysContext _context;
        public HomeController(ClinicSysContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			return View();
		}
	}
}
