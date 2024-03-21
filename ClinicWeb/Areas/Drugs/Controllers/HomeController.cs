using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Drugs.Controllers
{
	[Area("Drugs")]
	public class HomeController : Controller
	{
		//首頁：藥品資訊
		public IActionResult Index()
		{
			return View();
		}

		//劑型管理
		[Route("{Areas}/{Controller}/{Action}")]
		public IActionResult Type()
		{
			return View();
		}

        //適應症(藥品作用)管理
        [Route("{Areas}/{Controller}/{Action}")]
        public IActionResult ClinicalUse() 
		{
			return View();
		}

		//副作用管理
		public IActionResult SideEffect() 
		{
			return View();
		}

        public IActionResult Index2()
        {
            return View();
        }

    }
}
