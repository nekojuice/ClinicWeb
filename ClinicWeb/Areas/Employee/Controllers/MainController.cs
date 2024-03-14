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

			//if (user == null)
			//{
			//    return Content("帳號密碼錯誤");
			//}
			if (user == null)
			{
				ViewData["ErrorMessage"] = "帳號密碼輸入有誤";
				return View("~/Areas/Employee/Views/Main/Login/Login.cshtml");
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
                var claimsIdentity = new ClaimsIdentity(claims, "backendForStaff");   //注意!!!
                HttpContext.SignInAsync("backend", new ClaimsPrincipal(claimsIdentity));    //注意!!!
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("backend");    //注意!!!
            return RedirectToAction("Login");
			//return Content("123");
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
        public IActionResult ProfileforStaff()
        {
            var user = HttpContext.User;
            var staffNumber = user.Claims.FirstOrDefault(c => c.Type == "StaffNumber")?.Value;
            var staffName=user.Claims.FirstOrDefault(c=>c.Type== "EmpName")?.Value;
            var empType = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var empIdCookie = user.Claims.FirstOrDefault(c => c.Type == "EmpId")?.Value;

            var resultObject = new
            {
                StaffNumber = staffNumber,
                StaffName = staffName,
                EmpType = empType,
                EmpIdCookie= empIdCookie
            };

            string result = staffNumber.IsNullOrEmpty() ? "未登入" : JsonConvert.SerializeObject(resultObject);

            return Content(result, "application/json");
        }

        [AllowAnonymous]
        public IActionResult ProfileForPicture()
        {
            var user = HttpContext.User;
            var EmpIdCookie = user.Claims.FirstOrDefault(c => c.Type == "EmpId")?.Value;

            // 從資料庫中查詢員工信息
            var employee = _context.MemberEmployeeList.FirstOrDefault(e => (e.EmpId).ToString() == EmpIdCookie);

            // 如果找到了對應的員工且員工有大頭照數據
            if (employee != null && employee.EmpPhoto != null && employee.EmpPhoto.Length > 0)
            {
                                return File(employee.EmpPhoto, "image/jpeg"); 
            }
            else
            {
              
                return NotFound(); 
            }
        }


    }
}
