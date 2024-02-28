using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ClinicWeb.Controllers
{
    [AllowAnonymous]
    public class LoginGoogletestController : Controller
    {
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

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Google");

            if (result == null || result.Principal == null)
            {
                // 驗證失敗 之後希望返回原本畫面加上viewdata
                ViewData["Msg"] = "驗證 Google 授權失敗";
                return Content("GoogleLoginFailed");
            }
            else
            {
                var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,
                    ClaimTypes.Name
                });
                //return Json(claims);
                return RedirectToAction("Index",new {area=""});
            }
        }


        public async Task<IActionResult> GoogleLogout()
        {
            await HttpContext.SignOutAsync();
            return Content("你成功用googel登出了喔");
        }
    }
}
