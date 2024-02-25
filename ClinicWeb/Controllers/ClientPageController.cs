using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ClinicWeb.Controllers
{
	public class ClientPageController : Controller
	{

        private ClinicSysContext _context;
        public ClientPageController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
		{
			return View();
		}
		public IActionResult shop()
		{
			return View();
		}
		public IActionResult about()
		{
			return View();
		}
		public IActionResult services()
		{
			return View();
		}
		public IActionResult blog()
		{
			return View();
		}
		public IActionResult contact()
		{
			return View();
		}
        public IActionResult Login()
        {
            return View("~/Views/ClientPage/Login/Login.cshtml");
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginForClient(MemberMemberList m)
        {
            var user = (from a in _context.MemberMemberList
                        where a.MemEmail == m.MemEmail
                        && a.MemPassword == m.MemPassword
                        select a).SingleOrDefault();

            if (user == null)
            {
                ViewData["ErrorMessage"] = "帳號密碼輸入有誤";
                return View("~/Views/ClientPage/Login/Login.cshtml");
            }

            else
            {
                //這邊等等寫驗證加角色
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.MemEmail.ToString()),
                    new Claim("MemberNumber", user.MemberNumber.ToString()),
                     new Claim("MemberName", user.Name),
                      new Claim("MemberID", user.MemberId.ToString()),

                    //加上角色設定
                   //new Claim(ClaimTypes.Role,)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "frontendForCustomer");   //注意!!!
                HttpContext.SignInAsync("frontend", new ClaimsPrincipal(claimsIdentity));    //注意!!!
            }
            //return Content("你是會員 登入成功了 喔喔");
            return Content("你是會員 登入成功了 喔喔");
        }

        //回傳登入者相關資訊
        [AllowAnonymous]
        public IActionResult ProfileforClient()
        {
            var user = HttpContext.User;
            var memberNumber = user.Claims.FirstOrDefault(c => c.Type == "MemberNumber")?.Value;
            var memberName = user.Claims.FirstOrDefault(c => c.Type == "MemberName")?.Value;
          
            var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

            var resultObject = new
            {
                MemberNumber = memberNumber,
                MemberName = memberName,

                MemberID = memberID
            };

            string result = memberID.IsNullOrEmpty() ? "未登入" : JsonConvert.SerializeObject(resultObject);

            //return Content(result, "application/json");
            return Content(result);
        }
    }
}
