using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Member.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace ClinicWeb.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IEmailService _emailService;
        private ClinicSysContext _context;
        public RegisterController(ClinicSysContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        //寄信方法呼叫
        [HttpPost]
        public async Task<IActionResult> SendEmail(string emailTo, string subject, string message)
        {
            try
            {
                await _emailService.SendEmailAsync(emailTo, subject, message);
                return Json(new { success = true, message = "註冊成功!" });

            }
            catch (Exception)
            {
                return Json(new { Error = false, message = "註冊時發生錯誤，請聯繫服務人員。" });
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        //啟用成功後顯示的葉面
        public IActionResult ActivateConfirm()
        {
            return View("~/Views/ClientPage/Login/ActivateConfirm.cshtml");
        }


        //生成寄信連結裡面的token
        public static string GenerateActivationToken()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[32]; // 生成驗證token
                rng.GetBytes(tokenData);

                return Convert.ToBase64String(tokenData); // 轉成Base64
            }


        }

        //用戶點選啟用連結
        [HttpGet]
        public async Task<IActionResult> ActivateAccount(string token)
        {
            var member = _context.MemberMemberList.FirstOrDefault(m => m.ActivateToken == token);
            if (member != null)
            {
                member.Verification = true; // 
                await _context.SaveChangesAsync();
                TempData["Success"] = "帳號啟用成功"; // 設置TempData標記
                return View("~/Views/ClientPage/Login/ClientLogin.cshtml"); 
            }
            TempData["Error"] = "帳號啟用失敗，請聯繫我們"; // 設置TempData標記
            return View("~/Views/ClientPage/Login/ClientLogin.cshtml");
        }


        //註冊帳號
        [HttpPost]

        [Route("Register/RegisterMem")]
        //[Bind("Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,IceName,IceNumber,MemPassword,MemEmail,Verification")]
        public async Task<IActionResult> RegisterMem([FromBody] MemberMemberList member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // 會員編號
                    var maxMemNumber = _context.MemberMemberList.Any() ? _context.MemberMemberList.Max(m => m.MemberNumber) : 0;
                    var nextMemNumber = maxMemNumber + 1;
                    member.MemberNumber = nextMemNumber;

                    // 生成啟用連結
                    member.ActivateToken = GenerateActivationToken();
                    member.Verification = false;
                    _context.MemberMemberList.Add(member);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0) // SaveChangesAsync返回影響的行數，所以應該檢查它是否大於0
                    {
                        var activationLink = Url.Action("ActivateAccount", "Register",
                            new { token = member.ActivateToken }, protocol: HttpContext.Request.Scheme);

                        // 發送啟用的郵件
                        try
                        {
                            await _emailService.SendEmailAsync(member.MemEmail, "註冊通知", $"請點擊以下連結啟用您的帳戶 ：{activationLink}");
                        }
                        catch (Exception)
                        {

                            return Json(new { error = false, message = "註冊成功，但發送啟用郵件失敗，請聯繫管理員。" });
                        }

                        return Json(new { success = true, message = "註冊成功，請去信箱點選啟用帳戶連結。\n點選確認回到登入畫面。" });

                    }
                    else
                    {
                        // 沒辦法把資料新增到資料庫 
                        return Json(new { success = false, message = "註冊失敗，資料庫異常，請聯繫管理員 。" });
                    }
                }
            }
            catch (Exception )
            {
             // 其他未知異常
                    return Json(new { success = false, message = "註冊失敗，未知錯誤，請聯繫系統管理員 。" });
            }

            // 默認返回，防止出現未處理的情況
            return Json(new { error = false, message = "註冊失敗，請檢查輸入的數據。" });
        }
    }
}
