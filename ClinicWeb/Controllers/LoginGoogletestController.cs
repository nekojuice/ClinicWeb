using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClinicWeb.Areas.Member.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    [AllowAnonymous]
    public class LoginGoogletestController : Controller
    {
        private ClinicSysContext _context;
        private string redirect_uri = "https://localhost:7071/LoginGoogletest/LoginGoogle2";
        private string client_id = "526974450781-r15eef63hrf196gkhsn0tjuqkvk49vjm.apps.googleusercontent.com";
        private string client_secret = "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";

        public LoginGoogletestController(ClinicSysContext c)
        {
            _context = c;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task Logingoogle()
        {
            await HttpContext.ChallengeAsync("Google", new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }




        public async Task<IActionResult> GoogleResponse(MemberMemberList e)
        {
           
            var result = await HttpContext.AuthenticateAsync("Google");

            if (result == null || result.Principal == null)
            {
                // 驗證失敗 之後希望返回原本畫面加上viewdata
                TempData["Msg"] = "驗證 Google 授權失敗";
                //return RedirectToAction("Login", "ClientPage");
                return View("~/Views/ClientPage/Login/ClientLogin.cshtml");
            }
            else
            {

                var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
                {

                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,


                });
                var emailClaim = claims.FirstOrDefault(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                if (emailClaim != null)
                {
                    var email = emailClaim.Value;
                    var matchingMember = _context.MemberMemberList.FirstOrDefault(m => m.MemEmail == email);

                    if (matchingMember != null)
                    {
                        // 找到匹配的會員
                        var memberId = matchingMember.MemberId;

                        // 要來找該名會員的ID
                        var claimsAfterGoogleMatch = new List<Claim>
                        {
                             new Claim(ClaimTypes.Email, matchingMember.MemEmail.ToString()),
                             new Claim(ClaimTypes.Name, matchingMember.Name),
                            new Claim("MemberID", memberId.ToString()),

                        };


                        var claimsIdentity = new ClaimsIdentity(claimsAfterGoogleMatch, "frontendForCustomer");
                        await HttpContext.SignInAsync("frontend", new ClaimsPrincipal(claimsIdentity));


                    }
                    else
                    {
                        // 沒有找到匹配的會員導到註冊頁面
                        //return RedirectToAction("註冊頁面action");
                        //return Json(new { Message = "没有找到匹配的會員" });
                        TempData["RegisterPrompt"] = "沒有找到匹配會員，請點選下方註冊";
                        //沒有匹配會員要倒到註冊頁面並且填上信箱
                        TempData["ForRegister"] = emailClaim.Value;
                        await HttpContext.SignOutAsync();
                        TempData["Error"] = "沒有找到匹配會員，請先註冊帳戶";
                        return RedirectToAction("Register", "ClientPage");
                        //return View("~/Views/ClientPage/Login/Register.cshtml");
                    }


                }

                //return Json(claims);
                // return RedirectToAction("Index",new {area=""});

                //不能回傳view 不然會只是一個頁面 沒有狀態
                return RedirectToAction("Index", "ClientPage");
                //return View("~/Views/ClientPage/Index.cshtml");
            }
        }

        //測試從Google帳戶取出圖片 
        public async Task<IActionResult> GetGoogleUserImage(string imageUrl)
        {
            await HttpContext.ChallengeAsync("Google", new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
            var result = await HttpContext.AuthenticateAsync("Google");

            if (result == null || result.Principal == null)
            {
                // 驗證失敗 之後希望返回原本畫面加上viewdata
                TempData["Msg"] = "驗證 Google 授權失敗";
                //return RedirectToAction("Login", "ClientPage");
                return View("~/Views/ClientPage/Login/ClientLogin.cshtml");
            }
            else
            {

                var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
                {

                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,


                });
                return Json(claims);
            }
        }
        


        //public IActionResult LoginGoogle2(string state, string code)
        //{
        //    //if (TempData["state"] == null)
        //    //{//可能使用者停留Line登入頁面太久
        //    //    return Content("頁面逾期");
        //    //}

        //    //if (Convert.ToString(TempData["state"]) != state)
        //    //{//使用者原先Request QueryString的TempData["state"]和Line導頁回來夾帶的state Querystring不一樣，可能是parameter tampering或CSRF攻擊
        //    //    return Content("state驗證失敗");
        //    //}
        //    //if (Convert.ToString(TempData["state"]) == state)
        //        if (true)
        //    {//state字串驗證通過

        //        //取得id_token和access_token:GET https://www.googleapis.com/drive/v2/files?access_token=access_token 目前只有access
        //        string issue_token_url = "https://oauth2.googleapis.com/token";
        //        var request = new HttpRequestMessage(HttpMethod.Post, issue_token_url);

        //        var postParams = new Dictionary<string, string>
        //            {
        //                {"grant_type", "authorization_code"},
        //                {"code", code},
        //                {"redirect_uri", this.redirect_uri},
        //                {"client_id", this.client_id},
        //                {"client_secret", this.client_secret}
        //            };

        //        request.Content = new FormUrlEncodedContent(postParams);
        //        using (var client = new HttpClient())
        //        {
        //            var response = client.SendAsync(request).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseStr = response.Content.ReadAsStringAsync().Result;

        //                // 在這裡處理 API 回傳的字串 (responseStr)
        //                GoogleLoginToken tokenObj = JsonConvert.DeserializeObject<GoogleLoginToken>(responseStr);
        //                string id_token = tokenObj.id_token;
        //                var jst = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(id_token);
        //                var userId = jst.Payload.Sub;
        //                return Content(userId);
        //            }
        //        }
        //    }
        //    //            var dbMember = _context.MemberMemberList.
        //    //                SingleOrDefault(m => m.IceNumber == userId);
        //    //            //int currentMemberId = GetCurrentMemberId();
        //    //            //沒有綁定的人要登入
        //    //            if (dbMember == null && currentMemberId == 0) { return Content("尚未綁定Google帳號"); }
        //    //            //已登入，在帳號設定頁面請求綁定
        //    //            else if (dbMember == null && currentMemberId != 0)
        //    //            {
        //    //                var toAddGoogleMember = _context.MemberMemberList.SingleOrDefault(m => m.MemberId == currentMemberId);
        //    //                if (toAddGoogleMember == null) { return Content("資料庫系統異常，找不到已登入會員"); }
        //    //                toAddGoogleMember.IceNumber = userId;
        //    //                try
        //    //                {
        //    //                    _context.MemberMemberList.Update(toAddGoogleMember);
        //    //                    _context.SaveChanges();
        //    //                    TempData["Success"] = "Google帳號綁定成功！";
        //    //                    return RedirectToAction("Setting", "Member");
        //    //                }
        //    //                catch (Exception e)
        //    //                {
        //    //                    return Content("資料庫系統異常，無法綁定" + e);
        //    //                }
        //    //            }
        //    //            else if (dbMember != null && currentMemberId != 0)
        //    //            {
        //    //                TempData["Error"] = "此Google帳號已其他帳號綁定！";
        //    //                return RedirectToAction("Setting", "Member");
        //    //            }
        //    //            var claims = new List<Claim>{
        //    //                  new Claim("MemberId", dbMember.MemberId.ToString()),
        //    //                new Claim(ClaimTypes.Name, dbMember.Name),
        //    //                new Claim(ClaimTypes.Email, dbMember.MemEmail)
        //    //                };
        //    //            if (dbMember.Verification)
        //    //            {
        //    //                claims.Add(new Claim(ClaimTypes.Role, "Teacher"));
        //    //            }
        //    //            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    //            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        //    //            return RedirectToAction("Index");
        //    //        }
        //    //        else
        //    //        {
        //    //            // 處理錯誤狀態碼
        //    //            var statusCode = response.StatusCode;
        //    //            // ...

        //    //            return StatusCode((int)statusCode);
        //    //        }
        //    //    } 
        //    //}
        //    return View();
        //}

        public async Task<IActionResult> LoginGoogle2(string state, string code)
        {
            // Google token endpoint
            string issue_token_url = "https://oauth2.googleapis.com/token";
            var request = new HttpRequestMessage(HttpMethod.Post, issue_token_url);

            var postParams = new Dictionary<string, string>
                  {
                    {"grant_type", "authorization_code"},
                    {"code", code},
                    {"redirect_uri", this.redirect_uri},
                    {"client_id", this.client_id},
                    {"client_secret", this.client_secret}
                  };

            request.Content = new FormUrlEncodedContent(postParams);

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();

                    
                    GoogleLoginToken tokenObj = JsonConvert.DeserializeObject<GoogleLoginToken>(responseStr);
                    string ?id_token = tokenObj?.id_token;
                    var jst = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(id_token);
                    var userId = jst.Payload.Sub;
                    var payload = jst.Payload;
                    if (payload.TryGetValue("picture", out var picture))
                    {
                        var userPictureUrl = picture.ToString();
                        GetUserImage(userPictureUrl);
                    }
                    else
                    {
                        // 如果没有找到 "picture"
                    }
                    var dbMember = _context.MemberMemberList.SingleOrDefault(m => m.GoogleSub == userId);

                  
                    int currentMemberId = GetCurrentMemberId();
                
                    if (dbMember == null && currentMemberId == 0) { return Content("尚未綁定Google帳號"); }
                    //已登入，在帳號設定頁面請求綁定
                    else if (dbMember == null && currentMemberId != 0)
                    {
                        var toAddGoogleMember = _context.MemberMemberList.SingleOrDefault(m => m.MemberId == currentMemberId);
                        if (toAddGoogleMember == null) { return Content("資料庫系統異常，找不到已登入會員"); }
                       //先用icenumber測試
                        toAddGoogleMember.GoogleSub = userId;
                        try
                        {
                            _context.MemberMemberList.Update(toAddGoogleMember);
                            _context.SaveChanges();
                            TempData["Success"] = "Google帳號綁定成功！";
                            return RedirectToAction("Index", "ClientPage");
                        }
                        catch (Exception e)
                        {
                            return Content("資料庫系統異常，無法綁定" + e);
                        }
                    }
                    else if (dbMember != null && currentMemberId != 0)
                    {
                        TempData["Error"] = "此Google帳號已其他帳號綁定！";
                        return RedirectToAction("Index", "ClientPage");
                    }
                    var claims = new List<Claim>{
                    new Claim("MemberId", dbMember.MemberId.ToString()),
                    new Claim(ClaimTypes.Name, dbMember.Name),
                    new Claim(ClaimTypes.Email, dbMember.MemEmail)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "frontendForCustomer");
                    await HttpContext.SignInAsync("frontend", new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "ClientPage");
                }
                else
                {
                    // 處理錯誤狀態碼
                    var statusCode = response.StatusCode;
                    // ...

                    return StatusCode((int)statusCode);
                }
            }
        }
        [AllowAnonymous]
        //把抓到的URL轉成檔案
        public async Task<IActionResult> GetUserImage(string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                return File(imageBytes, "image/jpeg"); //
            }
        }

        [AllowAnonymous]
        public int GetCurrentMemberId()
        {
            // 檢查是否有登入的用戶
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // 從Claims中尋找MemberId
                var memberIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "MemberID");
                if (memberIdClaim != null)
                {
                    // 嘗試將MemberId轉換成int型別
                    if (int.TryParse(memberIdClaim.Value, out int memberId))
                    {
                        return memberId;
                    }
                }
            }

            // 如果沒有找到MemberId，或用戶未登入，則返回0或適當的錯誤值
            return 0;
        }







        public class GoogleLoginToken
        {
            public string ?access_token { get; set; }
            public int ?expires_in { get; set; }
            public string ?id_token { get; set; }
            public string ?refresh_token { get; set; }
            public string ?scope { get; set; }
            public string ?token_type { get; set; }
        }
    }
}
