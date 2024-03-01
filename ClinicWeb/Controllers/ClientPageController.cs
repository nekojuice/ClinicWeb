using ClinicWeb.Areas.Appointment.Models;
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
				return Content("現在是登入狀態喔");
			}
			else
			{
				return View("~/Views/ClientPage/Login/Login.cshtml");
			}
		}

        [AllowAnonymous]
        public IActionResult LoginTest()
        {
           
                return View("~/Views/ClientPage/Login/LoginTest.cshtml");
            
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


        //google登入action
        [AllowAnonymous]
        public async Task<IActionResult> ValidGoogleLogin()
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = await VerifyGoogleToken(formCredential, formToken, cookiesToken);
            if (payload == null)
            {
                // 驗證失敗
                ViewData["Msg"] = "驗證 Google 授權失敗";
            }
            else
            {
                //驗證成功，取使用者資訊內容
                ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";
                ViewData["Msg"] += "Email:" + payload.Email + "<br>";
                ViewData["Msg"] += "Name:" + payload.Name + "<br>";
                ViewData["Msg"] += "Picture:" + payload.Picture;
            }

            return View();
        }


        /// <summary>
        /// 驗證 Google Token
        /// </summary>
        /// <param name="formCredential"></param>
        /// <param name="formToken"></param>
        /// <param name="cookiesToken"></param>
        /// <returns></returns>
        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }

       
    }
}
