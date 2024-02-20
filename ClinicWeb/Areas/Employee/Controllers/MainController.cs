using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class MainController : Controller
    {
        private ClinicSysContext _context;
        public MainController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Login()
		{
			return View("~/Areas/Employee/Views/Main/Login/Login.cshtml");
		}

        [HttpPost]
        public IActionResult LoginForStaff(MemberEmployeeList e)
        {
            var user = (from a in _context.MemberEmployeeList
                        where a.StaffNumber == e.StaffNumber
                        && a.EmpPassword == e.EmpPassword
                        select a).SingleOrDefault();

            if (user == null)
            {
                return Content("帳號密碼錯誤");
            }
            else
            {


                //這邊等等寫驗證加角色
            }
            return RedirectToAction("Index");
        }
        //public class LoginPost
        //{
        //    public int StaffNumber { get; set; }
        //    public string Password { get; set; }
        //}
    }
}
