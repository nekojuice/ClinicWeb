using ClinicWeb.Areas.Member.Models;

using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Security.Claims;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class ClientPageController : Controller
    {


        private ClinicSysContext _context;
        public ClientPageController(ClinicSysContext context)
        {
            _context = context;
        }
        public IActionResult Example()
        {
            return View();
        }
        public IActionResult Essence()
        {
            return View();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)

            //考慮寫成claims.count===0
            {
                //未來會加上會員中心畫面以及個人資料
                return View("~/Views/FMemberB/MemberIndex.cshtml");
                //return Content("現在是登入狀態喔");
            }
            else
            {
                return View("~/Views/ClientPage/Login/ClientLogin.cshtml");
            }
        }

        [AllowAnonymous]
        public IActionResult LoginTest()
        {

            return View("~/Views/ClientPage/Login/ClientLogin.cshtml");

        }

        [AllowAnonymous]
        public IActionResult MemberProfile()
        {

            return View("~/Views/ClientPage/Login/MemberProfile.cshtml");

        }

        [AllowAnonymous]
        public IActionResult MemberProfiletest()
        {

            return View("~/Views/ClientPage/Login/MemberProfiletest.cshtml");

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
                return View("~/Views/ClientPage/Login/ClientLogin.cshtml");
            }

            else
            {
                //這邊等等寫驗證加角色
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.MemEmail.ToString()),
                       new Claim(ClaimTypes.Name, user.Name),
                    new Claim("MemberNumber", user.MemberNumber.ToString()),
                     new Claim("MemberName", user.Name),
                      new Claim("MemberID", user.MemberId.ToString()),


                };
                var claimsIdentity = new ClaimsIdentity(claims, "frontendForCustomer");   //注意!!!
                HttpContext.SignInAsync("frontend", new ClaimsPrincipal(claimsIdentity));    //注意!!!
            }

            //return Content("你是會員 登入成功了 喔喔");
            //登入後進入網頁主畫面 之後會想直接顯示前台會員中心
            return RedirectToAction("Index");
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


            return Content(result, "application/json");
        }


        //寫登出Action
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync("frontend");
                HttpContext.SignOutAsync();
            }
            catch (Exception)
            {
            }

            return RedirectToAction("Login");
            //return Content("123");
        }

        //打開註冊畫面
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("~/Views/ClientPage/Login/Register.cshtml");
        }

        //註冊帳號
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////[Bind("Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,IceNumber,MemPassword,MemEmail,Verification")]
        //public IActionResult Register([Bind("Name,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,IceNumber,MemPassword,MemEmail")][FromBody] MemberMemberList member)
        //{

        //    if (member == null)
        //    {
        //        Console.WriteLine("null~~");
        //    }
        //    else
        //    {
        //        Console.WriteLine(member.Name);
        //        Console.WriteLine(member.Gender);
        //        Console.WriteLine(member.BloodType);
        //        Console.WriteLine(member.NationalId);
        //        Console.WriteLine(member.Address);
        //        Console.WriteLine(member.ContactAddress);
        //        Console.WriteLine(member.Phone);
        //        Console.WriteLine(member.BirthDate);
        //        Console.WriteLine(member.IceName);
        //        Console.WriteLine(member.IceNumber);
        //        Console.WriteLine(member.MemPassword);
        //        Console.WriteLine(member.MemEmail);
        //        Console.WriteLine(member.Verification);
        //    }
        //    //檢查驗證
        //    if (!ModelState.IsValid)
        //    {
        //        // 如果模型驗證失敗，重新顯示包含錯誤信息的表單
        //        Console.WriteLine("驗證失敗");
        //        return Content("~/Areas/Member/Views/Partial/_MemberCreatePartial.cshtml"); // 返回相同的視圖，這將會顯示錯誤信息
        //        //return Json(member);
        //    }
        //    else
        //    {
        //        //加入資料庫

        //        var maxMemberNumber = _context.MemberMemberList.Max(m => m.MemberNumber);
        //        var nextMemberNumber = maxMemberNumber + 1;
        //        member.MemberNumber = nextMemberNumber;

        //        _context.MemberMemberList.Add(member);
        //        _context.SaveChanges();
        //        return Content("success");
        //    }

        //}
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginForClient2(string email, string password)
        {
            var user = (from a in _context.MemberMemberList
                        where a.MemEmail == email
                        && a.MemPassword == password
                        select a).SingleOrDefault();

            if (user == null)
            {
                ViewData["ErrorMessage"] = "帳號密碼輸入有誤";
                return BadRequest("帳號密碼輸入有誤");
            }

            else
            {
                //這邊等等寫驗證加角色
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.MemEmail.ToString()),
                       new Claim(ClaimTypes.Name, user.Name),
                    new Claim("MemberNumber", user.MemberNumber.ToString()),
                     new Claim("MemberName", user.Name),
                      new Claim("MemberID", user.MemberId.ToString()),


                };
                var claimsIdentity = new ClaimsIdentity(claims, "frontendForCustomer");   //注意!!!
                HttpContext.SignInAsync("frontend", new ClaimsPrincipal(claimsIdentity));    //注意!!!
            }

            //return Content("你是會員 登入成功了 喔喔");
            //登入後進入網頁主畫面 之後會想直接顯示前台會員中心
            return Ok("登入成功");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginId()
        { 
            var idText = HttpContext.User.Claims.Where(m=>m.Type== "MemberID").FirstOrDefault()?.Value;
            if (idText == null) { return BadRequest("目前無人登入"); };
            int memberId = Convert.ToInt32(idText);
            return Ok(new { Data = memberId });
        }


        }
}
