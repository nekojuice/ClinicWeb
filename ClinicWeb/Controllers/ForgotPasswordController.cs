using ClinicWeb.Areas.Member.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace ClinicWeb.Controllers
{
    public class ForgotPasswordController : Controller
    {

        private readonly IEmailService _emailService;
        private readonly ClinicSysContext _context;
        //private readonly UserManager<IdentityUser> _userManager;

        // 合併建構式
        //public ForgotPasswordController(UserManager<IdentityUser> userManager, ClinicSysContext context)
        //{
        //    _userManager = userManager;
        //    _context = context;
        //}

        public ForgotPasswordController(ClinicSysContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public IActionResult ForgotPasswordIndex()
        {
            return View();
        }
        // Controllers/AccountController.cs
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            // 檢查電子郵件是否存在於用戶資料庫中
            var user = await _context.MemberMemberList.FirstOrDefaultAsync(u => u.MemEmail == email);

            if (user == null)
            {
                //TempData["UserNotFound"] = true;
                TempData["ErrorMessage"] = "沒有找到對應的信箱帳戶";
                return RedirectToAction("ForgotPasswordIndex", "ForgotPassword"); 
            }
            else
            {
                // 新增一個重設密碼的連接 
                var passwordResetLink = Url.Action("ResetPassword", "ForgotPassword", new { userId = user.MemberId }, Request.Scheme);
                try
                {
                    await _emailService.SendEmailAsync(email, "重設密碼 ", $"請點擊以下鏈接 重設您的密碼 ：{passwordResetLink}");
                    return Json(new { success = true, message = "郵件發送成功，請檢查您信箱 。" });
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "無法發送郵件，請聯繫管理員 。" });
                }
            }



            //生成密碼重置令牌
           //var token = await _userManager.GeneratePasswordResetTokenAsync(user);

           // // 建立重置密碼連結（這裡需要替換成實際的重置密碼頁面路徑）
           // var passwordResetLink = Url.Action("ResetPassword", "Account", new { token = token }, Request.Scheme);

           // //發送郵件（使用您選擇的郵件服務）
           //  SendEmail(email, passwordResetLink);

            //return View("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            // 確保token有東西
            if (token == null)
            {
                // 有錯的話
            }
            ViewBag.Token = token;
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        // 找不到用戶的話
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        //    if (result.Succeeded)
        //    {
        //        // 密码重置成功，回到登入畫面並顯示成功
        //    }
        //    else
        //    {
        //        // 有錯的話
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //        return View();
        //    }

        //    return RedirectToAction("Login", "ClientPage");
        //}

        public class ResetPasswordViewModel
        {
            public string ?Email { get; set; }
            public string ?Password { get; set; }
            public string ?ConfirmPassword { get; set; }
            [HiddenInput]
            public string ?Token { get; set; }
        }


    }
}
