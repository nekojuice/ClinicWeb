using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Temp.Controllers
{
    [Area("Temp")]
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }
        public IActionResult Chartjs()
        {
            return View();
        }
        public IActionResult Chartjs2()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult E_commerce()
        {
            return View();
        }
        public IActionResult Echarts()
        {
            return View();
        }
        public IActionResult Form()
        {
            return View();
        }
        public IActionResult Form_advanced()
        {
            return View();
        }
        public IActionResult Form_buttons()
        {
            return View();
        }
        public IActionResult Form_upload()
        {
            return View();
        }
        public IActionResult Form_validation()
        {
            return View();
        }
        public IActionResult Form_wizards()
        {
            return View();
        }
        public IActionResult General_elements()
        {
            return View();
        }
        public IActionResult Glyphicons()
        {
            return View();
        }
        public IActionResult Icons()
        {
            return View();
        }
        public IActionResult Inbox()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
        public IActionResult Index3()
        {
            return View();
        }
        public IActionResult Invoice()
        {
            return View();
        }
        public IActionResult Level2()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View("/Areas/Temp/Views/Employee/Login/Login.cshtml");
        }
        //Map壞去
        public IActionResult Media_gallery()
        {
            return View();
        }
        public IActionResult Morisjs()
        {
            return View();
        }
        public IActionResult Other_charts()
        {
            return View();
        }
        public IActionResult Page_403()
        {
            return View("/Areas/Temp/Views/Employee/ErrPage/Page_403.cshtml");
        }
        public IActionResult Page_404()
        {
            return View("/Areas/Temp/Views/Employee/ErrPage/Page_404.cshtml");
        }
        public IActionResult Page_500()
        {
            return View("/Areas/Temp/Views/Employee/ErrPage/Page_500.cshtml");
        }
        public IActionResult Plain_page()
        {
            return View();
        }
        public IActionResult Pricing_tables()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Project_detail()
        {
            return View();
        }
        public IActionResult Project()
        {
            return View();
        }
        public IActionResult Tables()
        {
            return View();
        }
    }
}
