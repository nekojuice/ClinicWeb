using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClinicWeb.Areas.Member.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Controllers
{
	[AllowAnonymous]
	public class LoginGoogletestController : Controller
	{
		private ClinicSysContext _context;
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
							new Claim("MemberID", memberId.ToString()),
						};

						
						var claimsIdentity = new ClaimsIdentity(claimsAfterGoogleMatch, "frontendForCustomer");
						await HttpContext.SignInAsync("frontend", new ClaimsPrincipal(claimsIdentity));

				
					}
					else
					{
						// 沒有找到匹配的會員導到註冊頁面
						//return RedirectToAction("註冊頁面action");
						return Json(new { Message = "没有找到匹配的會員" });
					}

				}

				//return Json(claims);
				// return RedirectToAction("Index",new {area=""});
				//要回到之後的會員畫面
				return View("Index");
			}
		}




		//public async Task<IActionResult> GoogleProfile()
		//{
		//	// For all claims
		//	var claims = User.Claims;

		//	// Individual claim value
		//	var name = User.FindFirstValue(ClaimTypes.Name);
		//	var email = User.FindFirstValue(ClaimTypes.Email);


		//	return Json(new { Name = name, Email = email, Claims = claims });
		//}



	}
}
