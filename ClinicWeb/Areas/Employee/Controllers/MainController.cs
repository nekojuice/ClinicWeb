using ClinicWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [AllowAnonymous]
        public IActionResult Login()
		{
			return View("~/Areas/Employee/Views/Main/Login/Login.cshtml");
		}

        //加上  [AllowAnonymous] 讓登入的不須經過驗證也能使用
        [AllowAnonymous]
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.StaffNumber.ToString()),
                    new Claim("StaffNumber", user.StaffNumber.ToString()),
                    //加上角色設定
                   new Claim(ClaimTypes.Role, user.EmpType)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Content("123");
        }
        [HttpGet]
        public IActionResult noAccess()
        {
          
            return Content("你沒有權限喔喔喔");
        }

        //public class LoginPost
        //{
        //    public int StaffNumber { get; set; }
        //    public string Password { get; set; }
        //}
    }
}
