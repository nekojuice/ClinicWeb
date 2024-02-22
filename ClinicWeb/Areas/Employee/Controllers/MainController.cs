using ClinicWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ClinicWeb.Areas.Employee.Controllers
{

    [Area("Employee")]
    public class MainController : Controller
    {
        private ClinicSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MainController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //測試業面是否知道登入者是誰
        public IActionResult who()
        {
            HttpContext.User.Claims.ToList();
            return Content("");
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
                     new Claim("EmpName", user.Name),
                      new Claim("EmpId", user.EmpId.ToString()),

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

        //取得httpcontext的使用者資訊
        //[AllowAnonymous]

        //public IActionResult Profile()
        //{

        //    var user = HttpContext.User;
        //    var staffNumber = user.Claims.FirstOrDefault(c => c.Type == "StaffNumber")?.Value;
        //    var empType = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        //    string result = staffNumber.IsNullOrEmpty() ? "未登入" : staffNumber;
        //    // 根據需要從用戶身份中獲取其他信息

        //    return Content(result);
        //}

        [AllowAnonymous]
        public IActionResult Profile()
        {
            var user = HttpContext.User;
            var staffNumber = user.Claims.FirstOrDefault(c => c.Type == "StaffNumber")?.Value;
            var staffName=user.Claims.FirstOrDefault(c=>c.Type== "EmpName")?.Value;
            var empType = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var resultObject = new
            {
                StaffNumber = staffNumber,
                StaffName = staffName,
                EmpType = empType
            };

            string result = staffNumber.IsNullOrEmpty() ? "未登入" : JsonConvert.SerializeObject(resultObject);

            return Content(result, "application/json");
        }


    }
}
