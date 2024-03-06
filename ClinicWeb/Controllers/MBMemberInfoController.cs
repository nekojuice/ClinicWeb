using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
//using ClinicWeb.Models;
using ClinicWeb.Areas.Member.Models;


namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class MBMemberInfoController : Controller
    {
        private ClinicSysContext _context;
        public IActionResult Index()
        {
            var user = HttpContext.User;
            var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

            if (string.IsNullOrEmpty(memberID))
            {
                return Content("未登入", "application/json");
            }

            var memberInfo = _context.MemberMemberList
                                .FirstOrDefault(m => m.MemberId == Convert.ToInt32(memberID));
            
            
            if (memberInfo == null)
            {
                return Content("找不到會員資料", "application/json");
            }


            return View(memberInfo);
        }

        public MBMemberInfoController(ClinicSysContext context)
        {
            _context = context;
        }
        public IActionResult MemberProfile()
        {
            return PartialView("~/Views/FMemberB/PartialView/_MemberProfilePartial.cshtml");
        }

        //編輯資料時檢查該筆會員id是否存在的函數
        private bool MemberMemberListExists(int memberId)
        {
            return _context.MemberMemberList.Any(e => e.MemberId == memberId);
        }

        // 修改會員資料
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberMemberList member)
        {
            member.Verification = false; // 在這裡設置 Verification 為 false

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();

                    // 修改成功，顯示 SweetAlert
                    TempData["SuccessMessage"] = "資料修改成功！"; // 在 TempData 中存儲成功消息
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberMemberListExists(member.MemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return Content("驗證未通過");
            }

            // 返回原頁面
            return RedirectToAction("Index"); 
        }

    }
}
